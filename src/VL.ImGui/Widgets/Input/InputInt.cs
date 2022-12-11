﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Input (Int)", Category = "ImGui.Widgets", Tags = "number, updown")]
    internal partial class InputInt : ChannelWidget<int>
    {

        public string? Label { get; set; }

        public int Step { private get; set; } = 1;

        public int StepFast { private get; set; } = 100;

        public ImGuiNET.ImGuiInputTextFlags Flags { private get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update();
            if (ImGuiNET.ImGui.InputInt(Context.GetLabel(this, Label), ref value, Step, StepFast, Flags))
                Value = value;
        }
    }
}
