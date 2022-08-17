﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Cursor position in window coordinates (relative to window position)
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetCursorPos : Widget
    {
        public Vector2 Value { get; private set; }

        internal override void Update(Context context)
        {
            var pos = ImGuiNET.ImGui.GetCursorPos();
            Value = ImGuiConversion.ToVL(pos);
        }
    }
}