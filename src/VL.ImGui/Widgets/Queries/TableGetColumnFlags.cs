﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Return column flags so you can query their Enabled/Visible/Sorted/Hovered status flags.
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class TableGetColumnFlags : Widget
    {
        /// <summary>
        /// Pass -1 to use current column.
        /// </summary>
        public int Index { private set; get; } = -1;

        public ImGuiNET.ImGuiTableColumnFlags Value { get; private set; }

        internal override void Update(Context context)
        {
            Value = ImGuiNET.ImGui.TableGetColumnFlags(Index);
        }
    }
}
