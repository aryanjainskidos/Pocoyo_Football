using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Graphic))]
[AddComponentMenu("UI/iTween/UIColorTween")]
public class UIColorTween : MonoBehaviour {

	public bool AutoStart = false;
	public Color From;
	public Color To;
	public float Delay;
	public float Duration;
	public EaseType Ease = EaseType.Linear;
	public LoopType Loop = LoopType.None;
	public bool IgnoreTimeScale = false;

	public UnityEvent OnStart;
	public UnityEvent OnCompleted;
    public UnityEvent OnStartReverse;
    public UnityEvent OnCompletedReverse;

    private Graphic image;
	private bool AutoLaunched = false;
	private bool Played = false;

	void Reset() {
		From = GetComponent<Graphic>().color;
		To = GetComponent<Graphic>().color;
	}

	void Start() {
		image = GetComponent<Graphic>();
		image.color = From;
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

	public void PlayTween() {
		if(Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, From,
			iTweenX.to, To,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UIColorTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIColorTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIColorTween_oncompletedtween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = true;
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
			iTweenX.onStart, "UIColorTween_onstarttweenreverse",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UIColorTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UIColorTween_oncompletedtweenreverse",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = false;
	}

	void UIColorTween_onstarttween() {
		OnStart.Invoke();
	}

    void UIColorTween_onstarttweenreverse()
    {
        OnStartReverse.Invoke();
    }

    void UIColorTween_onupdatetween(Color color) {
		image.color = color;
	}

	void UIColorTween_oncompletedtween() {
		OnCompleted.Invoke();
	}

    void UIColorTween_oncompletedtweenreverse()
    {
        OnCompletedReverse.Invoke();
    }

}
