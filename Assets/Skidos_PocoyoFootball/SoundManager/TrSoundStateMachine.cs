using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SoundManager {
	public class TrSoundStateMachine : StateMachineBehaviour {

		public TrSoundsData soundData;
		[HideInInspector]
		public List<string> sound = new List<string>();
	    public float timeToStart = 0;
	    public bool loop = false;
	    public bool stopSound = true;

	    private float timeToStartNormalize;
	    private bool isSoundLaunched = false;
	    private bool isAnimationFinish = false;

	    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	        isSoundLaunched = false;
	        isAnimationFinish = false;

	        if (timeToStart > 0)
	        {
	            timeToStartNormalize = timeToStart / stateInfo.length;
	        }
	        else
	        {
				int i = UnityEngine.Random.Range(0, sound.Count);
				AudioClip clip = soundData.getAudioClip(sound[i]);
	            SSoundManager.GetInstance().playSoundFX(clip, loop);          
	            isSoundLaunched = true;
	        }
	    }

	    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	        if (stateInfo.normalizedTime >= timeToStartNormalize && !isSoundLaunched)
	        {
	            isSoundLaunched = true;
				int i = UnityEngine.Random.Range(0, sound.Count);
				AudioClip clip = soundData.getAudioClip(sound[i]);
				SSoundManager.GetInstance().playSoundFX(clip, loop);
	        }
	        else if(stateInfo.normalizedTime > 1.0f && !isAnimationFinish)
	        {
	            isAnimationFinish = true;
	        }
	    }

	    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	        if(stopSound || loop)
			{
				int i = UnityEngine.Random.Range(0, sound.Count);
				AudioClip clip = soundData.getAudioClip(sound[i]);
	            SSoundManager.GetInstance().stopSFX(clip);
			}
	    }
	}
}
