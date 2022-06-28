﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using Stride.Core.Mathematics;
using System.Runtime.CompilerServices;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Slider (Int2)")]
    internal partial class SliderInt2 : Widget
    {
        public string? Label { get; set; }

        public int Min { private get; set; } = 0;

        public int Max { private get; set; } = 100;

        public string? Format { private get; set; }

        public ImGuiNET.ImGuiSliderFlags Flags { private get; set; }

        public BehaviorSubject<Int2> Value { get; } = new BehaviorSubject<Int2>(Int2.Zero);

        internal override void Update(Context context)
        {
            var value = Value.Value;

            ref var x = ref value.X;
            if (ImGuiNET.ImGui.SliderInt2(Label ?? string.Empty, ref x, Min, Max, string.IsNullOrWhiteSpace(Format) ? null : Format, Flags))
                Value.OnNext(Unsafe.As<int, Int2>(ref x));
        }
    }
}
