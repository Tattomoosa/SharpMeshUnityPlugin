using SharpMesh.Decomposer.Voxel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace SharpMeshUnity
{
    [CreateAssetMenu(fileName = "SharpMesh", menuName = "SharpMesh/SharpMesh", order = 1)]
    public class SharpMeshObject : ScriptableObject
    {
        public Mesh inputMesh;
        public SharpMeshDecomposer decomposer = null;
        private List<SerializedMesh> outputMeshList;

        private double lastDecompositionTime = -1.0;

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
            // SharpMesh.Data.Mesh<float> sInputMesh = MeshToSharpMesh(inputMesh);
            SharpMesh.Data.Mesh sInputMesh = MeshToSharpMesh(inputMesh);
            // TODO
            // 1. Create SharpMesh processor (maybe keep it as class member?)
            // 2. Get output processed mesh
            // 3. Convert to serialized mesh for outputMeshList

            // Clear output mesh list
            outputMeshList = new List<SerializedMesh>();

            // Build decomposer
            GetOrCreateSharpMeshDecomposer();
            var decompResult = decomposer.Run(sInputMesh);
            // Error TODO better errors
            if (decompResult == null)
                Debug.LogError("Null Decomposition Error");
            else if (decompResult.FinishedWithError)
                Debug.LogError("Decomposition Finished With Error");
            // Success
            else
            {
                lastDecompositionTime = decompResult.TimeTaken;
                Debug.Log("Input mesh decomposed in " + lastDecompositionTime +
                    ", resulting in " + decompResult.Mesh.Count + " convex meshes.");
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

        public void GetOrCreateSharpMeshDecomposer()
        {
            if (!decomposer)
                decomposer = ScriptableObject.CreateInstance<SharpMeshDecomposer>();
        }

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
                // sMesh.Triangles = mesh.triangles.ToList();
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
            // TODO keep track of normals
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
            return new SerializedMesh(vertices, triangles);
        }
    }
}
