﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using VL.Core;
using Stride.Core.Mathematics;

namespace VL.ImGui.Widgets
{
    [GenerateNode]
    internal partial class ColorEdit : Widget
    {
        public string? Label { get; set; }

        public ImGuiNET.ImGuiColorEditFlags Flags { private get; set; }

        public BehaviorSubject<Color4> Value { get; } = new BehaviorSubject<Color4>(Stride.Core.Mathematics.Color.White);

        internal override void Update(Context context)
        {
            var value = Value.Value.ToImGui();
            if (ImGuiNET.ImGui.ColorEdit4(Label ?? string.Empty, ref value, Flags))
                Value.OnNext(value.ToVLColor4());
        }
    }
}
