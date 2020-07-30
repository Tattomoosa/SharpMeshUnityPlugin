using SharpMeshUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SharpMeshCollider : MonoBehaviour
{
    public Mesh inputMesh;
    public bool debugDraw = true;
    private SharpMeshObject sharpMesh;

    private GameObject sharpMeshColliderParent;
    private List<Mesh> sharpMeshList;
    private List<Material> debugDrawMaterials;

    // Start is called before the first frame update
    void Start()
    {
        Process();
        InitDebugDraw();
        if (Application.isPlaying)
        {
            CreateColliderGameObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO for some reason these meshes are only lit from one side...
        if (debugDraw)
        {
            for (int i = 0; i < sharpMeshList.Count; ++i)
            {
                Graphics.DrawMesh(sharpMeshList[i], transform.position, Quaternion.identity, debugDrawMaterials[i], 0);
            }
        }
    }

    private void GetInputMesh()
    {
        if (!inputMesh)
            inputMesh = GetComponent<MeshFilter>().sharedMesh;
        if (!inputMesh)
            Debug.LogError("Input mesh required.");
    }

    private void GetOrCreateSharpMeshObject()
    {
        if (!sharpMesh)
            sharpMesh = ScriptableObject.CreateInstance<SharpMeshObject>();
    }

    private void InitDebugDraw()
    {
        debugDrawMaterials = new List<Material>();
        foreach (Mesh mesh in sharpMeshList)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = Random.ColorHSV();
            debugDrawMaterials.Add(material);
        }
    }

    void Process()
    {
        Debug.Log("Processing...");
        GetInputMesh();
        GetOrCreateSharpMeshObject();
        sharpMesh.inputMesh = inputMesh;
        sharpMesh.Process();
        sharpMeshList = sharpMesh.IntoMeshList();
    }

    void CreateColliderGameObjects()
    {
        // init parent
        sharpMeshColliderParent = new GameObject();
        sharpMeshColliderParent.name = "SharpMeshCollider";
        sharpMeshColliderParent.transform.parent = transform;
        sharpMeshColliderParent.transform.position = transform.position;
        foreach (Mesh mesh in sharpMeshList)
        {
            GameObject meshColliderObject = new GameObject();
            meshColliderObject.name = "SharpMesh MeshCollider";
            meshColliderObject.transform.parent = sharpMeshColliderParent.transform;
            meshColliderObject.transform.position = sharpMeshColliderParent.transform.position;
            MeshCollider meshCollider = meshColliderObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
    }
}
