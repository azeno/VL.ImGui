﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Get current font size (= height in pixels) of current font with current scale applied
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetFontSize : Widget
    {

        public float Value { get; private set; }

        internal override void UpdateCore(Context context)
        {
            Value = ImGuiNET.ImGui.GetFontSize();
        }
    }
}
