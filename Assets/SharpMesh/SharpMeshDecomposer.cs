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
        // TODO many of these properties are public for the sake of the editor only
        // They should be getters/setters to keep this class's state consistent.

        // Method corresponding to any new Decomposers added in the SharpMesh library
        // TODO Look if there are any reflection methods to do some of this work automatically.
        // TODO Keep this enum somewhere else maybe? In the library itself?
        public enum Method { Voxel };
        // Method used by this Decomposer
        public Method method = Method.Voxel;
        // Options used by this Decomposer
        // TODO enforce compliance with method field.
        public DecomposerOptions options = new VoxelOptions();

        /// <summary>
        /// Runs the current Decomposition Method on the input mesh
        /// </summary>
        /// <param name="inputMesh">Mesh to decompose</param>
        /// <returns></returns>
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

        /// <summary>
        /// Changes the method and updates internal state accordingly.
        /// </summary>
        /// <param name="newMethod"></param>
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


        /// <summary>
        /// Helper for editing SharpMesh's DecomposerOptions type
        /// </summary>
        [System.Serializable]
        public class DecomposerOptions
        {
            public float precision = 0.1f;
        }

        /// <summary>
        /// Helper for editing SharpMesh's VoxelOptions type
        /// </summary>
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
    }
}
