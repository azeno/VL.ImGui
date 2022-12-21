﻿using VL.Core.EditorAttributes;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Drag (Float64)", Category = "ImGui.Widgets.Advanced", Tags = "number")]
    [WidgetType(WidgetType.Drag)]
    internal partial class DragFloat64 : ChannelWidget<double>
    {
        public string? Label { get; set; }

        public float Speed { private get; set; } = 0.01f;

        public double Min { private get; set; } = 0d;

        public double Max { private get; set; } = 1d;

        /// <summary>
        /// Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: % .0f" -> Biscuit: 1; etc.
        /// </summary>
        public string? Format { private get; set; }

        public ImGuiNET.ImGuiSliderFlags Flags { private get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update();
            if (ImGuiUtils.DragDouble(Context.GetLabel(this, Label), ref value, Speed, Min, Max, string.IsNullOrWhiteSpace(Format) ? null : Format, Flags))
                Value = value;
        }
    }
}
