using UnityEngine;

/// <summary>
/// Unity does not serialize its Mesh class, expecting them to be stored as .asset files.
/// Perhaps storing them that way would be better, but it would create a lot of .asset files.
/// TODO Maybe there is a better way to do this.
/// </summary>
[System.Serializable]
public class SerializedMesh
{
    public Vector3[] vertices;
    public int[] triangles;

    public Mesh IntoMesh()
    {
        return new Mesh
        {
            vertices = vertices,
            triangles = triangles
        };
    }
}
