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
    [GenerateNode(Fragmented = false)]
    internal partial class SetScrollStyle : Widget
    {
        public Widget? Input { private get; set; }

        public Optional<Color4> Backgorund { private get; set; }

        public Optional<Color4> GrabColor { private get; set; }

        public Optional<Color4> GrabHovered { private get; set; }

        public Optional<Color4> GrabActive { private get; set; }

        public Optional<float> GrabWidth { private get; set; }

        public Optional<float> GrabRounding { private get; set; }

        internal override void Update(Context context)
        {
            if (Input is null)
                return;

            var colorCount = 0;
            var valueCount = 0;
            try
            {
                if (Backgorund.HasValue)
                {
                    colorCount++;
                    ImGui.PushStyleColor(ImGuiCol.ScrollbarBg, Backgorund.Value.ToImGui());
                }
                if (GrabColor.HasValue)
                {
                    colorCount++;
                    ImGui.PushStyleColor(ImGuiCol.ScrollbarGrab, GrabColor.Value.ToImGui());
                }
                if (GrabHovered.HasValue)
                {
                    colorCount++;
                    ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabHovered, GrabHovered.Value.ToImGui());
                }
                if (GrabActive.HasValue)
                {
                    colorCount++;
                    ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabActive, GrabActive.Value.ToImGui());
                }
                if (GrabWidth.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarSize, GrabWidth.Value);
                }
                if (GrabRounding.HasValue)
                {
                    valueCount++;
                    ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, GrabRounding.Value);
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