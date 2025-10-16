using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SoundManager_Pocoyo_Football {
	public class SSoundManager : MonoBehaviour {
		private AudioSource[] m_SourceFX;
		private AudioSource m_SourceMusic;
		// new booleans for mute and unmute the bgm and sfx
		private bool isMusicMuted = false;
		private bool isSfxMuted = false;

	    private const int maxAudioSource = 3;
		private static SSoundManager instance = null;
		private float GeneralSfxVolument = 1.0f;
		private float GeneralMusicVolument = 1.0f;

		public static SSoundManager GetInstance()
		{
			if (instance == null)
			{
				GameObject mySingletonObject = new GameObject("SSoundManager");
				DontDestroyOnLoad(mySingletonObject);
				instance = mySingletonObject.AddComponent<SSoundManager>();
			}

			return instance;
		}

		void Awake () {
	        m_SourceFX = new AudioSource[maxAudioSource];

	        for (int i = 0; i < m_SourceFX.Length; i++)
			{
	            m_SourceFX[i] = gameObject.AddComponent<AudioSource>();
			}

			m_SourceMusic = gameObject.AddComponent<AudioSource> ();
			m_SourceMusic.loop = true;
	        m_SourceMusic.volume = 1.0f;

			Debug.Log("SoundManager CREADO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		}

		public void playSoundFX(AudioClip fx, bool isLoop = false, float volume = 1.0f)
		{
	        if (fx != null)
	        {
	            for (int i = 0; i < m_SourceFX.Length; i++)
	            {
	                if (!m_SourceFX[i].isPlaying)
	                {
	                    m_SourceFX[i].clip = fx;
	                    m_SourceFX[i].loop = isLoop;
	                    m_SourceFX[i].volume = volume * GeneralSfxVolument;
	                    m_SourceFX[i].Play();
	                    break;
	                }
	            }
	        }
		}

		public void playMusic(AudioClip music, bool loop = false)
		{
	        if (music != null) {

				m_SourceMusic.clip = music;
                m_SourceMusic.loop = loop;
                m_SourceMusic.volume = GeneralMusicVolument;
	            m_SourceMusic.Play ();
			}
		}

		public void pauseMusic()
		{
			m_SourceMusic.Pause ();
		}

		public void resumeMusic()
		{
			m_SourceMusic.UnPause ();
		}

		public void stopMusic()
		{
			m_SourceMusic.Stop ();
		}

	    public void stopAllSFX()
	    {
	        for (int i = 0; i < m_SourceFX.Length; i++)
	        {
	            m_SourceFX[i].Stop();
	        }
	    }

		public void stopSFX(AudioClip sound)
	    {
			if (sound != null)
			{
				for (int i = 0; i < m_SourceFX.Length; i++)
				{
					if (m_SourceFX[i].isPlaying && m_SourceFX[i].clip.name == sound.name)
					{
						m_SourceFX[i].Stop();
						break;
					}
				}
			}
	    }

	    public void setVolumeMusic(float volume)
	    {
	        m_SourceMusic.volume = volume;
			GeneralMusicVolument = volume;
	    }

	    public void setVolumeFX(float volume)
	    {
			GeneralSfxVolument = volume;
	        for (int i = 0; i < m_SourceFX.Length; i++)
	        {
	            m_SourceFX[i].volume = volume;
	        }
	    }

		public void mute_all()
        {
			m_SourceMusic.mute = !m_SourceMusic.mute;
			for (int i = 0; i < m_SourceFX.Length; i++)
			{
				m_SourceFX[i].mute = !m_SourceFX[i].mute;
			}
		}

		public void StopAll()
        {
			stopAllSFX();
			stopMusic();
        }

		

		public float GetFxVolume()
        {
			return GeneralSfxVolument;

		}

		public float GetMusicVolume()
		{
			return GeneralMusicVolument;

		}

		public void ToggleMusicMute()
			{
				isMusicMuted = !isMusicMuted;
				m_SourceMusic.mute = isMusicMuted;
			}

	public void ToggleSfxMute()
		{
			isSfxMuted = !isSfxMuted;
			for (int i = 0; i < m_SourceFX.Length; i++)
			{
				m_SourceFX[i].mute = isSfxMuted;
			}
		}

		public bool IsMusicMuted() { return isMusicMuted; }
		public bool IsSfxMuted() { return isSfxMuted; }
	}
}
