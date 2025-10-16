#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public static class STakeScreenshot {

	[MenuItem ("Tools/Take Screenshot")]
	private static void takeScreenshot () {
		
		string path = EditorUtility.SaveFilePanelInProject(
			"Take Screenshot",
			"Screenshot",
			"png",
			"Screenshot level");
		
		if(path != "") {
			ScreenCapture.CaptureScreenshot(path);
		}
	}

	//private static int CurrentLanguage = 0;
	//[MenuItem("Tools/Translations/Next Language")]
	//private static void NextTranslation()
	//{
	//	GameObject SelectedObject = Selection.activeTransform.gameObject;
	//	CurrentLanguage = Mathf.Clamp(CurrentLanguage + 1, 0, (int)Translation.Language.LNG_SIZE - 1);
	//	Debug.Log("CurrentLanguage = " + (Translation.Language)CurrentLanguage);
	//	Translation.GetInstance().SetSelectedLanguage((Translation.Language)CurrentLanguage);
	//	Translation.GetInstance().resetLanguage(SelectedObject);
	//}

	//[MenuItem("Tools/Translations/Prev Language")]
	//private static void PrevTranslation()
	//{
	//	GameObject SelectedObject = Selection.activeTransform.gameObject;
	//	CurrentLanguage = Mathf.Clamp(CurrentLanguage - 1, 0, (int)Translation.Language.LNG_SIZE - 1);
	//	Debug.Log("CurrentLanguage = " + (Translation.Language)CurrentLanguage);
	//	Translation.GetInstance().SetSelectedLanguage((Translation.Language)CurrentLanguage);
	//	Translation.GetInstance().resetLanguage(SelectedObject);
	//}

	//[MenuItem("Tools/Translations/Reset To Default")]
	//private static void ResetToDefault()
	//{
	//	GameObject SelectedObject = Selection.activeTransform.gameObject;
	//	Translation.GetInstance().SetSelectedLanguage(Translation.Language.LNG_NULL);
	//	Translation.GetInstance().resetLanguage(SelectedObject);
	//	SceneView.RepaintAll();
	//}
}


#endif