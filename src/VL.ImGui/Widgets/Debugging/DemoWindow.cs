﻿using VL.Core;

namespace VL.ImGui.Windows
{
    [GenerateNode(Category = "ImGui.Debug")]
    public sealed partial class MetricsWindow : Widget
    {
        public bool HasCloseButton { get; set; } = true;

        public bool Closing { get; private set; }

        internal override void Reset()
        {
            Closing = false;
        }

        internal override void UpdateCore(Context context)
        {
            if (HasCloseButton)
            {
                var open = true;
                ImGuiNET.ImGui.ShowMetricsWindow(ref open);
                Closing = !open;
            }
            else
            {
                ImGuiNET.ImGui.ShowMetricsWindow();
            }
        }
    }
}
