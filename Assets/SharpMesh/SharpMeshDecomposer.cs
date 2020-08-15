using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace SharpMeshUnity
{
    /// <summary>
    /// Wrapper around SharpMesh's Decomposer classes/options to facilitate custom Unity editors for them.
    /// </summary>
    [CreateAssetMenu(fileName = "Decomposer", menuName = "SharpMesh/Decomposer", order = 1)]
    public class SharpMeshDecomposer : ScriptableObject
    {
        public enum Method { Voxel };
        public Method method = Method.Voxel;
        public DecomposerOptions options = new VoxelOptions();

        [System.Serializable]
        public class DecomposerOptions
        {
            public float precision = 0.1f;
            // public int timeout = 0;
        }

        public SharpMesh.Decomposer.DecomposerResult Run(SharpMesh.Data.Mesh inputMesh)
        {
            switch (method)
            {
                case Method.Voxel:
                    if (!(options is VoxelOptions))
                        options = new VoxelOptions();
                    return new SharpMesh.Decomposer.Voxel.VoxelDecomposer(inputMesh, ((VoxelOptions)options).Create()).Run();
                default:
                    Debug.LogError("Unknown Method");
                    return null;
            }
        }

        [System.Serializable]
        public class VoxelOptions : DecomposerOptions
        {
            // TODO link up with physics primitives
            // BaseShape baseShape = 

            public SharpMesh.Decomposer.Voxel.VoxelOptions Create()
            {
                // TODO add other properties
                return new SharpMesh.Decomposer.Voxel.VoxelOptions(
                        precision
                    );
            }

        }

        public void SetMethod (Method newMethod)
        {
            // No need to change method and overwrite settings for no reason
            if (method == newMethod)
                return;
            switch (newMethod)
            {
                case Method.Voxel:
                    Debug.Log("Creating new Voxel Decomposer");
                    options = new VoxelOptions();
                    break;
                default:
                    Debug.LogError("Unknown method.");
                    break;
            }
            method = newMethod;
        }

    }
}
