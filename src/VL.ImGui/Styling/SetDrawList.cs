﻿using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using ImGuiNET;
using VL.ImGui.Widgets.Primitives;

namespace VL.ImGui.Styling
{
    using ImGui = ImGuiNET.ImGui;

    // We decided that the style nodes shall take all the relevant values in one go (= disable fragments).
    [GenerateNode(Fragmented = false, Category = "ImGui.Styling", GenerateImmediate = false)]
    internal partial class SetDrawList : StyleBase
    {
        public DrawList DrawList { protected get; set; }
        ImDrawListPtr previousDrawList;
        System.Numerics.Vector2 previousOffset;

        internal override void SetCore()
        {
            previousDrawList = Context.Current.DrawListPtr;
            previousOffset = Context.Current.DrawListOffset;

            Context.Current.SetDrawList(DrawList);
        }

        public override void Reset()
        {
            Context.Current.DrawListPtr = previousDrawList;
            Context.Current.DrawListOffset = previousOffset;
            base.Reset();
        }
    }
}
