using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {
	[AddComponentMenu("SoundManager/UITrSound")]
	public class UITrSound : MonoBehaviour {
		public TrSoundsData soundData;
		public bool PlayOnStart = false;
		public bool PlayOnDisable = false;
		public bool PlayOnLoop = false;
		public float Delay = 0.0f;
		public float Repeat = Mathf.Infinity;
		[Range(0f, 1f)]
		public float Volumen = 1.0f;
		[HideInInspector]
		public List<string> soundId = new List<string>();
		[HideInInspector]
		public float AudioClipLength = 0.0f;
		private AudioClip currentClip;

		// Use this for initialization
		void Start () {
			if( PlayOnStart ) Restart();
		}

		void OnDisable() {
			if( PlayOnDisable ) Invoke("Play", 0.0f);
		}

		public void Restart() {
            Stop();
			InvokeRepeating("Play", Delay, Repeat);
		}

		public void Play() {
            if (soundId.Count == 0) return;
			int i = Random.Range(0, soundId.Count);
			currentClip = soundData.getAudioClip(soundId[i]);
			SSoundManager.GetInstance().playSoundFX(currentClip, PlayOnLoop, Volumen);
			AudioClipLength = currentClip.length;
		}

        public void Play(int i)
        {
            if (i < 0 || i >= soundId.Count) return;

            currentClip = soundData.getAudioClip(soundId[i]);
            SSoundManager.GetInstance().playSoundFX(currentClip, PlayOnLoop, Volumen);
            AudioClipLength = currentClip.length;
        }

        public void Stop() {
			AudioClipLength = 0.0f;
			CancelInvoke();
            SSoundManager.GetInstance().stopSFX(currentClip);
		}

		public void StopMusic()
        {
			SSoundManager.GetInstance().stopMusic();

		}

		public void StopAllFx()
        {
			SSoundManager.GetInstance().stopAllSFX();

		}

		public void StopAll()
        {
			SSoundManager.GetInstance().StopAll();

		}
	}
}
