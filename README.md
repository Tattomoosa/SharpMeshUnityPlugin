# Sharp Mesh Unity Plugin

Unity plugin for the [SharpMesh](https://github.com/HiceS/SharpMesh) mesh decomposition tool. At the moment, both that tool and this plugin are in extremely early development and are not at all useful to anybody. Check back later!

## Installation

Requires `dotnet` and Unity.

This repo does not contain the necessary SharpMesh DLL file. Probably you want to clone that repo first and use this as a submodule.

```
$ git clone --recurse-submodules https://github.com/HiceS/SharpMesh
```

If you have already cloned that repo, then from a path within it:

```
$ git submodule update --init --recursive
```

Then build SharpMesh. Take the output SharpMesh.dll file and drop it in the Unity project at `Assets/SharpMesh/DLL`.

Then open this folder as a Unity project.

## Usage

In the Project pane, click the "plus" icon. At the top will be an option to create a SharpMesh. This is a ScriptableObject asset that takes an input mesh and will (eventually) utilize SharpMesh to perform a mesh decomposition on that mesh into a list of meshes that can be used for creating colliders or destructible objects.

You can also make a Decomposer object, although there is no way to currently register a single Decomposerwith a SharpMeshObject to share settings across multiple meshes. This is planned in the future.
