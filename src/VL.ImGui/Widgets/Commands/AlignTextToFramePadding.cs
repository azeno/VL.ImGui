﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Category = "ImGui.Commands")]
    internal partial class AlignTextToFramePadding : Widget
    {
        internal override void Update(Context context)
        {
            ImGuiNET.ImGui.AlignTextToFramePadding();
        }
    }
}
