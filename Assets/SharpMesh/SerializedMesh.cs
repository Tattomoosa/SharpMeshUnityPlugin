using UnityEngine;

/// <summary>
/// Unity does not serialize its Mesh class, expecting them to be stored as .asset files.
/// Perhaps storing them that way would be better, but it would create a lot of .asset files.
/// TODO Maybe there is a better way to do this.
/// </summary>
namespace SharpMeshUnity
{
    [System.Serializable]
    public class SerializedMesh
    {
        public Vector3[] vertices;
        public int[] triangles;

        public SerializedMesh(Vector3[] vertices_, int[] triangles_)
        {
            vertices = vertices_;
            triangles = triangles_;
        }
        public SerializedMesh(Mesh mesh)
        {
            vertices = mesh.vertices;
            triangles = mesh.triangles;
        }

        public Mesh IntoMesh()
        {
            return new Mesh
            {
                vertices = vertices,
                triangles = triangles
            };
        }
    }
}
