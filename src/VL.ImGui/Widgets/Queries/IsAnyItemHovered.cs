﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Is any item hovered?
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class IsAnyItemHovered : Widget
    {

        public bool Value { get; private set; }

        internal override void Update(Context context)
        {
            Value = ImGuiNET.ImGui.IsAnyItemHovered();
        }
    }
}