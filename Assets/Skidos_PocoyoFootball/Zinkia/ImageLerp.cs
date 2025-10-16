using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

#endif

[RequireComponent(typeof(Image))]
public class ImageLerp : MonoBehaviour {

	#if UNITY_EDITOR
	[ContextMenu("FillWithFolder")]
	void FillWithFolder() {
		string path = EditorUtility.OpenFolderPanel("Load All Sprites", "", "");
		if (path.StartsWith(Application.dataPath)) {
			path = "Assets" + path.Substring(Application.dataPath.Length);
		}

		string [] paths = { path };
		string[] data;
		data = AssetDatabase.FindAssets("t:sprite", paths);

		ImagesList = new List<Sprite>();
		foreach (string guid in data)
		{
			string spritePath = AssetDatabase.GUIDToAssetPath(guid);
			Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
			ImagesList.Add( sprite );
		}
	}

    [ContextMenu("FillWithTexturePacker")]
    void FillWithTexturePacker()
    {
        string path = EditorUtility.OpenFilePanel("Load Texture Packer", "", "png");
        if (path.StartsWith(Application.dataPath))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
        }

        Object[] data;
        data = AssetDatabase.LoadAllAssetsAtPath(path);

        ImagesList = new List<Sprite>();
        for(int i=0; i < data.Length; i++)
        {
            Sprite spr = data[i] as Sprite;
            if (spr != null)
                ImagesList.Add(spr);
        }
    }
#endif

    public List<Sprite> ImagesList;
	[Range(0f,1f)]
	public float CurrentFrame = 0f;

	public float Value {
		set {
			currentValue = value;
			currentValue = Mathf.Clamp01(currentValue);
			int spriteIndex = (int)Mathf.Lerp(0, ImagesList.Count - 1, currentValue);
			_image.sprite = ImagesList[spriteIndex];
		}

		get {
			return currentValue;
		}
	}

	private Image _image;
	private float currentValue;

	// Use this for initialization
	void Start () {
		currentValue = 0.0f;
		_image = GetComponent<Image>();
		_image.sprite = ImagesList[0];
	}

	void OnValidate() {
		/*Image ed_image = GetComponent<Image>();
		int spriteIndex = (int)Mathf.Lerp(0, ImagesList.Count - 1, CurrentFrame);
		ed_image.sprite = ImagesList[spriteIndex];*/
	}
}
