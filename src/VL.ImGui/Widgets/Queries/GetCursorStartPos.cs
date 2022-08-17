﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Cursor position in absolute coordinates (useful to work with ImDrawList API). generally top-left == GetMainViewport()->Pos == (0,0) in single viewport mode, and bottom-right == GetMainViewport()->Pos+Size == io.DisplaySize in single-viewport mode.
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetCursorScreenPos : Widget
    {
        public Vector2 Value { get; private set; }

        internal override void Update(Context context)
        {
            var pos = ImGuiNET.ImGui.GetCursorScreenPos();
            Value = ImGuiConversion.ToVL(pos);
        }
    }
}
