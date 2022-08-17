﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Call between widgets or groups to layout them horizontally. X position given in window coordinates.
    /// </summary>
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class SameLine : Widget
    {

        public float Offset { private get; set; }

        public float Spacing { private get; set; }

        internal override void Update(Context context)
        {
            ImGuiNET.ImGui.SameLine(Offset, Spacing);
        }
    }
}