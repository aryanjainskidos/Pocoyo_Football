using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RawImage))]
[AddComponentMenu("UI/iTween/UIUVPositionTween")]
public class UIUVPositionTween : MonoBehaviour
{

    public bool AutoStart = false;

    [ContextMenuItem("Set Current Value", "SetFrom")]
    public Vector2 FromXY = Vector2.zero;
    public void SetFrom()
    {
        FromXY = GetComponent<RawImage>().uvRect.position;
    }

    [ContextMenuItem("Set Current Value", "SetTo")]
    public Vector2 ToXY = Vector2.zero;
    public void SetTo()
    {
        ToXY = GetComponent<RawImage>().uvRect.position;
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

    private RawImage rawImage;
    private bool AutoLaunched = false;
    private bool Played = false;
    private bool isPlaying = false;

    void Reset()
    {
        FromXY = GetComponent<RawImage>().uvRect.position;
        ToXY = GetComponent<RawImage>().uvRect.position;
    }

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.uvRect = new Rect( FromXY, rawImage.uvRect.size);
    }

    void OnEnable()
    {
        AutoLaunched = false;
        Played = false;
        GetComponent<RawImage>().uvRect = new Rect(FromXY, GetComponent<RawImage>().uvRect.size);
    }

    private void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    void Update()
    {
        if (AutoStart && !AutoLaunched)
        {
            //rectTransform.localScale = From;
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

    public void ResetPositions()
    {
        AutoLaunched = false;
        Played = false;
        GetComponent<RawImage>().uvRect = new Rect(FromXY, rawImage.uvRect.size);
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

    public void PlayTween(bool reset = false)
    {

        if (reset)
            Played = false;

        if (Played) return;
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, FromXY,
            iTweenX.to, ToXY,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "UIUVPositionTween_onstarttween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "UIUVPositionTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "UIUVPositionTween_oncompletedtween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
        Played = true;
        isPlaying = true;

    }

    public void ReverseTween()
    {
        if (!Played) return;
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, ToXY,
            iTweenX.to, FromXY,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "UIUVPositionTween_onstarttweenreverse",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "UIUVPositionTween_onupdatetweenreverse",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "UIUVPositionTween_oncompletedtweenreverse",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
        Played = false;
        isPlaying = true;

    }

    void UIUVPositionTweenn_onstarttween()
    {
        OnStart.Invoke();
    }

    void UIUVPositionTween_onstarttweenreverse()
    {
        OnStartReverse.Invoke();
    }

    void UIUVPositionTween_onupdatetween(Vector2 position)
    {
        rawImage.uvRect = new Rect(position, rawImage.uvRect.size);
    }

    void UIUVPositionTween_onupdatetweenreverse(Vector2 position)
    {
        rawImage.uvRect = new Rect(position, rawImage.uvRect.size);
    }

    void UIUVPositionTween_oncompletedtween()
    {
        isPlaying = false;
        OnCompleted.Invoke();
    }

    void UIUVPositionTween_oncompletedtweenreverse()
    {
        isPlaying = false;
        OnCompletedReverse.Invoke();
    }

}
