﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using VL.Core.CompilerServices;
using VL.ImGui.Widgets;
using VL.ImGui.Windows;

[assembly: AssemblyInitializer(typeof(VL.ImGui.Initialization))]

namespace VL.ImGui
{
    public sealed class Initialization : AssemblyInitializer<Initialization>
    {
        protected override void RegisterServices(IVLFactory factory)
        {
            factory.RegisterNodeFactory(NodeBuilding.NewNodeFactory(factory, "VL.ImGUI.Nodes", f =>
            {
                var nodes = GetNodes(f).ToImmutableArray();
                return NodeBuilding.NewFactoryImpl(nodes);
            }));
        }

        static IEnumerable<IVLNodeDescription> GetNodes(IVLNodeDescriptionFactory factory)
        {
            yield return DemoWindow.GetNodeDescription(factory);

            yield return ObjectEditor.GetNodeDescription(factory);

            yield return SkiaWidget.GetNodeDescription(factory);

            //Slider
            yield return SliderFloat.GetNodeDescription(factory);

            //Dropdown
            yield return Combo.GetNodeDescription(factory);

            // Windows
            yield return Window.GetNodeDescription(factory);
            yield return ChildWindow.GetNodeDescription(factory);

            // Buttons
            yield return Button.GetNodeDescription(factory);
            yield return InvisibleButton.GetNodeDescription(factory);
            yield return Selectable.GetNodeDescription(factory);
            yield return ArrowButton.GetNodeDescription(factory);

            // Checkbox
            yield return Checkbox.GetNodeDescription(factory);

            // Separator
            yield return Separator.GetNodeDescription(factory);

            // Dummy
            yield return Dummy.GetNodeDescription(factory);

            // Input
            yield return InputFloat.GetNodeDescription(factory);
            yield return InputInt.GetNodeDescription(factory);

            // Style
            yield return SetStyleColor.GetNodeDescription(factory);
            yield return SetStyleVarFloat.GetNodeDescription(factory);
            yield return SetStyleVarVector.GetNodeDescription(factory);

            // Layout
            yield return Row.GetNodeDescription(factory);
            yield return Column.GetNodeDescription(factory);
            yield return SameLine.GetNodeDescription(factory);
            yield return SetIndent.GetNodeDescription(factory);
            yield return SetWidth.GetNodeDescription(factory);
            yield return SetNextWindowContentSize.GetNodeDescription(factory);
            yield return SetID.GetNodeDescription(factory);
            yield return SetAlignTextToFramePadding.GetNodeDescription(factory);
            yield return CalcTextSize.GetNodeDescription(factory);
            yield return CalcItemWidth.GetNodeDescription(factory);
            yield return GetItemRectSize.GetNodeDescription(factory);
            yield return GetItemRectMin.GetNodeDescription(factory);
            yield return GetItemRectMax.GetNodeDescription(factory);
            yield return GetWindowSize.GetNodeDescription(factory);
            yield return GetCursorPos.GetNodeDescription(factory);
            yield return GetContentRegionAvail.GetNodeDescription(factory);
            yield return GetContentRegionMax.GetNodeDescription(factory);
            yield return Bullet.GetNodeDescription(factory);

            // Text
            yield return TextWidget.GetNodeDescription(factory);
            yield return SetTextWrapPosition.GetNodeDescription(factory);

            // Table
            yield return Table.GetNodeDescription(factory);
            yield return TableNextColumn.GetNodeDescription(factory);
            yield return TableSetupColumn.GetNodeDescription(factory);

            // Behaviour
            yield return IsItemClicked.GetNodeDescription(factory);
            yield return IsItemHovered.GetNodeDescription(factory);

        }
    }

    internal static class NodeBuildingUtils
    {
        public static IVLPinDescription Input<T>(this NodeBuilding.NodeDescriptionBuildContext c, string name, T defaultValue, string? summary = null, string? remarks = null)
        {
            return c.Pin(name, typeof(T), defaultValue, summary, remarks);
        }

        public static IVLPinDescription Output<T>(this NodeBuilding.NodeDescriptionBuildContext c, string name, T? witness = default, string? summary = null, string? remarks = null)
        {
            return c.Pin(name, typeof(T), summary, remarks);
        }
    }
}
