﻿using SkiaSharp;
using Stride.Core.Mathematics;
using System;
using VL.Core;
using VL.Skia;

namespace VL.ImGui.Widgets
{
    [GenerateNode(GenerateRetained = false)]
    public sealed partial class RetainedMode : Widget
    {
        public Widget Widget { get; set; }

        internal override void Update(Context context)
        {
            Widget.Update(context);
        }
    }
}