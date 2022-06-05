﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using VL.Core;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Name = "Combo (String)")]
    internal partial class Combo : Widget
    {
        private string? _value;

        public string Value
        {
            get => ObservableValue.Value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    ObservableValue.OnNext(value);
                }
            }
        }

        public string? Label { get; set; }

        [Documentation(@"Dropdown")]
        public string? Format { private get; set; }

        public IEnumerable<string> Items { get; set; } = Enumerable.Empty<string>();

        public ImGuiNET.ImGuiComboFlags Flags { private get; set; }

        public BehaviorSubject<string> ObservableValue { get; } = new BehaviorSubject<string>("");

        internal override void Update(Context context)
        {
            var value = ObservableValue.Value;

            var count = Items.Count();
            if (count > 0)
            {
                if (ImGuiNET.ImGui.BeginCombo(Label ?? string.Empty, value, Flags))
                {

                    try
                    {
                        foreach (var item in Items)
                        {
                            bool is_selected = value == item;
                            if (ImGuiNET.ImGui.Selectable(item, is_selected))
                            {
                                ObservableValue.OnNext(item);
                            }
                            if (is_selected)
                            {
                                ImGuiNET.ImGui.SetItemDefaultFocus();
                            }
                        }
                    }
                    finally
                    {
                        ImGuiNET.ImGui.EndCombo();
                    }
                }
            }
        }
    }
}