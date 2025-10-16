#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class TranslationTools {
	[MenuItem("GameObject/UI/TrText")]
	private static void CreateComponentUITrText() {		
		GameObject go = new GameObject("TrText", typeof(RectTransform), typeof(Text), typeof(TrText) );
		go.transform.SetParent( Selection.activeGameObject.transform );
		EditorSceneManager.MarkSceneDirty( SceneManager.GetActiveScene() );
	}

	[MenuItem("GameObject/UI/TrText", true)]
	private static bool ValidateComponentUITrText() {
		return (Selection.activeGameObject != null) && (Selection.activeGameObject.GetComponent<RectTransform>() != null);
	}
}


#endif