using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FacialAnimation))]
public class TestButton : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FacialAnimation myScript = (FacialAnimation)target;
        if (GUILayout.Button("Test"))
        {
           
        }
    }
}
