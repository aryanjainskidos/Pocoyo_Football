using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions_Football.Tweens;

namespace UnityEngine.UI.Extensions_Football
{
	
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Audio/iTween/AudioVolumenTween")]
public class AudioVolumenTween : MonoBehaviour {
	public bool AutoStart = false;

	public float From = 0f;
    public float To = 1f;

    public float Delay;
    public float Duration;
    public EaseType Ease = EaseType.Linear;
    public LoopType Loop = LoopType.None;
    public bool IgnoreTimeScale = false;

    public UnityEvent OnStart;
    public UnityEvent OnCompleted;

    public UnityEvent OnReverseStart;
    public UnityEvent OnReverseCompleted;

	private AudioSource target;
	private bool AutoLaunched = false;
	private bool Played = false;

	private void Start()
	{
		target = GetComponent<AudioSource>();
		target.volume = From;
	}

	public void Reset()
	{
		iTween.Stop(gameObject);
		target = GetComponent<AudioSource>();
		target.volume = From;
		AutoLaunched = false;
		Played = false;
	}

	void OnEnable()
	{
		AutoLaunched = false;
		Played = false;
	}

	private void OnDisable()
	{
		iTween.Stop(gameObject);
	}

	void Update()
	{
		if (AutoStart && !AutoLaunched)
		{
			AutoLaunched = true;
			PlayTween();
		}
	}

	public void ToggleTween()
	{
		if (Played)
			ReverseTween();
		else
			PlayTween();
	}

	public void PlayTween(bool force = false)
	{

		if (force) Played = false;
		if (Played) return;
		iTween.ValueTo(gameObject, iTween.Hash(
			iTweenX.from, From,
			iTweenX.to, To,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "AudioVolumenTween_onstarttween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "AudioVolumenTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "AudioVolumenTween_oncompletedtween",
			iTweenX.ignoreTimescale, IgnoreTimeScale
		));
		Played = true;
	}

	public void ReverseTween(bool force = false)
	{

		if (force) Played = true;
		if (!Played) return;
		iTween.ValueTo(gameObject, iTween.Hash(
			iTweenX.from, To,
			iTweenX.to, From,
			iTweenX.delay, Delay,
			iTweenX.time, Duration,
			iTweenX.easeType, iTweenX.Ease(Ease),
			iTweenX.loopType, iTweenX.Loop(Loop),
			iTweenX.onStartTarget, gameObject,
			iTweenX.onStart, "AudioVolumenTween_onstartreversetween",
			iTweenX.onUpdateTarget, gameObject,
			iTweenX.onUpdate, "AudioVolumenTween_onupdatetween",
			iTweenX.onCompleteTarget, gameObject,
			iTweenX.onComplete, "AudioVolumenTween_oncompletedreversetween",
			iTweenX.ignoreTimescale, IgnoreTimeScale
		));
		Played = false;
	}

	void AudioVolumenTween_onstarttween()
	{
		OnStart.Invoke();
	}

	void AudioVolumenTween_onupdatetween(float position)
	{
		target.volume = position;
	}

	void AudioVolumenTween_oncompletedtween()
	{
		OnCompleted.Invoke();
	}

	void AudioVolumenTween_onstartreversetween()
	{
		OnReverseStart.Invoke();
	}

	void AudioVolumenTween_oncompletedreversetween()
	{
		OnReverseCompleted.Invoke();
	}
}

}
