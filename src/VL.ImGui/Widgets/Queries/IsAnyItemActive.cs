﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Is any item active?
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class IsAnyItemActive : Widget
    {

        public bool Value { get; private set; }

        internal override void UpdateCore(Context context)
        {
            Value = ImGuiNET.ImGui.IsAnyItemActive();
        }
    }
}
