using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SharpMeshUnity;

[CustomEditor(typeof(SharpMeshCollider))]
public class SharpMeshColliderEditor : Editor
{
    private SharpMeshObjectEditor smObjEditor;
    bool decompOptionsExpanded = false;
    private void OnEnable()
    {
        SharpMeshCollider obj = (SharpMeshCollider)target;
        obj.GetOrCreateSharpMeshObject();
        smObjEditor = (SharpMeshObjectEditor)CreateEditor(obj.sharpMesh);
    }

    public override void OnInspectorGUI()
    {
        SharpMeshCollider obj = (SharpMeshCollider)target;

        EditorGUILayout.LabelField("[ Decomposition Options ]", EditorStyles.boldLabel);
        // Decomposition Algorithm Settings
        smObjEditor.DecompositionOptions(obj.sharpMesh);

        EditorGUILayout.LabelField("[ Input Mesh ]", EditorStyles.boldLabel);
        // Input Mesh
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useMeshFilter"), new GUIContent("Input From Mesh Filter Component"));
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

        if (GUILayout.Button("Clear"))
        {
            obj.Clear();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
