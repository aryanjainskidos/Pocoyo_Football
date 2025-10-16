using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/iTween/UITransformToObjectTween")]
public class UITransformToObjectTween : MonoBehaviour {

	public bool AutoStart = false;

	public RectTransform Target;
	//public bool UseWorldTransform = false;

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
	private Vector2 OriginalPosition;
	private Quaternion OriginalRotation;
	private Vector3 OriginalScale;
	private bool AutoLaunched = false;
	private bool Played = false;

	void Awake() {
		rectTransform = GetComponent<RectTransform>();
		OriginalPosition = rectTransform.position;
		OriginalRotation = rectTransform.rotation;
	}

	void OnEnable() {
		AutoLaunched = false;
		Played = false;
		rectTransform.position = OriginalPosition;
		rectTransform.rotation = OriginalRotation;
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

	public void PlayTween() {
		if(Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, 0f,
			iTweenX.to, 1f,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UITransformToObjectTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UITransformToObjectTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UITransformToObjectTween_oncompletedtween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = true;
	}

	public void ReverseTween() {
		if(!Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, 0f,
			iTweenX.to, 1f,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UITransformToObjectTween_onstartreversetween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UITransformToObjectTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UITransformToObjectTween_oncompletedreversetween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = false;
	}

	void UITransformToObjectTween_onstarttween() {
		OnStart.Invoke();
	}

	void UITransformToObjectTween_onupdatetween(float value) {
		rectTransform.position = Vector2.Lerp(OriginalPosition, Target.position, value);
		rectTransform.rotation = Quaternion.Lerp(OriginalRotation, Target.rotation, value);
	}

	void UITransformToObjectTween_oncompletedtween() {
		OnCompleted.Invoke();
	}

	void UITransformToObjectTween_onstartreversetween() {
		OnReverseStart.Invoke();
	}

	void UITransformToObjectTween_oncompletedreversetween() {
		OnReverseCompleted.Invoke();
	}
}
