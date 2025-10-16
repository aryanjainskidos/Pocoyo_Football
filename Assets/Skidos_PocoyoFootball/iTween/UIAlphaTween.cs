using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/iTween/UIAlphaTween")]
public class UIAlphaTween : MonoBehaviour {

    public bool AutoStart = false;

    [ContextMenuItem("Set Current Value", "SetFrom")]
    public float From;
    public void SetFrom() {
        From = GetComponent<CanvasGroup>().alpha;
    }

    [ContextMenuItem("Set Current Value", "SetTo")]
    public float To;
    public void SetTo() {
        To = GetComponent<CanvasGroup>().alpha;
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

    private CanvasGroup canvasGroup;
    private bool AutoLaunched = false;

    void Reset() {
        From = GetComponent<CanvasGroup>().alpha;
        To = GetComponent<CanvasGroup>().alpha;
    }

    public void Stop() {
        iTween.Stop(gameObject);
        AutoLaunched = false;
    }

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = From;
    }

    void OnEnable() {
        AutoLaunched = false;
    }

    private void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    void Update() {
        if (AutoStart && !AutoLaunched) {
            AutoLaunched = true;
            PlayTween();
        }
    }

    public void PlayTween() {
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, From,
            iTweenX.to, To,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "UIAlphaTween_onstarttween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "UIAlphaTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "UIAlphaTween_oncompletedtween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
    }

    public void ReverseTween() {
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, To,
            iTweenX.to, From,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "UIAlphaTween_onstartreversetween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "UIAlphaTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "UIAlphaTween_oncompletedreversetween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
    }

    void UIAlphaTween_onstarttween() {
        if (OnStart != null) {
            OnStart.Invoke();
        }
    }

    void UIAlphaTween_onstartreversetween()
    {
        OnReverseStart.Invoke();
    }

    void UIAlphaTween_onupdatetween(float alpha) {
        canvasGroup.alpha = alpha;
    }

    void UIAlphaTween_oncompletedtween() {
        if (OnCompleted != null) {
            OnCompleted.Invoke();
        }
    }

    void UIAlphaTween_oncompletedreversetween()
    {
        OnReverseCompleted.Invoke();
    }
}