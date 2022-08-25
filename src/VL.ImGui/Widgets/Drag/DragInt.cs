﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Drag (Int)", Category = "ImGui.Widgets")]
    internal partial class DragInt : ChannelWidget<int>
    {
        public string? Label { get; set; }

        public int Speed { private get; set; } = 1;

        public int Min { private get; set; } = 0;

        public int Max { private get; set; } = 100;

        public string? Format { private get; set; }

        public ImGuiNET.ImGuiSliderFlags Flags { private get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update();
            if (ImGuiNET.ImGui.DragInt(Label ?? string.Empty, ref value, Speed, Min, Max, string.IsNullOrWhiteSpace(Format) ? null : Format, Flags))
                Value = value;
        }
    }
}
