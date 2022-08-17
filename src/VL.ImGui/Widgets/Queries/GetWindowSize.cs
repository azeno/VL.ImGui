﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Get current window size
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetWindowSize : Widget
    {

        public Vector2 Value { get; private set; }


        internal override void Update(Context context)
        {
            var size = ImGuiNET.ImGui.GetWindowSize();
            Value = ImGuiConversion.ToVL(size);
        }
    }
}
