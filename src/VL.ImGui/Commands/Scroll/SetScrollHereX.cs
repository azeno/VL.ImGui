﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Adjust scrolling amount to make current cursor position visible.
    /// </summary>
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class SetScrollHereX : Widget
    {

        /// <summary>
        /// 0.0 - Left, 0.5 - Center, 1.0 - Right.
        /// </summary>
        public float Ratio { private get; set; }

        internal override void Update(Context context)
        {
            ImGuiNET.ImGui.SetScrollHereX(Ratio);
        }
    }
}
