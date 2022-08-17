﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Horizontal distance preceding label when using TreeNode or Bullet == (g.FontSize + style.FramePadding.x*2) for a regular unframed TreeNode
    /// </summary>
    [GenerateNode(Category = "ImGui.Queries")]
    internal partial class GetTreeNodeToLabelSpacing : Widget
    {

        public float Value { get; private set; }

        internal override void Update(Context context)
        {
            Value = ImGuiNET.ImGui.GetTreeNodeToLabelSpacing();
        }
    }
}
