// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Editor for the SharpMeshUnity object.
/// </summary>
[CustomEditor(typeof(SharpMeshUnity))]
public class SharpMeshUnityEditor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        SharpMeshUnity obj = (SharpMeshUnity)target;
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
        /*
        foreach (SerializedMesh mesh in obj.outputMeshList)
        {
            GUILayout.Label(mesh.ToString());
        }
        */
        // GUILayout.Label(obj.outputMeshList.Count.ToString());
        GUILayout.Label("Output Mesh Count: " + obj.outputMeshList.Count.ToString());
    }
}
