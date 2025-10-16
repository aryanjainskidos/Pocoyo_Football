using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/iTween/UISpriteTween")]
public class UISpriteTween : MonoBehaviour {
	
	public bool AutoStart = false;
	public Sprite[] ImageSpriteList;
	public float Delay;
	public float Duration;
	//public EaseType Ease = EaseType.Linear;
	public LoopType Loop = LoopType.None;
	public bool IgnoreTimeScale = false;


	public UnityEvent OnStart;
	public UnityEvent OnCompleted;
	public UnityEvent OnStartReverse;
	public UnityEvent OnCompletedReverse;

	private Image image;
	private bool AutoLaunched = false;
	private bool Played = false;

	void Reset() {
		if(ImageSpriteList.Length < 1) return;
		image = GetComponent<Image>();
		image.sprite = ImageSpriteList[0];
	}

    void Start() {
        image = GetComponent<Image>();
        image.sprite = ImageSpriteList[0];
	}

	void Update() {
		if(AutoStart && !AutoLaunched)
		{
			AutoLaunched = true;
			PlayTween();
		}
	}

	private void OnDisable()
	{
		iTween.Stop(gameObject);

		AutoLaunched = false;
		Played = false;

        if (image != null)
            image.sprite = ImageSpriteList[0];
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
			iTweenX.from, 0,
			iTweenX.to, (ImageSpriteList.Length),
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(EaseType.Linear),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UISpriteTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UISpriteTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UISpriteTween_oncompletedtween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = true;
	}

	public void ReverseTween() {
		if(!Played) return;
		iTween.ValueTo( gameObject, iTween.Hash(
			iTweenX.from, (ImageSpriteList.Length),
			iTweenX.to, 0,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(EaseType.Linear),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "UISpriteTween_onstartreversetween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "UISpriteTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "UISpriteTween_oncompletedreversetween",
			iTweenX.ignoreTimescale,IgnoreTimeScale
		));
		Played = false;
	}

	void UISpriteTween_onstarttween() {
		OnStart.Invoke();
	}

	void UISpriteTween_onupdatetween(int position) {
		if(position < ImageSpriteList.Length)
			image.sprite = ImageSpriteList[position];
	}

	void UISpriteTween_oncompletedtween() {
		OnCompleted.Invoke();
	}

	void UISpriteTween_onstartreversetween() {
		OnStartReverse.Invoke();
	}

	void UISpriteTween_oncompletedreversetween() {
		OnCompletedReverse.Invoke();
	}
}
