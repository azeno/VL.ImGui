﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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
            var result = new List<MethodInfo>();
            foreach (var type in typeof(Initialization).Assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<GenerateNodeAttribute>();
                if (attr is null)
                    continue;

                if (attr.GenerateRetained)
                    result.Add(type.GetMethod("GetNodeDescription_RetainedMode", BindingFlags.Static | BindingFlags.NonPublic));
                if (attr.GenerateImmediate)
                    result.Add(type.GetMethod("GetNodeDescription_ImmediateMode", BindingFlags.Static | BindingFlags.NonPublic));
            }

            foreach (var m in result)
                yield return (IVLNodeDescription)m.Invoke(null, new[] { factory });
        }
    }

    internal static class NodeBuildingUtils
    {
        public static IVLPinDescription Input<T>(this NodeBuilding.NodeDescriptionBuildContext c, string name, T defaultValue, string? summary = null, string? remarks = null)
        {
            return c.Pin(name, typeof(T), defaultValue, summary, remarks);
        }

        // Special overload for pins of type Optional<T> - we explicitly need to set the VL default value to null
        public static IVLPinDescription Input<T>(this NodeBuilding.NodeDescriptionBuildContext c, string name, Optional<T> defaultValue, string? summary = null, string? remarks = null)
        {
            return c.Pin(name, typeof(Optional<T>), null, summary, remarks);
        }

        public static IVLPinDescription Output<T>(this NodeBuilding.NodeDescriptionBuildContext c, string name, T? witness = default, string? summary = null, string? remarks = null)
        {
            return c.Pin(name, typeof(T), summary, remarks);
        }
    }
}