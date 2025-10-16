using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions_Football.Tweens;


namespace UnityEngine.UI.Extensions_Football
{


    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("UI/iTween/UISliderTween")]
    public class UISliderTween : MonoBehaviour
    {

        [Header("UI Slider Tween Values")] public float StartValue;

        [Header("iTween Common Values")] public float Delay;
        public float Duration;
        public EaseType Ease = EaseType.Linear;
        public LoopType Loop = LoopType.None;
        public bool IgnoreTimeScale = false;

        public UnityEvent OnStart;
        public UnityEvent OnCompleted;

        private Slider _slider;

        // Use this for initialization
        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            isPlaying = false;
            StartValue = Mathf.Clamp(StartValue, _slider.minValue, _slider.maxValue);
            _slider.value = StartValue;
        }

        private void OnDisable()
        {
            iTween.Stop(gameObject);
        }

        private bool isPlaying = false;

        public void AddPlayTween(float ammount)
        {
            PlayTween(StartValue + ammount);
        }

        private float _endValue;

        public void PlayTween(float endValue)
        {
            if (isPlaying) return;
            _endValue = Mathf.Clamp(endValue, _slider.minValue, _slider.maxValue);
            iTween.ValueTo(gameObject, iTween.Hash(
                iTweenX.from, StartValue,
                iTweenX.to, endValue,
                iTweenX.delay, Delay,
                iTweenX.time, Duration,
                iTweenX.easeType, iTweenX.Ease(Ease),
                iTweenX.loopType, iTweenX.Loop(Loop),
                iTweenX.onStartTarget, gameObject,
                iTweenX.onStart, "UISliderTween_onstarttween",
                iTweenX.onUpdateTarget, gameObject,
                iTweenX.onUpdate, "UISliderTween_onupdatetween",
                iTweenX.onCompleteTarget, gameObject,
                iTweenX.onComplete, "UISliderTween_oncompletedtween",
                iTweenX.ignoreTimescale, IgnoreTimeScale
            ));
            isPlaying = true;
        }

        void UISliderTween_onstarttween()
        {
            isPlaying = true;
            OnStart.Invoke();
        }

        void UISliderTween_onupdatetween(float value)
        {
            _slider.value = Mathf.Clamp(value, _slider.minValue, _slider.maxValue);
        }

        void UISliderTween_oncompletedtween()
        {
            OnCompleted.Invoke();
            StartValue = _endValue;
            isPlaying = false;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartValue = _slider.maxValue * 0.25f;
            PlayTween(_slider.maxValue * 0.75f);
        }
    }
}
