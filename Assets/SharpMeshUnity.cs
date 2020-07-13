using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SharpMesh", menuName = "SharpMesh", order = 1)]
public class SharpMeshUnity : ScriptableObject 
{
    public Mesh inputMesh;
    public List<SerializedMesh> outputMeshList;

    /// <summary>
    /// Processes an input mesh into a list of serialized mesh data.
    /// </summary>
    public void Process()
    {
        if (inputMesh == null)
        {
            Debug.LogError("SharpMesh: Must specify a Mesh to process.");
            return;
        }
        SharpMesh.Mesh sInputMesh = MeshToSharpMesh(inputMesh);
        // TODO this is temporary
        outputMeshList = new List<SerializedMesh>();
        outputMeshList.Add(TestCreateCube());
        outputMeshList.Add(TestCreateCube());
    }

    /// <summary>
    /// Clears SharpMesh data.
    /// </summary>
    public void Clear()
    {
        outputMeshList.Clear();
    }

    private SharpMesh.Mesh MeshToSharpMesh(Mesh mesh)
    {
        SharpMesh.Mesh sMesh = new SharpMesh.Mesh();
        // TODO actually do vertex conversion
        // sMesh.vertices = mesh.vertices;
        // sMesh.triangles = mesh.triangles;
        return sMesh;
    }

    private Mesh SharpMeshToMesh(SharpMesh.Mesh sMesh)
    {
        Mesh mesh = new Mesh();
        // TODO actually do vertex conversion
        // mesh.vertices = sMesh.vertices;
        // mesh.triangles = sMesh.triangles;
        return mesh;
    }

    // TODO Temporary for testing. Referenced from http://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html
    private SerializedMesh TestCreateCube()
    {
        Vector3[] vertices = {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
        };
        int[] triangles = {
            0, 2, 1, //face front
            0, 3, 2,
            2, 3, 4, //face top
            2, 4, 5,
            1, 2, 5, //face right
            1, 5, 6,
            0, 7, 4, //face left
            0, 4, 3,
            5, 4, 7, //face back
            5, 7, 6,
            0, 6, 7, //face bottom
            0, 1, 6
        };
        return new SerializedMesh
        {
            vertices = vertices,
            triangles = triangles
        };
    }
}
