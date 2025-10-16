using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
	[CreateAssetMenu(fileName = "sounds", menuName = "SoundsData", order = 1)]
	public class SoundsData : ScriptableObject
	{
	    [System.Serializable]
	    public struct soundInfo
	    {
	        public string name;
	        public AudioClip audio;
	    }

		public List<soundInfo> sounds = new List<soundInfo>();

		public AudioClip getAudioClip(int index)
		{
			if (index >= 0 && index < sounds.Count)
				return sounds[index].audio;

			return null;
		}
	}
}
