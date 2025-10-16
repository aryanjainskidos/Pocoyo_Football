using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("iTween/PositionAxisTween")]
public class PositionAxisTween : MonoBehaviour {

	public bool AutoStart = false;
	public Vector3 From;
	public Vector3 To;

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

	private void OnEnable()
	{
		transform.localPosition = From;
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

	public void PlayToggle(bool value)
	{
		if (value)
			PlayTween();
		else
			ReverseTween();
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
				iTweenX.onStart, "PositionAxisTween_onstartXtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateXtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedXtween",
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
				iTweenX.onStart, "PositionAxisTween_onstartYtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateYtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedYtween",
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
				iTweenX.onStart, "PositionAxisTween_onstartZtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateZtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedZtween",
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
				iTweenX.onStart, "PositionAxisTween_onstartXtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateXtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedXtween",
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
				iTweenX.onStart, "PositionAxisTween_onstartYtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateYtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedYtween",
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
				iTweenX.onStart, "PositionAxisTween_onstartZtween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "PositionAxisTween_onupdateZtween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "PositionAxisTween_oncompletedZtween",
				iTweenX.ignoreTimescale,IgnoreTimeScale
			));
		}
	}

	void PositionAxisTween_onstartXtween() {
		OnStartXAxis.Invoke();
	}

	void PositionAxisTween_onupdateXtween(float x) {
		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}

	void PositionAxisTween_oncompletedXtween() {
		OnCompletedXAxis.Invoke();
	}

	void PositionAxisTween_onstartYtween() {
		OnStartYAxis.Invoke();
	}

	void PositionAxisTween_onupdateYtween(float y) {
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
	}

	void PositionAxisTween_oncompletedYtween() {
		OnCompletedYAxis.Invoke();
	}

	void PositionAxisTween_onstartZtween() {
		OnStartZAxis.Invoke();
	}

	void PositionAxisTween_onupdateZtween(float z) {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}

	void PositionAxisTween_oncompletedZtween() {
		OnCompletedZAxis.Invoke();
	}

}
