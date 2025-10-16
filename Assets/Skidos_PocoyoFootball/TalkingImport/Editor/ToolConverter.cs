#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ToolConverter
{
    [MenuItem("Tools/Test Conversion")]
    public static void TestConversor()
    {
        Debug.Log("Hello");
        string path = EditorUtility.OpenFilePanel("Select Animation File", "", "");
        if(path.Length != 0)
        {
            PocoyoAnimation pocoyoAnimation = new PocoyoAnimation("", path, false);
            Debug.Log("Blan");
        }
    }
}


#endif