using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
[AddComponentMenu("UI/iTween/UISliderToTween")]
public class UISliderToTween : MonoBehaviour {
    [Header("iTween Common Values")]
    public float Delay;
    public float Duration;
    public EaseType Ease = EaseType.Linear;
    public LoopType Loop = LoopType.None;
    public bool IgnoreTimeScale = false;

    public UnityEvent OnStart;
    public UnityEvent OnCompleted;

    private Slider _slider;
    private float _currentValue;
    // Use this for initialization
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        isPlaying = false;
        //StartValue = Mathf.Clamp(StartValue, _slider.minValue, _slider.maxValue);
        _currentValue = _slider.value;
    }

    private void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    private bool isPlaying = false;

    private float _endValue;
    public void PlayTween(float endValue)
    {
        if (isPlaying) return;
        _endValue = Mathf.Clamp(endValue, _slider.minValue, _slider.maxValue);
        iTween.ValueTo(gameObject, iTween.Hash(
            iTweenX.from, _currentValue,
            iTweenX.to, endValue,
            iTweenX.delay, Delay,
            iTweenX.time, Duration,
            iTweenX.easeType, iTweenX.Ease(Ease),
            iTweenX.loopType, iTweenX.Loop(Loop),
            iTweenX.onStartTarget, gameObject,
            iTweenX.onStart, "UISliderToTween_onstarttween",
            iTweenX.onUpdateTarget, gameObject,
            iTweenX.onUpdate, "UISliderToTween_onupdatetween",
            iTweenX.onCompleteTarget, gameObject,
            iTweenX.onComplete, "UISliderToTween_oncompletedtween",
            iTweenX.ignoreTimescale, IgnoreTimeScale
        ));
        isPlaying = true;
    }

    void UISliderToTween_onstarttween()
    {
        isPlaying = true;
        OnStart.Invoke();
    }

    void UISliderToTween_onupdatetween(float value)
    {
        _slider.value = Mathf.Clamp(value, _slider.minValue, _slider.maxValue);
    }

    void UISliderToTween_oncompletedtween()
    {
        OnCompleted.Invoke();
        _currentValue = _endValue;
        isPlaying = false;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        _slider = GetComponent<Slider>();
        _currentValue = GetComponent<Slider>().maxValue * 0.25f;
        PlayTween(GetComponent<Slider>().maxValue * 0.75f);
    }
}
