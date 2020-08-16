using UnityEditor;
using SharpMeshUnity;
using UnityEngine;

/// <summary>
/// Custom Editor for the SharpMesh.SharpMeshObject
/// </summary>
[CustomEditor(typeof(SharpMeshObject))]
public class SharpMeshObjectEditor : Editor
{
    Editor decompOptionsEditor;
    private void OnEnable()
    {
        SharpMeshObject obj = (SharpMeshObject)target;
        obj.GetOrCreateSharpMeshDecomposer();
        // SharpMeshDecomposerEditor decompOptionsEditor = new SharpMeshDecomposerEditor();
        decompOptionsEditor = Editor.CreateEditor(obj.decomposer);
    }

    public override void OnInspectorGUI()
    {
        SharpMeshObject obj = (SharpMeshObject)target;
        // DrawDefaultInspector();
        // EditorGUILayout.PropertyField(serializedObject.FindProperty("decompMethod"));
        DecompositionOptions(obj);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inputMesh"));
        if (GUILayout.Button("Process"))
        {
            Debug.Log("Processing...");
            obj.Process();
            Debug.Log("Done");
        }
        if (GUILayout.Button("Clear"))
        {
            obj.Clear();
        }
        GUILayout.Label("Output Mesh Count: " + obj.IntoMeshList().Count.ToString());
    }

    // TODO make DecompositionOptions its own editor class.
    public void DecompositionOptions(SharpMeshObject obj)
    {
        ((SharpMeshDecomposerEditor)decompOptionsEditor).DecompositionMethodPopup(obj.decomposer);
        ((SharpMeshDecomposerEditor)decompOptionsEditor).DecompositionOptions(obj.decomposer);
    }
}
