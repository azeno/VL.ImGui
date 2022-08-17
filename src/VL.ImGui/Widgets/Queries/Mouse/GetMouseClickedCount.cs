﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Return the number of successive mouse-clicks at the time where a click happen (otherwise 0).
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetMouseClickedCount : Widget
    {

        public ImGuiNET.ImGuiMouseButton Flag { private get; set; }

        public int Value { get; private set; }

        internal override void Update(Context context)
        {
            Value = ImGuiNET.ImGui.GetMouseClickedCount(Flag);
        }
    }
}
