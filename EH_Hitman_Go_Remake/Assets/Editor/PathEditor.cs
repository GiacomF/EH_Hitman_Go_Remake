#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapPath))]
public class PathEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        MapPath newMapPath = (MapPath)target;
        newMapPath.TestInt = EditorGUILayout.IntField("Test Int ", newMapPath.TestInt);
        

        EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif