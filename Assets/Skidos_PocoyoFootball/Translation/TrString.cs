using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/TrString")]
public class TrString : MonoBehaviour {
	#if UNITY_EDITOR
	void Reset() {
		if(groupId == null || groupId.Length == 0)
		{
			Scene scene = SceneManager.GetActiveScene();
			groupId = scene.name;
		}
	}

	[ContextMenu("SetupGroupId")]
	public void SetupGroupId()
	{
		Scene scene = SceneManager.GetActiveScene();
		groupId = scene.name;

		UnityEditor.EditorUtility.SetDirty(this);
	}
	#endif

	[System.Serializable]
	public struct TrStringElement {
		public string OriginalText;
		[ReadOnly] public string stringId;
	}

	public Text TextToWrite;
	public int CurrentIndex = -1;
	public bool FillOnStart = false;
	public bool FillRandom = false;
	[Space(10)]
	[Header("Translation Data")]
	[ReadOnly] public string groupId;
	public List<TrStringElement> strings = new List<TrStringElement>();

	void OnEnable() {
		if(FillOnStart)
		{
			if(FillRandom)
				UseRandom();
			else
				UseIndex(CurrentIndex);
		}
	}

	private void initTranslate() {
		if(TextToWrite == null || CurrentIndex < 0 || CurrentIndex > strings.Count - 1) return;
		Translation.GetInstance ().getString(TextToWrite, strings[CurrentIndex].stringId.ToLower());
	}

	public void UseIndex(int i) {
		if (i == -1) TextToWrite.text = string.Empty;
		if(i > -1 && i < strings.Count)
		{
			CurrentIndex = i;
			initTranslate();
		}
	}

	public void UseRandom() {
		CurrentIndex = UnityEngine.Random.Range(0, strings.Count);
		initTranslate();
	}

	public void resetTranslate()
	{
		initTranslate ();
	}

    public string GetStringIndex(int i)
    {
        if (i > -1 && i < strings.Count)
        {
            return Translation.GetInstance().getString(strings[i].stringId.ToLower());
        }

        return "Out Of Index";
    }

    public string GetStringRandom()
    {
        int i = UnityEngine.Random.Range(0, strings.Count);
        return Translation.GetInstance().getString(strings[i].stringId.ToLower());
    }
}

