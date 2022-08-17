﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Move content position toward the right, by Value, or style.IndentSpacing if Value <= 0.
    /// </summary>
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class Indent : Widget
    {

        public float Value { private get; set; }

        internal override void Update(Context context)
        {
            ImGuiNET.ImGui.Indent(Value);
        }
    }
}