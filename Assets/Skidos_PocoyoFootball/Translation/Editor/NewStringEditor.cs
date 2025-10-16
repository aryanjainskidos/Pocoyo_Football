#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class NewStringEditor : EditorWindow {
	private string groupId = "";
	private string stringId = "";
	private string translation = "";

	private TranslationData translationData;

	void OnEnable() {
		stringId = Translation.NextStringId();
		if(translationData == null)
		{
			translationData = Resources.Load<TranslationData>("languages") as TranslationData;
		}
	}

	void OnGUI() 
	{
		BeginWindows();
		EditorGUILayout.LabelField("Group ID");
		groupId = EditorGUILayout.TextArea(groupId);
		EditorGUILayout.LabelField("String ID");
		EditorGUILayout.LabelField(stringId);
		EditorGUILayout.LabelField("Original Text");
		translation = EditorGUILayout.TextArea(translation);
		EditorGUILayout.BeginHorizontal();
		int fidx = translationData.OriginalText.translations.FindIndex( x => ( x.grougId == groupId && x.translation == translation ) );
		GUI.enabled = (translation.Length != 0 && fidx == -1);
		if(GUILayout.Button("Add String"))
		{
			TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
			newEntry.grougId = groupId;
			newEntry.stringId = stringId;
			newEntry.translation = translation;
			translationData.OriginalText.translations.Add(newEntry);

			newEntry.translation = "";
			foreach(var lang in translationData.Languages)
				lang.translations.Add(newEntry);
			this.Close();
		}
		GUI.enabled = true;
		if(GUILayout.Button("Cancel"))
			this.Close();
		EditorGUILayout.EndHorizontal();
		EndWindows();
	}
}


#endif