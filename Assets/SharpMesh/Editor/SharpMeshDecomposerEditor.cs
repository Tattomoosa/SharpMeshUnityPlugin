using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SharpMeshUnity;


[CustomEditor(typeof(SharpMeshDecomposer))]
public class SharpMeshDecomposerEditor : Editor
{
    Editor optionsEditor;
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        SharpMeshDecomposer obj = (SharpMeshDecomposer)target;
        // base.OnInspectorGUI();
        DecompositionMethodPopup(obj);
        DecompositionOptions(obj);
    }

    public void DecompositionMethodPopup(SharpMeshDecomposer obj)
    {
        obj.SetMethod((SharpMeshDecomposer.Method)EditorGUILayout.EnumPopup("Decomposition Method", obj.method));
    }

    // TODO Can Unity do some reflection stuff to clean this up as we add more
    // options?
    public void DecompositionOptions(SharpMeshDecomposer obj)
    {
        obj.options.precision = EditorGUILayout.FloatField("Precision", obj.options.precision);
        switch (obj.method)
        {
            case SharpMeshDecomposer.Method.Voxel:
                if (obj.options is SharpMeshDecomposer.VoxelOptions)
                {
                    var opt = (SharpMeshDecomposer.VoxelOptions)obj.options;
                    opt.resolution = EditorGUILayout.IntField("Resolution", opt.resolution);
                }
                break;
            default:
                GUILayout.Label("Unknown Method");
                break;
        }
    }
}
