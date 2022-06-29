﻿using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using ImGuiNET;

namespace VL.ImGui.Styling
{
    using ImGui = ImGuiNET.ImGui;

    // We decided that the style nodes shall take all the relevant values in one go (= disable fragments).
    [GenerateNode(Fragmented = false, Category = "ImGui.Styling", Tags = "ItemSpacing ItemInnerSpacing")]
    internal partial class SetSpacingStyle : Widget
    {
        public Widget? Input { private get; set; }

        /// <summary>
        /// Horizontal and vertical spacing between widgets/lines.
        /// </summary>
        public Optional<Vector2> ItemSpacing { private get; set; }

        /// <summary>
        /// Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).
        /// </summary>
        public Optional<Vector2> InnerSpacing { private get; set; }

        internal override void Update(Context context)
        {
            if (Input is null)
                return;

            var valueCount = 0;
            try
            {
                if (ItemSpacing.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, ItemSpacing.Value.ToImGui());
                }
                if (InnerSpacing.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.ItemInnerSpacing, InnerSpacing.Value.ToImGui());
                }

                context.Update(Input);
            }
            finally
            {
                if (valueCount > 0)
                    ImGui.PopStyleVar(valueCount);
            }
        }
    }
}
