using SharpMesh.Decomposer.Voxel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace SharpMeshUnity
{
    [CreateAssetMenu(fileName = "SharpMesh", menuName = "SharpMesh/SharpMesh", order = 1)]
    public class SharpMeshObject : ScriptableObject
    {
        // Mesh to be decomposed.
        public Mesh inputMesh;

        // Decomposition method and options to decompose inputMesh with.
        public SharpMeshDecomposer decomposer = null;

        // Mesh output of a decomposition into a custom serialized Mesh class
        // so it can be saved into game data.
        private List<SerializedMesh> outputMeshList;

        /// <summary>
        /// Processes an input mesh into a list of serialized mesh data.
        /// </summary>
        public void Process()
        {
            Profiler.BeginSample("SharpMesh: Process Input Mesh");
            if (inputMesh == null)
            {
                Debug.LogError("SharpMesh: Must specify a Mesh to process.");
                return;
            }
            SharpMesh.Data.Mesh sInputMesh = MeshToSharpMesh(inputMesh);

            // Clear output mesh list
            outputMeshList = new List<SerializedMesh>();

            // Get the decomposer
            GetOrCreateSharpMeshDecomposer();
            var decompResult = decomposer.Run(sInputMesh);
            // Error TODO: better errors
            if (decompResult == null)
                Debug.LogError("Null Decomposition Error");
            else if (decompResult.FinishedWithError)
                Debug.LogError("Decomposition Finished With Error");
            // Success
            else
            {
                Debug.Log("Input mesh decomposed into " +
                    decompResult.Mesh.Count + " convex meshes.");
                foreach (var mesh in decompResult.Mesh)
                    outputMeshList.Add(new SerializedMesh(SharpMeshToMesh(mesh)));
            }

            Profiler.EndSample();
        }

        /// <summary>
        /// Clears SharpMesh output data.
        /// </summary>
        public void Clear()
        {
            outputMeshList.Clear();
        }

        /// <summary>
        /// If no decomposer exists, creates one.
        /// </summary>
        // Should this be a getter?
        public void GetOrCreateSharpMeshDecomposer()
        {
            if (!decomposer)
                decomposer = ScriptableObject.CreateInstance<SharpMeshDecomposer>();
        }

        // TODO maybe make a utility class to contain all these different type conversions.

        /// <summary>
        /// Creates a SharpMesh.Mesh from a UnityEngine.Data.Mesh
        /// </summary>
        private SharpMesh.Data.Mesh MeshToSharpMesh(Mesh mesh)
        {
            SharpMesh.Data.Mesh sMesh = new SharpMesh.Data.Mesh();
            foreach (Vector3 vertex in mesh.vertices)
                sMesh.Vertices.Add(Vector3ToSharpVector(vertex));
            foreach (int triIndex in mesh.triangles)
                sMesh.Triangles.Add(triIndex);
            return sMesh;
        }

        /// <summary>
        /// Creates a UnityEngine.Mesh from a SharpMesh.Data.Mesh
        /// </summary>
        private Mesh SharpMeshToMesh(SharpMesh.Data.Mesh<float> sMesh)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            foreach (SharpMesh.Data.Vector<float> vertex in sMesh.Vertices)
                vertices.Add(SharpVectorToVector3(vertex));
            mesh.vertices = vertices.ToArray();
            mesh.triangles = sMesh.Triangles.ToArray();
            // TODO keep track of normals? In library?
            // This way should always work but maybe is a bit sloppy.
            mesh.RecalculateNormals();
            return mesh;
        }

        /// <summary>
        /// Creates a SharpMesh.Data.Vector<float> from a UnityEngine.Vector3
        /// </summary>
        private SharpMesh.Data.Vector Vector3ToSharpVector(Vector3 v)
        {
            return new SharpMesh.Data.Vector(new[] { v.x, v.y, v.z });
        }

        /// <summary>
        /// Creates a UnityEngine.Vector3 from a SharpMesh.Data.Vector<float>
        /// </summary>
        private Vector3 SharpVectorToVector3(SharpMesh.Data.Vector<float> v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Returns a list of UnityEngine.Mesh objects from the output SharpMesh
        /// processing, suitable for engine use.
        /// </summary>
        public List<Mesh> IntoMeshList()
        {
            Profiler.BeginSample("SharpMesh: Converting serialized meshes into UnityEngine meshes");
            List<Mesh> meshes = new List<Mesh>();
            foreach (var serializedMesh in outputMeshList)
                meshes.Add(serializedMesh.IntoMesh());
            Profiler.EndSample();
            return meshes;
        }
    }
}
