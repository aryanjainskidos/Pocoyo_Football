using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/iTween/UIScaleTween")]
public class UIScaleTween : MonoBehaviour {

	public bool AutoStart = false;

	[ContextMenuItem("Set Current Value", "SetFrom")]
	public Vector3 From = Vector3.one;
	public void SetFrom() {
		From = GetComponent<RectTransform>().localScale;
	}

	[ContextMenuItem("Set Current Value", "SetTo")]
	public Vector3 To= Vector3.one;
	public void SetTo() {
		To = GetComponent<RectTransform>().localScale;
	}

	public float Delay;
	public float Duration;
	public EaseType Ease = EaseType.Linear;
	public LoopType Loop = LoopType.None;
	public bool IgnoreTimeScale = false;

	public UnityEvent OnStart;
	public UnityEvent OnCompleted;
    public UnityEvent OnStartReverse;
    public UnityEvent OnCompletedReverse;

    private RectTransform rectTransform;
	private bool AutoLaunched = false;
	private bool Played = false;
    private bool isPlaying = false;

	void Reset() {
		From = GetComponent<RectTransform>().localScale;
		To = GetComponent<RectTransform>().localScale;
	}

	void Start() {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = From;
	}

	void OnEnable() {
		AutoLaunched = false;
		Played = false;
		GetComponent<RectTransform>().localScale = From;
	}

	private void OnDisable()
	{
		iTween.Stop(gameObject);
	}

	void Update() {
		if(AutoStart && !AutoLaunched)
		{
			//rectTransform.localScale = From;
			AutoLaunched = true;
			PlayTween();
		}
	}

	public void ToggleTween() {
		if(Played)
			ReverseTween();
		else
			PlayTween();
	}

    public void ResetPositions()
    {
		AutoLaunched = false;
        Played = false;
        GetComponent<RectTransform>().localScale = From;
    }

	public void SetAsReversed()
    {
		Played = true;
		isPlaying = true;
	}

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void Pause()
    {
        isPlaying = false;
        iTween.Pause(this.gameObject);
    }

    public void Resume()
    {
        isPlaying = true;
        iTween.Resume(this.gameObject);
    }

    public void Stop()
    {
        iTween.Stop(this.gameObject);
    }

    public void PlayTween(bool reset = false) {

        if (reset)
            Played = false;

        if (Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, From,
			iTweenX.to, To,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIScaleTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIScaleTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIScaleTween_oncompletedtween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = true;
        isPlaying = true;

    }

	public void ReverseTween() {
		if(!Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, To,
			iTweenX.to, From,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIScaleTween_onstarttweenreverse",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIScaleTween_onupdatetweenreverse",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIScaleTween_oncompletedtweenreverse",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = false;
        isPlaying = true;

    }

    void UIScaleTween_onstarttween() {
		OnStart.Invoke();
	}

    void UIScaleTween_onstarttweenreverse()
    {
        OnStartReverse.Invoke();
    }

    void UIScaleTween_onupdatetween(Vector3 scale) {
		rectTransform.localScale = scale;
	}

    void UIScaleTween_onupdatetweenreverse(Vector3 scale)
    {
        rectTransform.localScale = scale;
    }

    void UIScaleTween_oncompletedtween() {
        isPlaying = false;
        OnCompleted.Invoke();
	}

    void UIScaleTween_oncompletedtweenreverse()
    {
        isPlaying = false;
        OnCompletedReverse.Invoke();
    }

}
