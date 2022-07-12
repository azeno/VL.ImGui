﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Slider (Int Vertical)", Category = "ImGui.Widgets")]
    internal partial class SliderIntVertical : ChannelWidget<int>
    {
        public string? Label { get; set; }

        public int Min { private get; set; } = 0;

        public int Max { private get; set; } = 100;

        public Vector2 Size { get; set; } = new Vector2 (20, 100);

        public string? Format { private get; set; }

        public ImGuiNET.ImGuiSliderFlags Flags { private get; set; }

        internal override void Update(Context context)
        {
            var value = Update();
            if (ImGuiNET.ImGui.VSliderInt(Label ?? string.Empty, Size.ToImGui(), ref value, Min, Max, Format, Flags))
                Value = value;
        }
    }
}
