﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VL.Core;

namespace VL.ImGui.Widgets
{
    [GenerateNode]
    internal sealed partial class Column : Widget
    {
        public IEnumerable<Widget> Children { get; set; } = Enumerable.Empty<Widget>();

        internal override void Update(Context context)
        {
            var count = Children.Count(x => x != null);
            if (count > 0)
            {
                ImGuiNET.ImGui.BeginGroup();
                try
                {
                    foreach (var child in Children)
                    {
                        if (child is null)
                            continue;
                        else
                            context.Update(child);
                    }
                }
                finally
                {
                    ImGuiNET.ImGui.EndGroup();
                }
            }
        }
    }
}
