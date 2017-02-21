using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    SerializedProperty lookAtPoint;
    SerializedProperty a;

    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        a = serializedObject.FindProperty("a");
    }

    TextAsset xx = null;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        xx = EditorGUILayout.ObjectField("DragonBones Data", xx, typeof(TextAsset), false) as TextAsset;
        if(xx)
        if (GUILayout.Button("www"))
        {
            Debug.Log("button");
        }
        GUILayout.Label("asdfasf");
        EditorGUILayout.PropertyField(lookAtPoint, new GUIContent("look at", "tooltip"));
        EditorGUILayout.HelpBox("123124", MessageType.Info);
        EditorGUILayout.PropertyField(a);


        serializedObject.ApplyModifiedProperties();
    }
}
