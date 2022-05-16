﻿using System;
using VL.Lib.IO.Notifications;
using VL.Skia;
using ImGuiNET;
using System.Numerics;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace VL.ImGui.Skia
{
    using ImGui = ImGuiNET.ImGui;

    internal delegate void WidgetFunc(CallerInfo canvas, SKRect clipBounds);

    public class ToSkiaLayer : IDisposable, ILayer
    {
        readonly struct Handle<T> : IDisposable
            where T : class
        {
            private readonly GCHandle _handle;

            public Handle(T skiaObject)
            {
                _handle = GCHandle.Alloc(skiaObject);
            }

            public T? Target => _handle.Target as T;

            public IntPtr Ptr => GCHandle.ToIntPtr(_handle);

            public void Dispose()
            {
                _handle.Free();
            }
        }

        readonly ImGuiIOPtr _io;

        Widget? _widget;

        // OpenGLES rendering (https://github.com/dotnet/Silk.NET/tree/v2.15.0/src/OpenGL/Extensions/Silk.NET.OpenGL.Extensions.ImGui)
        private readonly SkiaContext _context;
        private readonly RenderContext _renderContext;
        private readonly Handle<SKPaint> _fontPaint;

        public ToSkiaLayer()
        {
            _context = new SkiaContext();
            using (_context.MakeCurrent())
            {
                _io = ImGui.GetIO();
            }

            _renderContext = RenderContext.ForCurrentThread();

            _fontPaint = new Handle<SKPaint>(new SKPaint());
            build_ImFontAtlas(_io.Fonts, _fontPaint);
        }

        public ILayer Update(Widget widget)
        {
            _widget = widget;
            return this;
        }

        public unsafe void Render(CallerInfo caller)
        {
            using (_context.MakeCurrent())
            {
                var bounds = caller.ViewportBounds;
                _io.DisplaySize = new Vector2(bounds.Width, bounds.Height);

                _context.NewFrame();
                try
                {
                    // ImGui.ShowDemoWindow();
                    _context.Update(_widget);
                }
                finally
                {
                    // Render (builds mesh with texture coordinates)
                    ImGui.Render();
                }

                // Render the mesh
                var drawDataPtr = ImGui.GetDrawData();
                Render(caller, drawDataPtr);
            }
        }

        static void build_ImFontAtlas(ImFontAtlasPtr atlas, Handle<SKPaint> paintHandle)
        {
            atlas.AddFontDefault();

            var fontsfolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            using var defaultTypeFace = SKTypeface.CreateDefault();
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                defaultTypeFace.FamilyName.Replace(" ", ""),
                "arial",
                "tahoma"
            };
            foreach (var fontPath in Directory.GetFiles(fontsfolder, "*.ttf").Where(p => names.Contains(Path.GetFileNameWithoutExtension(p))))
            {
                ImFontConfig cfg = new ImFontConfig()
                {
                    SizePixels = 16f,
                    FontDataOwnedByAtlas = 1,
                    EllipsisChar = 0x0085,
                    OversampleH = 1,
                    OversampleV = 1,
                    PixelSnapH = 1,
                    GlyphOffset = new Vector2(0, 0),
                    GlyphMaxAdvanceX = float.MaxValue,
                    RasterizerMultiply = 1.0f
                };
                unsafe
                {
                    atlas.AddFontFromFileTTF(fontPath, cfg.SizePixels, &cfg);
                }
            }

            atlas.GetTexDataAsAlpha8(out IntPtr pixels, out var w, out var h);
            var info = new SKImageInfo(w, h, SKColorType.Alpha8);
            var pmap = new SKPixmap(info, pixels, info.RowBytes);
            var localMatrix = SKMatrix.CreateScale(1.0f / w, 1.0f / h);
            var fontImage = SKImage.FromPixels(pmap);
            // makeShader(SkSamplingOptions(SkFilterMode::kLinear), localMatrix);
            var fontShader = fontImage.ToShader(SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, localMatrix);
            var paint = paintHandle.Target;
            if (paint != null)
            {
                paint.Shader = fontShader;
                paint.Color = SKColors.White;
                atlas.TexID = paintHandle.Ptr;
            }
        }

        // From https://github.com/google/skia/blob/main/tools/viewer/ImGuiLayer.cpp
        // TODO: With net6 we can probably get rid of a lot of allocations in here
        private void Render(CallerInfo caller, ImDrawDataPtr drawData)
        {
            var canvas = caller.Canvas;
            var callerInfo = caller.WithTransformation(SKMatrix.Identity);

            for (int i = 0; i < drawData.CmdListsCount; ++i)
            {
                var drawList = drawData.CmdListsRange[i];

                // De-interleave all vertex data (sigh), convert to Skia types
                //pos.Clear(); uv.Clear(); color.Clear();
                var size = drawList.VtxBuffer.Size;
                var pos = new SKPoint[size];
                var uv = new SKPoint[size];
                var color = new SKColor[size];
                for (int j = 0; j < size; ++j)
                {
                    var vert = drawList.VtxBuffer[j];
                    pos[j] = new SKPoint(vert.pos.X, vert.pos.Y);
                    uv[j] = new SKPoint(vert.uv.X, vert.uv.Y);
                    color[j] = vert.col;
                }

                // ImGui colors are RGBA
                SKSwizzle.SwapRedBlue(MemoryMarshal.AsBytes(color.AsSpan()), MemoryMarshal.AsBytes(color.AsSpan()), color.Length);

                int indexOffset = 0;

                // Draw everything with canvas.drawVertices...
                for (int j = 0; j < drawList.CmdBuffer.Size; ++j)
                {
                    var drawCmd = drawList.CmdBuffer[j];
                    var clipRect = new SKRect(drawCmd.ClipRect.X, drawCmd.ClipRect.Y, drawCmd.ClipRect.Z, drawCmd.ClipRect.W);

                    //using var _ = new SKAutoCanvasRestore(canvas, true);
                    canvas.Save();
                    canvas.ResetMatrix();
                    canvas.ClipRect(clipRect);

                    // TODO: Find min/max index for each draw, so we know how many vertices (sigh)
                    if (drawCmd.UserCallback != IntPtr.Zero)
                    {
                        var handle = GCHandle.FromIntPtr(drawCmd.UserCallback);
                        try
                        {
                            if (handle.Target is DrawCallback callback)
                                callback(drawList, drawCmd);
                        }
                        finally
                        {
                            handle.Free();
                        }
                    }
                    else
                    {
                        var idIndex = drawCmd.TextureId.ToInt64();
                        if (idIndex < _context.WidgetFuncs.Count)
                        {
                            // Small image IDs are actually indices into a list of callbacks. We directly
                            // examing the vertex data to deduce the image rectangle, then reconfigure the
                            // canvas to be clipped and translated so that the callback code gets to use
                            // Skia to render a widget in the middle of an ImGui panel.
                            var rectIndex = drawList.IdxBuffer[indexOffset];
                            var tl = pos[rectIndex];
                            var br = pos[rectIndex + 2];
                            var imageClipRect = new SKRect(tl.X, tl.Y, br.X, br.Y);
                            canvas.ClipRect(imageClipRect);
                            canvas.Translate(imageClipRect.Location);

                            _context.WidgetFuncs[(int)idIndex](callerInfo, imageClipRect);
                        }
                        else
                        {
                            var handle = GCHandle.FromIntPtr(drawCmd.TextureId);
                            var paint = handle.Target as SKPaint ?? _fontPaint.Target;

                            var indices = new ushort[drawCmd.ElemCount];
                            for (int k = 0; k < indices.Length; k++)
                                indices[k] = drawList.IdxBuffer[indexOffset + k];

                            var vertices = SKVertices.CreateCopy(SKVertexMode.Triangles, pos, uv, color, indices);

                            canvas.DrawVertices(vertices, SKBlendMode.Modulate, paint);
                        }

                        indexOffset += (int)drawCmd.ElemCount;
                    }

                    canvas.Restore();
                }
            }
        }

        public bool Notify(INotification notification, CallerInfo caller)
        {
            using (_context.MakeCurrent())
            {
                if (notification is NotificationBase n)
                {
                    _io.KeyAlt = n.AltKey;
                    _io.KeyCtrl = n.CtrlKey;
                    _io.KeyShift = n.ShiftKey;
                }

                if (notification is KeyNotification keyNotification)
                {
                    if (keyNotification is KeyCodeNotification keyCodeNotification)
                    {
                        _io.AddKeyEvent(ToImGuiKey(keyCodeNotification.KeyCode), keyCodeNotification.IsKeyDown);
                    }
                    else if (keyNotification is KeyPressNotification keyPressNotification)
                    {
                        _io.AddInputCharacter(keyPressNotification.KeyChar);
                    }
                }
                else if (notification is MouseNotification mouseNotification)
                {
                    var button = 0;
                    if (mouseNotification is MouseButtonNotification b)
                    {
                        button = b.Buttons switch
                        {
                            System.Windows.Forms.MouseButtons.Left => 0,
                            System.Windows.Forms.MouseButtons.Right => 1,
                            System.Windows.Forms.MouseButtons.Middle => 2,
                            System.Windows.Forms.MouseButtons.XButton1 => 3,
                            System.Windows.Forms.MouseButtons.XButton2 => 4,
                            _ => 0
                        };
                    }

                    switch (mouseNotification.Kind)
                    {
                        case MouseNotificationKind.MouseDown:
                            _io.AddMouseButtonEvent(button, true);
                            break;
                        case MouseNotificationKind.MouseUp:
                            _io.AddMouseButtonEvent(button, false);
                            break;
                        case MouseNotificationKind.MouseMove:
                            _io.AddMousePosEvent(mouseNotification.Position.X, mouseNotification.Position.Y);
                            break;
                        case MouseNotificationKind.MouseWheel:
                            if (mouseNotification is MouseWheelNotification wheel)
                                _io.AddMouseWheelEvent(0, wheel.WheelDelta / 120);
                            break;
                        case MouseNotificationKind.MouseHorizontalWheel:
                            if (mouseNotification is MouseHorizontalWheelNotification hWheel)
                                _io.AddMouseWheelEvent(hWheel.WheelDelta / 120, 0);
                            break;
                        case MouseNotificationKind.DeviceLost:
                            _io.ClearInputCharacters();
                            _io.ClearInputKeys();
                            break;
                        default:
                            break;
                    }
                }
                else if (notification is TouchNotification touchNotification)
                {
                    // TODO
                }
                return false;
            }
        }

        static ImGuiKey ToImGuiKey(System.Windows.Forms.Keys key)
        {
            switch (key)
            {
                case System.Windows.Forms.Keys.Back: return ImGuiKey.Backspace;
                case System.Windows.Forms.Keys.Tab: return ImGuiKey.Tab;
                case System.Windows.Forms.Keys.Enter: return ImGuiKey.Enter;
                case System.Windows.Forms.Keys.ShiftKey: return ImGuiKey.LeftShift;
                case System.Windows.Forms.Keys.ControlKey:  return ImGuiKey.LeftCtrl;
                case System.Windows.Forms.Keys.Menu:  return ImGuiKey.Menu;
                case System.Windows.Forms.Keys.Pause:  return ImGuiKey.Pause;
                case System.Windows.Forms.Keys.CapsLock: return ImGuiKey.CapsLock;
                case System.Windows.Forms.Keys.Escape: return ImGuiKey.Escape;
                case System.Windows.Forms.Keys.Space: return ImGuiKey.Space;
                case System.Windows.Forms.Keys.PageUp: return ImGuiKey.PageUp;
                case System.Windows.Forms.Keys.PageDown: return ImGuiKey.PageDown;
                case System.Windows.Forms.Keys.End: return ImGuiKey.End;
                case System.Windows.Forms.Keys.Home: return ImGuiKey.Home;
                case System.Windows.Forms.Keys.Left: return ImGuiKey.LeftArrow;
                case System.Windows.Forms.Keys.Up:  return ImGuiKey.UpArrow;
                case System.Windows.Forms.Keys.Right: return ImGuiKey.RightArrow;
                case System.Windows.Forms.Keys.Down: return ImGuiKey.DownArrow;
                case System.Windows.Forms.Keys.Snapshot: return ImGuiKey.PrintScreen;
                case System.Windows.Forms.Keys.Insert: return ImGuiKey.Insert;
                case System.Windows.Forms.Keys.Delete: return ImGuiKey.Delete;
                case System.Windows.Forms.Keys.Oem7: return ImGuiKey.Apostrophe;
                case System.Windows.Forms.Keys.Oemcomma: return ImGuiKey.Comma;
                case System.Windows.Forms.Keys.OemMinus: return ImGuiKey.Minus;
                case System.Windows.Forms.Keys.OemPeriod: return ImGuiKey.Period;
                case System.Windows.Forms.Keys.Oem2: return ImGuiKey.Slash;
                case System.Windows.Forms.Keys.Oem1: return ImGuiKey.Semicolon;
                case System.Windows.Forms.Keys.Oemplus: return ImGuiKey.Equal;
                case System.Windows.Forms.Keys.Oem4: return ImGuiKey.LeftBracket;
                case System.Windows.Forms.Keys.Oem5: return ImGuiKey.Backslash;
                case System.Windows.Forms.Keys.Oem6: return ImGuiKey.RightBracket;
                case System.Windows.Forms.Keys.Oem3: return ImGuiKey.GraveAccent;
                case System.Windows.Forms.Keys.D0: return ImGuiKey._0;
                case System.Windows.Forms.Keys.D1: return ImGuiKey._1;
                case System.Windows.Forms.Keys.D2: return ImGuiKey._2;
                case System.Windows.Forms.Keys.D3: return ImGuiKey._3;
                case System.Windows.Forms.Keys.D4: return ImGuiKey._4;
                case System.Windows.Forms.Keys.D5: return ImGuiKey._5;
                case System.Windows.Forms.Keys.D6: return ImGuiKey._6;
                case System.Windows.Forms.Keys.D7: return ImGuiKey._7;
                case System.Windows.Forms.Keys.D8: return ImGuiKey._8;
                case System.Windows.Forms.Keys.D9: return ImGuiKey._9;
                case System.Windows.Forms.Keys.A: return ImGuiKey.A;
                case System.Windows.Forms.Keys.B: return ImGuiKey.B;
                case System.Windows.Forms.Keys.C: return ImGuiKey.C;
                case System.Windows.Forms.Keys.D: return ImGuiKey.D;
                case System.Windows.Forms.Keys.E: return ImGuiKey.E;
                case System.Windows.Forms.Keys.F: return ImGuiKey.F;
                case System.Windows.Forms.Keys.G: return ImGuiKey.G;
                case System.Windows.Forms.Keys.H: return ImGuiKey.H;
                case System.Windows.Forms.Keys.I: return ImGuiKey.I;
                case System.Windows.Forms.Keys.J: return ImGuiKey.J;
                case System.Windows.Forms.Keys.K: return ImGuiKey.K;
                case System.Windows.Forms.Keys.L: return ImGuiKey.L;
                case System.Windows.Forms.Keys.M: return ImGuiKey.M;
                case System.Windows.Forms.Keys.N: return ImGuiKey.N;
                case System.Windows.Forms.Keys.O: return ImGuiKey.O;
                case System.Windows.Forms.Keys.P: return ImGuiKey.P;
                case System.Windows.Forms.Keys.Q: return ImGuiKey.Q;
                case System.Windows.Forms.Keys.R: return ImGuiKey.R;
                case System.Windows.Forms.Keys.S: return ImGuiKey.S;
                case System.Windows.Forms.Keys.T: return ImGuiKey.T;
                case System.Windows.Forms.Keys.U: return ImGuiKey.U;
                case System.Windows.Forms.Keys.V: return ImGuiKey.V;
                case System.Windows.Forms.Keys.W: return ImGuiKey.W;
                case System.Windows.Forms.Keys.X: return ImGuiKey.X;
                case System.Windows.Forms.Keys.Y: return ImGuiKey.Y;
                case System.Windows.Forms.Keys.Z: return ImGuiKey.Z;
                case System.Windows.Forms.Keys.NumPad0: return ImGuiKey.Keypad0;
                case System.Windows.Forms.Keys.NumPad1: return ImGuiKey.Keypad1;
                case System.Windows.Forms.Keys.NumPad2: return ImGuiKey.Keypad2;
                case System.Windows.Forms.Keys.NumPad3: return ImGuiKey.Keypad3;
                case System.Windows.Forms.Keys.NumPad4: return ImGuiKey.Keypad4;
                case System.Windows.Forms.Keys.NumPad5: return ImGuiKey.Keypad5;
                case System.Windows.Forms.Keys.NumPad6: return ImGuiKey.Keypad6;
                case System.Windows.Forms.Keys.NumPad7: return ImGuiKey.Keypad7;
                case System.Windows.Forms.Keys.NumPad8: return ImGuiKey.Keypad8;
                case System.Windows.Forms.Keys.NumPad9: return ImGuiKey.Keypad9;
                case System.Windows.Forms.Keys.Multiply: return ImGuiKey.KeypadMultiply;
                case System.Windows.Forms.Keys.Add: return ImGuiKey.KeypadAdd;
                case System.Windows.Forms.Keys.Subtract: return ImGuiKey.KeypadSubtract;
                case System.Windows.Forms.Keys.Decimal: return ImGuiKey.KeypadDecimal;
                case System.Windows.Forms.Keys.Divide: return ImGuiKey.KeypadDivide;
                case System.Windows.Forms.Keys.F1: return ImGuiKey.F1;
                case System.Windows.Forms.Keys.F2: return ImGuiKey.F2;
                case System.Windows.Forms.Keys.F3: return ImGuiKey.F3;
                case System.Windows.Forms.Keys.F4: return ImGuiKey.F4;
                case System.Windows.Forms.Keys.F5: return ImGuiKey.F5;
                case System.Windows.Forms.Keys.F6: return ImGuiKey.F6;
                case System.Windows.Forms.Keys.F7: return ImGuiKey.F7;
                case System.Windows.Forms.Keys.F8: return ImGuiKey.F8;
                case System.Windows.Forms.Keys.F9: return ImGuiKey.F9;
                case System.Windows.Forms.Keys.F10: return ImGuiKey.F10;
                case System.Windows.Forms.Keys.F11: return ImGuiKey.F11;
                case System.Windows.Forms.Keys.F12: return ImGuiKey.F12;
                case System.Windows.Forms.Keys.NumLock: return ImGuiKey.NumLock;
                case System.Windows.Forms.Keys.Scroll: return ImGuiKey.ScrollLock;
                case System.Windows.Forms.Keys.LShiftKey: return ImGuiKey.LeftShift;
                case System.Windows.Forms.Keys.RShiftKey: return ImGuiKey.RightShift;
                case System.Windows.Forms.Keys.LControlKey: return ImGuiKey.LeftCtrl;
                case System.Windows.Forms.Keys.RControlKey: return ImGuiKey.RightCtrl;
                case System.Windows.Forms.Keys.LMenu: return ImGuiKey.LeftAlt;
                case System.Windows.Forms.Keys.RMenu: return ImGuiKey.RightAlt;
                case System.Windows.Forms.Keys.LWin: return ImGuiKey.LeftSuper;
                case System.Windows.Forms.Keys.RWin: return ImGuiKey.RightSuper;
                default:
                    return ImGuiKey.None;
            }
        }

        public void Dispose()
        {
            _renderContext.Dispose();

            _context.Dispose();
        }
    }
}
