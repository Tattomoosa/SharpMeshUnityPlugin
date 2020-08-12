using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SharpMeshUnity;

[CustomEditor(typeof(SharpMeshCollider))]
public class SharpMeshColliderEditor : Editor
{
    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        SharpMeshCollider obj = (SharpMeshCollider)target;

        // Input Mesh
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useMeshFilter"), new GUIContent("Use Mesh Filter as Input Mesh"));
        if (!obj.useMeshFilter)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("inputMesh"));

        // Debug Draw toggle
        EditorGUILayout.PropertyField(serializedObject.FindProperty("debugDraw"));

        if (GUILayout.Button("Process"))
        {
            Debug.Log("Processing...");
            obj.Process();
            Debug.Log("Done");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
