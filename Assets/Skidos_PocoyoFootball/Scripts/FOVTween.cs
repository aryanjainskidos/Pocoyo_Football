using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class FOVTween : MonoBehaviour
{
    public bool AutoStart = false;

    [ContextMenuItem("Set Current Value", "SetFrom")]
    public float offset;
    public void SetFrom()
    {
        offset = GetComponent<Camera>().fieldOfView;
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

    private Camera _camera;
    private bool AutoLaunched = false;
    private float From;
    private float To;


    void Reset()
    {
        offset = GetComponent<Camera>().fieldOfView;
    }

    public void Stop()
    {
        iTween.Stop(gameObject);
        AutoLaunched = false;
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        AutoLaunched = false;
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

    public void PlayTween()
    {
        From = _camera.fieldOfView;
        To = From + offset;

        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, From,
            iTweenX.to, To,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "FOVTween_onstarttween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "FOVTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "FOVTween_oncompletedtween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
    }

    public void ReverseTween()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, To,
            iTweenX.to, From,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "FOVTween_onstartreversetween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "FOVTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "FOVTween_oncompletedreversetween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
    }

    void FOVTween_onstarttween()
    {
        if (OnStart != null)
        {
            OnStart.Invoke();
        }
    }

    void FOVTween_onstartreversetween()
    {
        OnReverseStart.Invoke();
    }

    void FOVTween_onupdatetween(float fov)
    {
        _camera.fieldOfView = fov;
    }

    void FOVTween_oncompletedtween()
    {
        if (OnCompleted != null)
        {
            OnCompleted.Invoke();
        }
    }

    void FOVTween_oncompletedreversetween()
    {
        OnReverseCompleted.Invoke();
    }
}
