// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SharpMeshUnity))]
public class SharpMeshUnityEditor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        SharpMeshUnity obj = (SharpMeshUnity)target;
        DrawDefaultInspector();
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
    }
}
