﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Input (String Hint)", Category = "ImGui.Widgets", Tags = "edit, textfield")]
    internal partial class InputTextWithHint : ChannelWidget<string>
    {

        public string? Label { get; set; }

        public string? Hint { get; set; }

        public int MaxLength { get; set; } = 100;

        public ImGuiNET.ImGuiInputTextFlags Flags { private get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update() ?? string.Empty;
            if (ImGuiNET.ImGui.InputTextWithHint(Label ?? string.Empty, Hint ?? string.Empty, ref value, (uint)MaxLength, Flags))
                Value = value;
        }
    }
}
