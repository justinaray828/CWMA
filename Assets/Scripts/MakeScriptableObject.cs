﻿using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
public class MakeScriptableObject
{
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        DialogueData asset = ScriptableObject.CreateInstance<DialogueData>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif