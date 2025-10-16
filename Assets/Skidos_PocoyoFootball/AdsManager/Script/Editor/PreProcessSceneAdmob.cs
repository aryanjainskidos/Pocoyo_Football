#if UNITY_EDITOR
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Reporting;

public class PreProcessSceneAdmob : IProcessSceneWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        List<GameObject> roots = new List<GameObject>();
        scene.GetRootGameObjects(roots);

        foreach(GameObject go in roots)
        {
            SAdsManager ads = go.GetComponent<SAdsManager>();
            if ( ads == null) continue;

            string temp = ads.AdmobID;
            string temp_ios = ads.AdmobID_IOS;

            string code = "public class Admob { public static string AppId = \"" + temp + "\"; public static string AppId_iOS = \"" + temp_ios + "\"; }";

            string temporaryTextFileName = "Admob.cs";

            var script = MonoScript.FromMonoBehaviour(ads);
            var path = AssetDatabase.GetAssetPath(script);

            string filePath = Path.GetDirectoryName(path);

            string outPath = Path.Combine(Application.dataPath, "../");
            outPath = Path.Combine(outPath, filePath);
            outPath = Path.Combine(outPath, "Editor");
            outPath = Path.Combine(outPath, temporaryTextFileName);
            outPath = Path.GetFullPath(outPath);


            File.WriteAllText(outPath, code);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

#endif
