using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions_Football;

[RequireComponent(typeof(Transform))]
public class ShakeTween : MonoBehaviour {

    [System.Serializable]
    public struct positionTween
    {
        public bool isActive;
        public Vector3 Ammount;
        public float Delay;
        public float Duration;
        public bool IgnoreTimeScale;
        public UnityEvent OnStart;
        public UnityEvent OnCompleted;
    };

    [System.Serializable]
    public struct rotationTween
    {
        public bool isActive;
        public Vector3 Ammount;
        public float Delay;
        public float Duration;
        public bool IgnoreTimeScale;
        public UnityEvent OnStart;
        public UnityEvent OnCompleted;
    };

    [System.Serializable]
    public struct scaleTween
    {
        public bool isActive;
        public Vector3 Ammount;
        public float Delay;
        public float Duration;
        public bool IgnoreTimeScale;
        public UnityEvent OnStart;
        public UnityEvent OnCompleted;
    };

    public bool AutoStart = false;

    public positionTween position;
    public rotationTween rotation;
    public scaleTween scale;

	private RectTransform rectTransform;
	private bool AutoLaunched = false;

    void OnEnable()
    {
        AutoLaunched = false;
    }

    private void OnDisable()
    {
        iTween.Stop(gameObject);
    }


    void Update() {
		if(AutoStart && !AutoLaunched)
		{
			AutoLaunched = true;
			PlayTween();
		}
	}

    public void StopTween()
    {
        iTween.Stop(gameObject);
    }

	public void PlayTween() {

        if (position.isActive)
        {
            //iTween.ShakePosition(gameObject, position.Ammount, position.Duration);

            iTween.ShakePosition(gameObject, iTween.Hash(
            iTweenX.amount, position.Ammount,
            iTweenX.delay, position.Delay,
            iTweenX.time, position.Duration,
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "onStartPositionTween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "onCompletePositionTween",
            iTweenX.ignoreTimescale, position.IgnoreTimeScale
            ));
        }

        if (rotation.isActive)
        {
            //iTween.ShakePosition(gameObject, position.Ammount, position.Duration);

            iTween.ShakeRotation(gameObject, iTween.Hash(
            iTweenX.amount, rotation.Ammount,
            iTweenX.delay, rotation.Delay,
            iTweenX.time, rotation.Duration,
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "onStartRotationTween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "onCompleteRotationTween",
            iTweenX.ignoreTimescale, rotation.IgnoreTimeScale
            ));
        }

        if (scale.isActive)
        {
           // iTween.ShakePosition(gameObject, position.Ammount, position.Duration);

            iTween.ShakeScale(gameObject, iTween.Hash(
            iTweenX.amount, scale.Ammount,
            iTweenX.delay, scale.Delay,
            iTweenX.time, scale.Duration,
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "onStartScaleTween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "onCompleteScaleTween",
            iTweenX.ignoreTimescale, scale.IgnoreTimeScale
            ));
        }

    }

	void onStartPositionTween()
    {		
        position.OnStart.Invoke();
	}

	void onCompletePositionTween() {
        position.OnCompleted.Invoke();
	}

    void onStartRotationTween()
    {
        rotation.OnStart.Invoke();
    }

    void onCompleteRotationTween()
    {
        rotation.OnCompleted.Invoke();
    }

    void onStartScaleTween()
    {
        scale.OnStart.Invoke();
    }

    void onCompleteScaleTween()
    {
        scale.OnCompleted.Invoke();
    }

}
