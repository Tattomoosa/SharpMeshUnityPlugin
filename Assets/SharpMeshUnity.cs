using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using SharpMesh;

[CreateAssetMenu(fileName = "SharpMesh", menuName = "ScriptableObjects/SharpMeshUnity", order = 1)]
public class SharpMeshUnity : ScriptableObject 
{
    public Mesh inputMesh;
    public List<Mesh> outputMeshList;

    public void Process()
    {
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
    }

    private SharpMesh.Mesh MeshToSharpMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        return new SharpMesh.Mesh();
    }

    private Mesh SharpMeshToMesh(SharpMesh.Mesh sMesh)
    {
        return new Mesh();
    }
}
