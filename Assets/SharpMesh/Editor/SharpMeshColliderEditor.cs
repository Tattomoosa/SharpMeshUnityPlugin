using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SharpMeshUnity;

[CustomEditor(typeof(SharpMeshCollider))]
public class SharpMeshColliderEditor : Editor
{
    private SharpMeshObjectEditor smObjEditor;

    private void OnEnable()
    {
        SharpMeshCollider obj = (SharpMeshCollider)target;
        obj.GetOrCreateSharpMeshObject();
        smObjEditor = (SharpMeshObjectEditor)CreateEditor(obj.sharpMesh);
    }

    public override void OnInspectorGUI()
    {
        SharpMeshCollider obj = (SharpMeshCollider)target;

        // Decomposition Algorithm Settings
        EditorGUILayout.LabelField("[ Decomposition Options ]", EditorStyles.boldLabel);
        smObjEditor.DecompositionOptions(obj.sharpMesh);

        // Input Mesh
        EditorGUILayout.LabelField("[ Input Mesh ]", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useMeshFilter"), new GUIContent("Input From Mesh Filter Component"));
        if (!obj.useMeshFilter)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("inputMesh"));

        // Debug Draw toggle
        EditorGUILayout.PropertyField(serializedObject.FindProperty("debugDraw"));

        // Actions
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

        serializedObject.ApplyModifiedProperties();
    }
}
