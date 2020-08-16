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
        // Whether to try and use a MeshFilter component existing on the GameObject
        public bool useMeshFilter = true;
        // Mesh to decompose.
        public Mesh inputMesh;
        // Whether to draw the decomposed Mesh in edit mode
        public bool debugDraw = true;
        // Sharp Mesh Object to keep track of Mesh processing.
        [SerializeField]
        public SharpMeshObject sharpMesh;

        // GameObject used during runtime to hold all colliders managed by SharpMesh
        private GameObject sharpMeshColliderParent;
        // List of Mesh's decomposed by SharpMesh
        private List<Mesh> sharpMeshList;
        // List of materials used to draw debug objects.
        private List<Material> debugDrawMaterials;

        // Start is called before the first frame update
        void Start()
        {
            // TODO why does Process need to get run at the start?
            // This isn't ideal. Is sharpMeshList getting cleared as
            // the game launches for some reason?
            Process();
            if (Application.isPlaying && sharpMeshList != null)
                CreateColliderGameObjects();
        }

        /// <summary>
        /// Called every frame. Updates debug draw in edit mode.
        /// TODO use an editor-only guard so its never called in production.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying && debugDraw && sharpMeshList != null)
            {
                for (int i = 0; i < sharpMeshList.Count; ++i)
                    Graphics.DrawMesh(sharpMeshList[i], transform.position, Quaternion.identity, debugDrawMaterials[i], 0);
            }
        }

        /// <summary>
        /// Gets the input mesh, either from a MeshFilter component or a custom specified mesh input.
        /// </summary>
        private void GetInputMesh()
        {
            if (useMeshFilter)
                inputMesh = GetComponent<MeshFilter>().sharedMesh;
            if (!inputMesh)
                Debug.LogError("Input mesh required.");
        }

        /// <summary>
        /// Gets or creates a SharpMesh ScriptableObject
        /// </summary>
        public void GetOrCreateSharpMeshObject()
        {
            if (!sharpMesh)
                sharpMesh = ScriptableObject.CreateInstance<SharpMeshObject>();
        }

        /// <summary>
        /// Populates materials of random colors for the debug draw mesh.
        /// </summary>
        private void InitDebugDraw()
        {
            debugDrawMaterials = new List<Material>();
            foreach (Mesh mesh in sharpMeshList)
                debugDrawMaterials.Add(new Material(Shader.Find("Standard"))
                {
                    color = Random.ColorHSV()
                });
        }

        /// <summary>
        /// Processes and applies the SharpMesh decomposition.
        /// </summary>
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

        /// <summary>
        /// Clears the SharpMesh mesh list data. It will need to be
        /// Processed again to get it back.
        /// </summary>
        public void Clear()
        {
            if (sharpMeshColliderParent != null)
                Destroy(sharpMeshColliderParent);
            sharpMeshList.Clear();
            sharpMesh.Clear();
            // TODO lmao there must be a better way.
            // Notes: Clear doesn't properly clear the debugDraw mesh for some reason
            // Faking movement of the object causes it to update. It is avoided during runtime.
            // It's still pretty dang hacky tho.
            if (!Application.isPlaying && debugDraw)
            {
                transform.position = transform.position + Vector3.one * 0.1f;
                transform.position = transform.position - Vector3.one * 0.1f;
            }
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
