﻿using ImGuiNET;
using Stride.Core.Mathematics;
using VL.Lib.Collections;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Access the Style structure (colors, sizes).
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetStyle : Widget
    {
        public StyleSnapshot? Value { get; private set; }

        internal override unsafe void UpdateCore(Context context)
        {
            var style = ImGuiNET.ImGui.GetStyle();
            Value = new StyleSnapshot(style);
        }
    }

    /// <summary>
    /// Immutable copy of all ImGui styles
    /// </summary>
    public record StyleSnapshot
    {
        internal StyleSnapshot(ImGuiStylePtr ptr)
        {
            Alpha = ptr.Alpha;
            DisabledAlpha = ptr.DisabledAlpha;
            WindowPadding = ptr.WindowPadding.ToVL();
            WindowRounding = ptr.WindowRounding;
            WindowBorderSize = ptr.WindowBorderSize;
            WindowMinSize = ptr.WindowMinSize.ToVL();
            WindowTitleAlign = ptr.WindowTitleAlign.ToVL();
            WindowMenuButtonPosition = ptr.WindowMenuButtonPosition;
            ChildRounding = ptr.ChildRounding;
            ChildBorderSize = ptr.ChildBorderSize;
            PopupRounding = ptr.PopupRounding;
            PopupBorderSize = ptr.PopupBorderSize;
            FramePadding = ptr.FramePadding.ToVL();
            FrameRounding = ptr.FrameRounding;
            FrameBorderSize = ptr.FrameBorderSize;
            ItemSpacing = ptr.ItemSpacing.ToVL();
            ItemInnerSpacing = ptr.ItemInnerSpacing.ToVL();
            CellPadding = ptr.CellPadding.ToVL();
            TouchExtraPadding = ptr.TouchExtraPadding.ToVL();
            IndentSpacing = ptr.IndentSpacing;
            ColumnsMinSpacing = ptr.ColumnsMinSpacing;
            ScrollbarSize = ptr.ScrollbarSize;
            ScrollbarRounding = ptr.ScrollbarRounding;
            GrabMinSize = ptr.GrabMinSize;
            GrabRounding = ptr.GrabRounding;
            LogSliderDeadzone = ptr.LogSliderDeadzone;
            TabRounding = ptr.TabRounding;
            TabBorderSize = ptr.TabBorderSize;
            TabMinWidthForCloseButton = ptr.TabMinWidthForCloseButton;
            ColorButtonPosition = ptr.ColorButtonPosition;
            ButtonTextAlign = ptr.ButtonTextAlign.ToVL();
            SelectableTextAlign = ptr.SelectableTextAlign.ToVL();
            DisplayWindowPadding = ptr.DisplayWindowPadding.ToVL();
            DisplaySafeAreaPadding = ptr.DisplaySafeAreaPadding.ToVL();
            MouseCursorScale = ptr.MouseCursorScale;
            AntiAliasedLines = ptr.AntiAliasedLines;
            AntiAliasedLinesUseTex = ptr.AntiAliasedLinesUseTex;
            AntiAliasedFill = ptr.AntiAliasedFill;
            CurveTessellationTol = ptr.CurveTessellationTol;
            CircleTessellationMaxError = ptr.CircleTessellationMaxError;
            Colors = ptr.Colors.Select(x => x.ToVLColor4()).ToSpread();
        }

        public float Alpha { get; }

        public float DisabledAlpha { get; }

        public Vector2 WindowPadding { get; }

        public float WindowRounding { get; }

        public float WindowBorderSize { get; }

        public Vector2 WindowMinSize { get; }

        public Vector2 WindowTitleAlign { get; }

        public ImGuiDir WindowMenuButtonPosition { get; }

        public float ChildRounding { get; }

        public float ChildBorderSize { get; }

        public float PopupRounding { get; }

        public float PopupBorderSize { get; }

        public Vector2 FramePadding { get; }

        public float FrameRounding { get; }

        public float FrameBorderSize { get; }

        public Vector2 ItemSpacing { get; }

        public Vector2 ItemInnerSpacing { get; }

        public Vector2 CellPadding { get; }

        public Vector2 TouchExtraPadding { get; }

        public float IndentSpacing { get; }

        public float ColumnsMinSpacing { get; }

        public float ScrollbarSize { get; }

        public float ScrollbarRounding { get; }

        public float GrabMinSize { get; }

        public float GrabRounding { get; }

        public float LogSliderDeadzone { get; }

        public float TabRounding { get; }

        public float TabBorderSize { get; }

        public float TabMinWidthForCloseButton { get; }

        public ImGuiDir ColorButtonPosition { get; }

        public Vector2 ButtonTextAlign { get; }

        public Vector2 SelectableTextAlign { get; }

        public Vector2 DisplayWindowPadding { get; }

        public Vector2 DisplaySafeAreaPadding { get; }

        public float MouseCursorScale { get; }

        public bool AntiAliasedLines { get; }

        public bool AntiAliasedLinesUseTex { get; }

        public bool AntiAliasedFill { get; }

        public float CurveTessellationTol { get; }

        public float CircleTessellationMaxError { get; }

        public Spread<Color4> Colors { get; }
    }
}