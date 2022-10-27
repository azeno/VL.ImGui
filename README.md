# VL.ImGui

A first sketch of a node set for [vvvv](visualprogramming.net) around [ImGui](https://github.com/ocornut/imgui). The mesh produced by ImGui gets rendered with VL.Skia and the input notifications of VL.Skia get translated to ImGui.

## Installation

This library is shipped with vvvv itself. Building the repository outside of the vvvv main repo is currently not supported.

## Design thoughts

In general the nodes are written in C# and exposed via a node factory. The relevant node description get generated by a source generator (see topic below).

### Retained mode

Maybe a little bit of a contradiction, but the current node set does not offer an "immediate" mode. Instead the original API gets wrapped in widget classes which can be connected to each other by the user and will later get traversed by the `ImGui (Skia)` node.
The benefits are that the end-user doesn't need to take care of the state machinge (`Begin`/`End` calls), but on the other hand it takes a way a lot of decisions thereby reducing the overall degree of freedom.
The widgets are represented as an object graph which gets traversed by `ImGui (Skia)`.

### Immediate mode

We can also think of a second node set exposing the real immediate mode as regions (taking care of the `Begin`/`End` calls). Those regions could in a first iteration be patched with the custom region API and if need be get replaced by ones generating C# code for best performance.

In any case this approach would follow the original design principles much more closely and therefor would offer a much higher degree of freedom.

#### To be discussed

- How to deal with the `ref` parameters - VL has no concept for this yet.

## Generator

Most of the nodes get generated with a [C# source generator](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview).
It can be configured with the `GenerateNode` attribute.
Help texts can be added to nodes and pins via `summary` XML comments.

## Updating ImGui
We're using a custom build of `cimgui` where the ImGui context is thread static (https://github.com/vvvv/VL.ImGui/issues/2). In order to update the following steps have to be done:
- Update the ImGui.NET pacakge
- Update the submodule ImGui.NET-nativebuild to the same version as the package by merging the remote changes. There will be conflicts in the generator output files, choose remote
- Regenerate the bindings with generator.bat - to run the generator you need to extract luajit (from https://luapower.com/luajit) and mingw64 (from https://github.com/niXman/mingw-builds-binaries/releases) and add both their bin folders to the path variable inside the generator.bat file.
- Compile the lib with `build-native.cmd Release`
- Copy the generated `cimgui.dll` to `\vl\VL.ImGui\runtimes\win-x64\native`