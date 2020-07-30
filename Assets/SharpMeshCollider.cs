using SharpMeshUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SharpMeshUnity
{
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
                CreateColliderGameObjects();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO for some reason these meshes are only lit from one side...
            if (debugDraw)
                for (int i = 0; i < sharpMeshList.Count; ++i)
                    Graphics.DrawMesh(sharpMeshList[i], transform.position, Quaternion.identity, debugDrawMaterials[i], 0);
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
                debugDrawMaterials.Add(new Material(Shader.Find("Standard"))
                {
                    color = Random.ColorHSV()
                });
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
            sharpMeshColliderParent = new GameObject
            {
                name = "SharpMeshCollider"
            };
            sharpMeshColliderParent.transform.parent = transform;
            sharpMeshColliderParent.transform.position = transform.position;
            foreach (Mesh mesh in sharpMeshList)
            {
                // init game object holding this collider
                GameObject meshColliderObject = new GameObject
                {
                    name = "SharpMesh MeshCollider"
                };
                meshColliderObject.transform.parent = sharpMeshColliderParent.transform;
                meshColliderObject.transform.position = sharpMeshColliderParent.transform.position;
                // add collider component and attach mesh
                MeshCollider meshCollider = meshColliderObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
            }
        }
    }
}
