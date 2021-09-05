using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(NestedAssetParent))]
public class NestedAssetParentEditor : Editor
{
    private ReorderableList _reorderableList;
    private SerializedProperty _childs;

    private void OnEnable()
    {
        _childs = serializedObject.FindProperty(NestedAssetParent.childs_prop_name);
        _reorderableList = new ReorderableList(serializedObject, _childs);
        _reorderableList.onAddCallback = OnAddCallback;
        _reorderableList.onRemoveCallback = OnRemoveCallback;
        _reorderableList.drawElementCallback = DrawElementCallback;
    }

    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        SerializedObject child = new SerializedObject(_childs.GetArrayElementAtIndex(index).objectReferenceValue);
        SerializedProperty childName = child.FindProperty(NestedAsset.editor_name_prop_name);
        
        child.Update();

        rect.height = EditorGUIUtility.singleLineHeight;
        rect.y += 2f;
        EditorGUI.PropertyField(rect, childName, new GUIContent("Child " + index));

        if (child.ApplyModifiedProperties())
        {
            if (child.targetObject.name != childName.stringValue)
            {
                child.targetObject.name = childName.stringValue;
                AssetDatabase.SaveAssets();
            }
        }
    }

    private void OnRemoveCallback(ReorderableList list)
    {
        DeleteChild(list.index);
    }

    private void OnAddCallback(ReorderableList list)
    {
        _childs.arraySize++;
        _childs.GetArrayElementAtIndex(_childs.arraySize - 1).objectReferenceValue = CreateChild();
    }

    public override void OnInspectorGUI()
    {
        //SerializedProperty childs = 

        serializedObject.Update();
        
        // GUI.enabled = false;
        // EditorGUILayout.PropertyField(childs);
        // GUI.enabled = true;
        
        _reorderableList.DoLayoutList();
        
        // if (GUILayout.Button("Create child"))
        // {
        //     childs.arraySize++;
        //     childs.GetArrayElementAtIndex(childs.arraySize - 1).objectReferenceValue = CreateChild();
        // }
        //
        // if (GUILayout.Button("Delete child"))
        // {
        //     var child = childs.GetArrayElementAtIndex(childs.arraySize - 1);
        //     DeleteChild(child.objectReferenceValue);
        //     childs.arraySize--;
        // }

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

    private void DeleteChild(int index)
    {
        var child = _childs.GetArrayElementAtIndex(index).objectReferenceValue;
        _childs.GetArrayElementAtIndex(index).objectReferenceValue = null;
        _childs.DeleteArrayElementAtIndex(index);
        AssetDatabase.RemoveObjectFromAsset(child);
        Object.DestroyImmediate(child, true);
        AssetDatabase.SaveAssets();
    }
}
