using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEngine.UI.Extensions_Football.Tweens;

namespace UnityEngine.UI.Extensions_Football
{



    [RequireComponent(typeof(Text))]
    [AddComponentMenu("UI/iTween/UINumberTween")]
    [HelpURL("http://blog.stevex.net/string-formatting-in-csharp/")]
    public class UINumberTween : MonoBehaviour
    {

        [Header("UI Number Tween Values")] public float StartValue = 0f;

        public bool ForceInteger = false;

        /*[HelpBox("String.Format Help:\n" +
                " Use Force Integer with:\n" +
                "   Integer\t\t{0:d}\n" +
                "   Hexadecimal\t{0:x4}\n" +
                " Don't Use Force Integer with:\n" +
                "   Scientific\t{0:e}\n" +
                "   Fixed point\t{0:f}\n",
            90f)]*/
        public string StringFormat = string.Empty;

        [Header("iTween Common Values")] public float Delay;
        public float Duration;
        public EaseType Ease = EaseType.Linear;
        public LoopType Loop = LoopType.None;
        public bool IgnoreTimeScale = false;

        public UnityEvent OnStart;
        public UnityEvent OnCompleted;

        private Text _text;

        // Use this for initialization
        void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            isPlaying = false;
            UINumberTween_onupdatetween(StartValue);
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
            _endValue = endValue;
            iTween.ValueTo(gameObject, iTween.Hash(
                iTweenX.from, StartValue,
                iTweenX.to, endValue,
                iTweenX.delay, Delay,
                iTweenX.time, Duration,
                iTweenX.easeType, iTweenX.Ease(Ease),
                iTweenX.loopType, iTweenX.Loop(Loop),
                iTweenX.onStartTarget, gameObject,
                iTweenX.onStart, "UINumberTween_onstarttween",
                iTweenX.onUpdateTarget, gameObject,
                iTweenX.onUpdate, "UINumberTween_onupdatetween",
                iTweenX.onCompleteTarget, gameObject,
                iTweenX.onComplete, "UINumberTween_oncompletedtween",
                iTweenX.ignoreTimescale, IgnoreTimeScale
            ));
            isPlaying = true;
        }

        void UINumberTween_onstarttween()
        {
            isPlaying = true;
            OnStart.Invoke();
        }

        void UINumberTween_onupdatetween(float value)
        {
            if (string.IsNullOrEmpty(StringFormat))
            {
                if (ForceInteger)
                    _text.text = Mathf.FloorToInt(value).ToString();
                else
                    _text.text = value.ToString();
            }
            else
            {
                if (ForceInteger)
                    _text.text = string.Format(StringFormat, Mathf.FloorToInt(value));
                else
                    _text.text = string.Format(StringFormat, value);
            }
        }

        void UINumberTween_oncompletedtween()
        {
            OnCompleted.Invoke();
            StartValue = _endValue;
            isPlaying = false;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            AddPlayTween(100);
        }
    }
}