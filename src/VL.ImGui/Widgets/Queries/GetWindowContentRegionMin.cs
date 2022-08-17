﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Content boundaries min for the full window (roughly (0,0)-Scroll), in window coordinates
    /// </summary>

    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetWindowContentRegionMin : Widget
    {

        public Vector2 Value { get; private set; }

        internal override void Update(Context context)
        {
            var size = ImGuiNET.ImGui.GetWindowContentRegionMin();
            Value = ImGuiConversion.ToVL(size);
        }
    }
}
