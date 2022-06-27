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
    internal sealed partial class TabItemButton : Widget
    {
        public string? Label { get; set; }

        public ImGuiNET.ImGuiTabItemFlags Flags { private get; set; }

        public BehaviorSubject<bool> Value { get; } = new BehaviorSubject<bool>(true);

        internal override void Update(Context context)
        {
            var value = Value.Value;
            if (ImGuiNET.ImGui.TabItemButton(Label ?? string.Empty, Flags))
                Value.OnNext(true);
        }
    }
}