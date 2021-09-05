using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(NestedAssetParent))]
public class NestedAssetParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty childs = serializedObject.FindProperty(nameof(NestedAssetParent.childs));

        serializedObject.Update();
        
        GUI.enabled = false;
        EditorGUILayout.PropertyField(childs);
        GUI.enabled = true;
        
        if (GUILayout.Button("Create child"))
        {
            childs.arraySize++;
            childs.GetArrayElementAtIndex(childs.arraySize - 1).objectReferenceValue = CreateChild();
        }

        if (GUILayout.Button("Delete child"))
        {
            var child = childs.GetArrayElementAtIndex(childs.arraySize - 1);
            DeleteChild(child.objectReferenceValue);
            childs.arraySize--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private NestedAsset CreateChild()
    {
        var child = ScriptableObject.CreateInstance<NestedAsset>();
        child.name = nameof(NestedAsset);
        AssetDatabase.AddObjectToAsset(child, target);
        AssetDatabase.SaveAssets();
        return child;
    }

    private void DeleteChild(Object child)
    {
        AssetDatabase.RemoveObjectFromAsset(child);
        Object.DestroyImmediate(child, true);
        AssetDatabase.SaveAssets();
    }
}
