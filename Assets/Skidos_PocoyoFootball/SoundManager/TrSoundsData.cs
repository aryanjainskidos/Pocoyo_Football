using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
	[CreateAssetMenu(fileName = "trsounds", menuName = "TrSoundsData", order = 2)]
	public class TrSoundsData : ScriptableObject {

		[System.Serializable]
		public struct AudioEntry {
			public string stringId;
			public AudioClip voice;
		};

		[System.Serializable]
		public struct Narration {
			public string languageId;
			public List<AudioEntry> narrations;
		};

		public const string no_language = "unknown";
		[ReadOnly]
		public string useLanguage = string.Empty;
		public List<Narration> voicesCollection = new List<Narration>();

		public AudioClip getAudioClip( string id ) {
			if (useLanguage == no_language) return null;

			string lang = useLanguage;

			if(string.IsNullOrEmpty(useLanguage))
				lang = Translation.GetInstance().getSystemLanguage();

			int idx = voicesCollection.FindIndex( x => (x.languageId == lang) );

			if(idx > -1)
			{
				Narration voices = voicesCollection[idx];
				int idv = voices.narrations.FindIndex( x => (x.stringId == id) );
				if(idv > -1)
				{
					AudioEntry entry = voices.narrations[idv];
					return entry.voice;
				}
			}

			return null;
		}
	}
}
