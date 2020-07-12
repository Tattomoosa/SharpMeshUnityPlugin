using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using SharpMesh;

[CreateAssetMenu(fileName = "SharpMesh", menuName = "SharpMesh", order = 1)]
public class SharpMeshUnity : ScriptableObject 
{
    public Mesh inputMesh;
    public List<Mesh> outputMeshList;

    public void Process()
    {
        if (inputMesh == null)
        {
            Debug.LogError("SharpMesh: Must specify a Mesh to process.");
            return;
        }
        SharpMesh.Mesh sInputMesh = MeshToSharpMesh(inputMesh);
        // TODO change to whatever this will be called
        // SharpMesh.Mesh[] result = SharpMesh.Process(sInputMesh);
        outputMeshList.Clear();
        /*
        foreach (SharpMesh.Mesh m in result)
        {
            outputMeshList.Push(SharpMeshToMesh(m);
        }
         */
        // TODO lol just to see if it works
        Mesh cubeMesh = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().sharedMesh;
        outputMeshList.Add(cubeMesh);
    }

    public void Clear()
    {
        outputMeshList.Clear();
    }

    private SharpMesh.Mesh MeshToSharpMesh(Mesh mesh)
    {
        SharpMesh.Mesh sMesh = new SharpMesh.Mesh();
        // sMesh.vertices = mesh.vertices;
        // sMesh.triangles = mesh.triangles;
        return sMesh;
    }

    private Mesh SharpMeshToMesh(SharpMesh.Mesh sMesh)
    {
        Mesh mesh = new Mesh();
        // mesh.vertices = sMesh.vertices;
        // mesh.triangles = sMesh.triangles;
        return mesh;
    }
}
