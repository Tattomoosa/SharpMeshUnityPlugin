// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using UnityEditor;
using SharpMeshUnity;
using UnityEngine;

/// <summary>
/// Custom Editor for the SharpMesh.SharpMeshObject
/// </summary>
[CustomEditor(typeof(SharpMeshObject))]
public class SharpMeshObjectEditor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        SharpMeshObject obj = (SharpMeshObject)target;
        // DrawDefaultInspector();
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
}
