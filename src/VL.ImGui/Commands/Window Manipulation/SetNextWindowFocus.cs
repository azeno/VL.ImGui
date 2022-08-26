﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class SetNextWindowFocus : Widget
    {
        public bool Enabled { private get; set; } = true;

        internal override void UpdateCore(Context context)
        {   
            if (Enabled)
                ImGuiNET.ImGui.SetNextWindowFocus();
        }
    }
}
