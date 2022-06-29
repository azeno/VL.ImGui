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
    [GenerateNode(Fragmented = false, Category = "ImGui.Styling", Tags = "WindowBg WindowMinSize WindowTitleAlign WindowPadding WindowRounding WindowBorderSize")]
    internal partial class SetWindowStyle : Widget
    {
        public Widget? Input { private get; set; }

        public Optional<Color4> Background { private get; set; }

        /// <summary>
        /// Minimum window size.
        /// </summary>
        public Optional<Vector2> MinSize { private get; set; }

        public Optional<Vector2> TitleAlign { private get; set; }

        /// <summary>
        /// Padding within a window.
        /// </summary>
        public Optional<Vector2> Padding { private get; set; }

        /// <summary>
        /// Radius of window corners rounding. Set to 0.0 to have rectangular windows. Large values tend to lead to variety of artifacts and are not recommended.
        /// </summary>
        public Optional<float> Rounding { private get; set; }

        /// <summary>
        /// Thickness of border around windows. Generally set to 0.0 or 1.0. (Other values are not well tested and more CPU/GPU costly).
        /// </summary>
        public Optional<float> BorderSize { private get; set; }

        internal override void Update(Context context)
        {
            if (Input is null)
                return;

            var colorCount = 0;
            var valueCount = 0;
            try
            {
                if (Background.HasValue)
                {
                    colorCount++;
                    ImGui.PushStyleColor(ImGuiCol.WindowBg, Background.Value.ToImGui());
                }
                if (MinSize.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, MinSize.Value.ToImGui());
                }
                if (TitleAlign.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowTitleAlign, TitleAlign.Value.ToImGui());
                }
                if (Padding.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Padding.Value.ToImGui());
                }
                if (Rounding.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, Rounding.Value);
                }
                if (BorderSize.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, BorderSize.Value);
                }

                context.Update(Input);
            }
            finally
            {
                if (colorCount > 0)
                    ImGui.PopStyleColor(colorCount);
                if (valueCount > 0)
                    ImGui.PopStyleVar(valueCount);
            }
        }
    }
}
