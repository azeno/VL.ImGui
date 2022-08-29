﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Is the last item visible? (items may be out of sight because of clipping/scrolling)
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries", IsStylable = false)]
    internal partial class IsItemVisible : Widget
    {

        public bool Value { get; private set; }

        internal override void UpdateCore(Context context)
        {
            Value = ImGuiNET.ImGui.IsItemVisible();
        }
    }
}
