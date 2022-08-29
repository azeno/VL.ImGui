﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Retrieve style color as stored in ImGuiStyle structure.
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetStyleColor : Widget
    {
        public ImGuiNET.ImGuiCol Flag { get; private set; }

        public Color4 Value { get; private set; }

        internal override unsafe void UpdateCore(Context context)
        {
            var color = ImGuiNET.ImGui.GetStyleColorVec4(Flag);
            Value = ImGuiConversion.ToVLColor4(*color);
        }
    }
}
