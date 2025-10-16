using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("iTween/ScaleAxisTween")]
public class ScaleAxisTween : MonoBehaviour {

	public bool AutoStart = false;
	public Vector3 From = Vector3.one;
	public Vector3 To = Vector3.one;

	public bool IgnoreTimeScale = false;

	[Header("X Axis Values")]
	public bool UseXAxis = true;
	public float XDelay;
	public float XDuration;
	public EaseType XEase = EaseType.Linear;
	public LoopType XLoop = LoopType.None;
	public UnityEvent OnStartXAxis;
	public UnityEvent OnCompletedXAxis;

	[Header("Y Axis Values")]
	public bool UseYAxis = true;
	public float YDelay;
	public float YDuration;
	public EaseType YEase = EaseType.Linear;
	public LoopType YLoop = LoopType.None;
	public UnityEvent OnStartYAxis;
	public UnityEvent OnCompletedYAxis;

	[Header("Z Axis Values")]
	public bool UseZAxis = true;
	public float ZDelay;
	public float ZDuration;
	public EaseType ZEase = EaseType.Linear;
	public LoopType ZLoop = LoopType.None;
	public UnityEvent OnStartZAxis;
	public UnityEvent OnCompletedZAxis;

	private bool AutoLaunched = false;

	void Start() {		
		transform.localScale = From;
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

	public void PlayTween() {
		if(UseXAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, From.x,
				iTweenX.to, To.x,
				iTweenX.delay, XDelay,
				iTweenX.time, XDuration,
				iTweenX.easeType, iTweenX.Ease(XEase),
				iTweenX.loopType, iTweenX.Loop(XLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartXtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateXtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedXtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}

		if(UseYAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, From.y,
				iTweenX.to, To.y,
				iTweenX.delay, YDelay,
				iTweenX.time, YDuration,
				iTweenX.easeType, iTweenX.Ease(YEase),
				iTweenX.loopType, iTweenX.Loop(YLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartYtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateYtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedYtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}

		if(UseZAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, From.z,
				iTweenX.to, To.z,
				iTweenX.delay, ZDelay,
				iTweenX.time, ZDuration,
				iTweenX.easeType, iTweenX.Ease(ZEase),
				iTweenX.loopType, iTweenX.Loop(ZLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartZtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateZtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedZtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}
	}

	public void ReverseTween() {
		if(UseXAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, To.x,
				iTweenX.to, From.x,
				iTweenX.delay, XDelay,
				iTweenX.time, XDuration,
				iTweenX.easeType, iTweenX.Ease(XEase),
				iTweenX.loopType, iTweenX.Loop(XLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartXtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateXtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedXtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}

		if(UseYAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, To.y,
				iTweenX.to, From.y,
				iTweenX.delay, YDelay,
				iTweenX.time, YDuration,
				iTweenX.easeType, iTweenX.Ease(YEase),
				iTweenX.loopType, iTweenX.Loop(YLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartYtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateYtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedYtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}

		if(UseZAxis) {
			iTween.ValueTo( gameObject, iTween.Hash(
				iTweenX.from, To.z,
				iTweenX.to, From.z,
				iTweenX.delay, ZDelay,
				iTweenX.time, ZDuration,
				iTweenX.easeType, iTweenX.Ease(ZEase),
				iTweenX.loopType, iTweenX.Loop(ZLoop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "ScaleAxisTween_onstartZtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "ScaleAxisTween_onupdateZtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "ScaleAxisTween_oncompletedZtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}
	}

	void ScaleAxisTween_onstartXtween() {
		OnStartXAxis.Invoke();
	}

	void ScaleAxisTween_onupdateXtween(float x) {
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
	}

	void ScaleAxisTween_oncompletedXtween() {
		OnCompletedXAxis.Invoke();
	}

	void ScaleAxisTween_onstartYtween() {
		OnStartYAxis.Invoke();
	}

	void ScaleAxisTween_onupdateYtween(float y) {
		transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
	}

	void ScaleAxisTween_oncompletedYtween() {
		OnCompletedYAxis.Invoke();
	}

	void ScaleAxisTween_onstartZtween() {
		OnStartZAxis.Invoke();
	}

	void ScaleAxisTween_onupdateZtween(float z) {
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
	}

	void ScaleAxisTween_oncompletedZtween() {
		OnCompletedZAxis.Invoke();
	}

}