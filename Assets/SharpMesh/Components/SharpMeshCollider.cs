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
        public bool useMeshFilter = true;
        public Mesh inputMesh;
        public bool debugDraw = true;
        [SerializeField]
        public SharpMeshObject sharpMesh;

        private GameObject sharpMeshColliderParent;
        private List<Mesh> sharpMeshList;
        private List<Material> debugDrawMaterials;

        // Start is called before the first frame update
        void Start()
        {
            Process();
            if (Application.isPlaying)
                CreateColliderGameObjects();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO should debugDraw draw during play? Colliders can be visualized without it...
            if (Application.isPlaying)
            {
            }
            // TODO for some reason these meshes are only lit from one side...
            else if (debugDraw)
            {
                for (int i = 0; i < sharpMeshList.Count; ++i)
                    Graphics.DrawMesh(sharpMeshList[i], transform.position, Quaternion.identity, debugDrawMaterials[i], 0);
            }
        }

        private void GetInputMesh()
        {
            if (useMeshFilter)
                inputMesh = GetComponent<MeshFilter>().sharedMesh;
            if (!inputMesh)
                Debug.LogError("Input mesh required.");
        }

        public void GetOrCreateSharpMeshObject()
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

        public void Process()
        {
            Debug.Log("Processing...");
            GetInputMesh();
            GetOrCreateSharpMeshObject();
            sharpMesh.inputMesh = inputMesh;
            sharpMesh.Process();
            sharpMeshList = sharpMesh.IntoMeshList();
            InitDebugDraw();
        }

        public void Clear()
        {
            if (sharpMeshColliderParent != null)
                Destroy(sharpMeshColliderParent);
        }

        /// <summary>
        /// Creates and initializes child GameObjects which actually contain the MeshCollider components.
        /// This is expensive on game 
        /// </summary>
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
                meshCollider.convex = true;
                meshCollider.sharedMesh = mesh;
                // meshCollider.sharedMesh = mesh;
            }
        }
    }
}
