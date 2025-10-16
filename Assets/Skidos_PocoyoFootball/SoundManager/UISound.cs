using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {
	[AddComponentMenu("SoundManager/UISound")]
	public class UISound : MonoBehaviour {
		public SoundsData soundData;
		public bool PlayOnStart = false;
		public bool PlayOnDisable = false;
		public bool PlayOnLoop = false;
		public float Delay = 0.0f;
		public float Repeat = Mathf.Infinity;
		[Range(0f,1f)]
		public float Volumen = 1.0f;
		public List<int> soundIndex = new List<int>();

		[HideInInspector]
		public float AudioClipLength = 0f;

		// Use this for initialization
		void Start () {
			if( PlayOnStart ) Restart();
		}

		void OnDisable() {
			if( PlayOnDisable ) Invoke("Play", 0.0f);
		}

		public void Restart() {
			InvokeRepeating("Play", Delay, Repeat);
		}

		public void Play() {
			int i = Random.Range(0, soundIndex.Count);
			AudioClip clip = soundData.getAudioClip(soundIndex[i]);
			SSoundManager.GetInstance().playSoundFX(clip, PlayOnLoop, Volumen);
			AudioClipLength = clip.length;
		}

		public void Play(int index) {
			if(index < 0 || index > soundIndex.Count) return;

			AudioClip clip = soundData.getAudioClip(soundIndex[index]);
			SSoundManager.GetInstance().playSoundFX(clip, PlayOnLoop, Volumen);
			AudioClipLength = clip.length;
		}

		public void Stop() {
			AudioClipLength = 0f;
			CancelInvoke();
		}

        public void Stop(int index)
        {
            if (index < 0 || index > soundIndex.Count) return;

            AudioClip clip = soundData.getAudioClip(soundIndex[index]);
            SSoundManager.GetInstance().stopSFX(clip);
			AudioClipLength = 0f;
		}

        public void StopAll()
        {
            SSoundManager.GetInstance().stopAllSFX();
			AudioClipLength = 0f;
		}
    }
}
