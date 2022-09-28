﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Retrieve mouse position at the time of opening popup.
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries", GenerateRetained = false)]
    internal partial class GetMousePosOnOpeningCurrentPopup : Widget
    {
        public Vector2 Value { get; private set; }

        internal override void UpdateCore(Context context)
        {
            var value = ImGuiNET.ImGui.GetMousePosOnOpeningCurrentPopup();
            Value = value.ToVLHecto();
        }
    }
}
