using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LogoAnimation : MonoBehaviour
{
    public static LogoAnimation instance;

    public UnityEvent AnimationEnded;


    private void Awake()
    {
        instance = this;
    }

    public void LogoAnimationEnded()
    {
        AnimationEnded.Invoke();
    }
}
