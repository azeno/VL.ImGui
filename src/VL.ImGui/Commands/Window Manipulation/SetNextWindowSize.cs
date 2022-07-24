﻿using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false)]
    internal partial class SetNextWindowSize : Widget
    {
        public Vector2 Size { private get; set; }

        public bool Enabled { private get; set; } = true;

        internal override void Update(Context context)
        {
            if (Enabled)
                ImGuiNET.ImGui.SetNextWindowSize(Size.ToImGui());
        }
    }
}
