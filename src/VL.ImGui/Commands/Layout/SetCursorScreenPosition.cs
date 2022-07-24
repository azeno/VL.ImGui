﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Cursor position in window coordinates (relative to window position)
    /// </summary>
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class SetCursorScreenPosition : Widget
    {
        public Vector2 Position { private get; set; }

        internal override void Update(Context context)
        {
            ImGuiNET.ImGui.SetCursorScreenPos(Position.ToImGui());
        }
    }
}
