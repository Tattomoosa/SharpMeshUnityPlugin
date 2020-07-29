using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SharpMesh.Data;

public class MeshProcess : MonoBehaviour
{
    
    // before mesh
    public UnityEngine.Mesh unityMesh;
    
    // after mesh
    public SharpMesh.Data.Mesh<float> computeMesh;

    // compute mesh timer
    public long elapsedTime;
    
    private void Start()
    {
        // store original mesh
        unityMesh = GetComponent<MeshFilter>().sharedMesh;
        
        var watch = new System.Diagnostics.Stopwatch();
            
        watch.Start();

        computeMesh = new SharpMesh.Data.Mesh<float>
        {
            Triangles = unityMesh.triangles.ToList()
        };

        foreach (var n in unityMesh.vertices)
        {
            // this should be simple but it isn't?
            var arr = new[] {n.x, n.y, n.z};
            
            computeMesh.Vertices.Add(new SharpMesh.Data.Vector<float>(arr));
        }
        
        watch.Stop();

        elapsedTime = watch.ElapsedTicks;
        
        Debug.Log($"Size of the Compute Mesh Vertices: \t {computeMesh.Vertices.Count}");
        Debug.Log($"Order of the Compute Mesh Vertices: \t {computeMesh.Order}");
        Debug.Log($"Size of the Compute Mesh Triangles: \t {computeMesh.Triangles.Count}");
        
        // Here is where we will make a compute Builder.
        
        // When we are done with the computer builder and get a Mesh in return we do the following to reset the mesh.
        unityMesh.triangles = computeMesh.Triangles.ToArray();
        unityMesh.vertices = computeMesh.Vertices.Select(n => new Vector3(n.X, n.Y, n.Z)).ToArray();

    }

    // For the asnyc portion we can check here for an update.
    // Maybe store a task variable in the member function.
    private void Update()
    {
        
    }
}