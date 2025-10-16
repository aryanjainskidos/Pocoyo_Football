#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace SoundManager
{
	[CustomEditor(typeof(SoundsData))]
	public class SoundsDataEditor : Editor
	{
	    public override void OnInspectorGUI()
	    {
			serializedObject.Update();

	        DrawDefaultInspector();
	        GUILayout.Space(20);

	        if (GUILayout.Button("Get Sounds"))
	        {
	            searchSounds();
	        }

	        if (GUILayout.Button("Remove Sounds"))
	        {
	            removeSounds();
	        }

			serializedObject.ApplyModifiedProperties();
	    }

		void searchSounds()
		{
			SoundsData myScript = (SoundsData)target;

			string path = EditorUtility.OpenFolderPanel("Load All Sounds", "", "");
			if (path.StartsWith(Application.dataPath)) {
				path = "Assets" + path.Substring(Application.dataPath.Length);
			}

			string [] paths = { path };
			string[] data;
			data = AssetDatabase.FindAssets("t:audioclip", paths);

			myScript.sounds.Clear();

			SoundsData.soundInfo currentSound = new SoundsData.soundInfo();

			int count;

			for (int i = 0; i < data.Length; i++)
			{
				count = 0;

				string guid = data[i];
				string audioPath = AssetDatabase.GUIDToAssetPath(guid);
				AudioClip audio = (AudioClip)AssetDatabase.LoadAssetAtPath(audioPath, typeof(AudioClip));

				for (int j = 0; j < myScript.sounds.Count; j++)
				{
					if (audio.name.CompareTo(myScript.sounds[j].name) != 0)
					{
						count++;
					}
				}

				if (count == myScript.sounds.Count)
				{
					currentSound.name = audio.name;
					currentSound.audio = audio;
					myScript.sounds.Add(currentSound);
				}
			}

			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
		}

		void removeSounds()
		{
			SoundsData myScript = (SoundsData)target;

			myScript.sounds.Clear();
		}
	}
}

#endif