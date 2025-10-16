using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
[AddComponentMenu("UI/TrText")]
public class TrText : MonoBehaviour {
#if UNITY_EDITOR
	void Reset() {
		if(groupId == null || groupId.Length == 0)
		{
			Scene scene = SceneManager.GetActiveScene();
			groupId = scene.name;
		}
		if(stringId == null || stringId.Length == 0)
			stringId = Translation.NextStringId();
	}
#endif
	
	[ReadOnly] public string groupId;
	[ReadOnly] public string stringId;

	void Start () {
		initTranslate ();
	}

	private void initTranslate() {
		Text text = GetComponent<Text> ();
		Translation.GetInstance ().getString(text, stringId.ToLower());
	}

	public void resetTranslate()
	{
		initTranslate ();
	}
}
