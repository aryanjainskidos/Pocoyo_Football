using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/iTween/UIPositionTween")]
public class UIPositionTween : MonoBehaviour {

	public bool AutoStart = false;

	[ContextMenuItem("Set Current Value", "SetFrom")]
	public Vector2 From;
	public void SetFrom() {
		From = GetComponent<RectTransform>().anchoredPosition;
	}

	[ContextMenuItem("Set Current Value", "SetTo")]
	public Vector2 To;
	public void SetTo() {
		To = GetComponent<RectTransform>().anchoredPosition;
	}

	public float Delay;
	public float Duration;
	public EaseType Ease = EaseType.Linear;
	public LoopType Loop = LoopType.None;
	public bool IgnoreTimeScale = false;

	public UnityEvent OnStart;
	public UnityEvent OnCompleted;

	public UnityEvent OnReverseStart;
	public UnityEvent OnReverseCompleted;

	private RectTransform rectTransform;
	private bool AutoLaunched = false;
	private bool Played = false;

	private void Start() {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.anchoredPosition = From;
	}

	public void Reset()
    {
		iTween.Stop(gameObject);
		rectTransform = GetComponent<RectTransform>();
		rectTransform.anchoredPosition = From;
		AutoLaunched = false;
		Played = false;
	}

	void OnEnable() {
		AutoLaunched = false;
		Played = false;
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

	public void ToggleTween() {
		if(Played)
			ReverseTween();
		else
			PlayTween();
	}

	public void PlayTween(bool force = false) {

        if (force) Played = false;
		if(Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, From,
			iTweenX.to, To,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIPositionTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIPositionTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIPositionTween_oncompletedtween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = true;
	}

	public void ReverseTween(bool force = false) {

        if (force) Played = true;
		if(!Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, To,
			iTweenX.to, From,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIPositionTween_onstartreversetween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIPositionTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIPositionTween_oncompletedreversetween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = false;
	}

	public void PlaybackTween(bool force = false)
	{
		if (force) Played = true;
		if (!Played) return;

		rectTransform = GetComponent<RectTransform>();
		Vector2 currentPosition = rectTransform.anchoredPosition;

		iTween.ValueTo(gameObject, iTween.Hash(
			iTweenX.from, currentPosition,
			iTweenX.to, From,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIPositionTween_onstartreversetween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIPositionTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIPositionTween_oncompletedreversetween",
			iTweenX.ignoreTimescale, IgnoreTimeScale
		));
		Played = false;
	}

	void UIPositionTween_onstarttween() {
		OnStart.Invoke();
	}

	void UIPositionTween_onupdatetween(Vector2 position) {
		rectTransform.anchoredPosition = position;
	}

	void UIPositionTween_oncompletedtween() {
		OnCompleted.Invoke();
	}

	void UIPositionTween_onstartreversetween() {
		OnReverseStart.Invoke();
	}

	void UIPositionTween_oncompletedreversetween() {
		OnReverseCompleted.Invoke();
	}
}
