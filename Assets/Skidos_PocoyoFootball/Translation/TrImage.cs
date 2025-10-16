using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/TrImage")]
public class TrImage : MonoBehaviour {

	[System.Serializable]
	public struct TranslationEntry {
		public string languageId;
		public Sprite image;
	};

	public Sprite defaultImage;
	public List<TranslationEntry> Images = new List<TranslationEntry>();

	void Reset() {
		TranslationData translationData = Resources.Load<TranslationData>("languages") as TranslationData;
		if(translationData == null){
			Debug.Log( "TrImage: Try create without TranslationData");
			return;
		}

		foreach(TranslationData.Language lang in translationData.Languages)
		{
			if( !Images.Exists( x => ( x.languageId == lang.languageId ) ) )
			{
				TranslationEntry newEntry = new TranslationEntry();
				newEntry.image = defaultImage;
				newEntry.languageId = lang.languageId;
				Images.Add(newEntry);
			}
		}
	}

	void Start () {
		ResetImage();
	}
	
	public void ResetImage() {
		Image image = GetComponent<Image>();
		if( Images.Exists( x => ( x.languageId == Translation.GetInstance().getSystemLanguage() ) ) )
			image.sprite = Images.Find( x => ( x.languageId == Translation.GetInstance().getSystemLanguage() ) ).image;
		else
			image.sprite = defaultImage;
	}
}
