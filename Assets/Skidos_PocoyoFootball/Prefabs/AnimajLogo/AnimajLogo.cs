using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class AnimajLogo : MonoBehaviour
{
    public UnityEvent AnimationEnded;
    
    VideoPlayer videoPlayer;


    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.Play();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Llama a OnVideoEnd cuando el video termine
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        AnimationEnded.Invoke();
    }
}
