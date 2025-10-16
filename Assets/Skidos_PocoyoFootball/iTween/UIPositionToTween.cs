using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions_Football.Tweens;


namespace UnityEngine.UI.Extensions_Football
{




	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/iTween/UIPositionToTween")]
	public class UIPositionToTween : MonoBehaviour
	{

		public bool AutoStart = false;

		[ContextMenuItem("Set Current Value", "SetTo")]
		public Vector2 To;

		public void SetTo()
		{
			To = GetComponent<RectTransform>().anchoredPosition;
		}

		public float Delay;
		public float Duration;
		public EaseType Ease = EaseType.Linear;
		public LoopType Loop = LoopType.None;
		public bool IgnoreTimeScale = false;

		public UnityEvent OnStart;
		public UnityEvent OnCompleted;

		private Vector2 From;
		private RectTransform rectTransform;
		private bool AutoLaunched = false;
		private bool Played = false;

		void Reset()
		{
			From = GetComponent<RectTransform>().anchoredPosition;
			To = GetComponent<RectTransform>().anchoredPosition;
		}

		void Start()
		{
			rectTransform = GetComponent<RectTransform>();
			rectTransform.anchoredPosition = From;
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

		public void PlayTween(bool force = false)
		{
			if (force) Played = false;
			if (Played) return;

			From = GetComponent<RectTransform>().anchoredPosition;

			iTween.ValueTo(gameObject, iTween.Hash(
				iTweenX.from, From,
				iTweenX.to, To,
				iTweenX.delay, Delay,
				iTweenX.time, Duration,
				iTweenX.easeType, iTweenX.Ease(Ease),
				iTweenX.loopType, iTweenX.Loop(Loop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "UIPositionToTween_onstarttween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "UIPositionToTween_onupdatetween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "UIPositionToTween_oncompletedtween",
				iTweenX.ignoreTimescale, IgnoreTimeScale
			));
			Played = true;
		}

		void UIPositionToTween_onstarttween()
		{
			OnStart.Invoke();
		}

		void UIPositionToTween_onupdatetween(Vector2 position)
		{
			rectTransform.anchoredPosition = position;
		}

		void UIPositionToTween_oncompletedtween()
		{
			OnCompleted.Invoke();
		}
	}

}