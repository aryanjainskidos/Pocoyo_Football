using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions_Football.Tweens;



namespace UnityEngine.UI.Extensions_Football
{



	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/iTween/UIRotationTween")]
	public class UIRotationTween : MonoBehaviour
	{

		public bool AutoStart = false;
		public bool UseLocalRotation = false;

		[ContextMenuItem("Set Current Value", "SetFrom")]
		public Vector3 From;

		public void SetFrom()
		{
			if (UseLocalRotation)
				From = GetComponent<RectTransform>().localRotation.eulerAngles;
			else
				From = GetComponent<RectTransform>().rotation.eulerAngles;
		}

		[ContextMenuItem("Set Current Value", "SetTo")]
		public Vector3 To;

		public void SetTo()
		{
			if (UseLocalRotation)
				To = GetComponent<RectTransform>().localRotation.eulerAngles;
			else
				To = GetComponent<RectTransform>().rotation.eulerAngles;
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

		private RectTransform rectTransform;
		private bool AutoLaunched = false;
		private bool Played = false;

		void Reset()
		{
			if (UseLocalRotation)
			{
				From = GetComponent<RectTransform>().localRotation.eulerAngles;
				To = GetComponent<RectTransform>().localRotation.eulerAngles;
			}
			else
			{
				From = GetComponent<RectTransform>().rotation.eulerAngles;
				To = GetComponent<RectTransform>().rotation.eulerAngles;
			}
		}

		void Start()
		{
			rectTransform = GetComponent<RectTransform>();
			if (UseLocalRotation)
				rectTransform.localRotation = Quaternion.Euler(From);
			else
				rectTransform.rotation = Quaternion.Euler(From);
		}

		private void OnDisable()
		{
			iTween.Stop(gameObject);
			AutoLaunched = false;
			Played = false;
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
				iTweenX.onStart, "UIRotationTween_onstarttween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "UIRotationTween_onupdatetween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "UIRotationTween_oncompletedtween",
				iTweenX.ignoreTimescale, IgnoreTimeScale
			));
			Played = true;
		}

		public void ReverseTween()
		{
			if (!Played) return;
			iTween.ValueTo(gameObject, iTween.Hash(
				iTweenX.from, To,
				iTweenX.to, From,
				iTweenX.delay, Delay,
				iTweenX.time, Duration,
				iTweenX.easeType, iTweenX.Ease(Ease),
				iTweenX.loopType, iTweenX.Loop(Loop),
				iTweenX.onStartTarget, gameObject,
				iTweenX.onStart, "UIRotationTween_onstartreversetween",
				iTweenX.onUpdateTarget, gameObject,
				iTweenX.onUpdate, "UIRotationTween_onupdatetween",
				iTweenX.onCompleteTarget, gameObject,
				iTweenX.onComplete, "UIRotationTween_oncompletedreversetween",
				iTweenX.ignoreTimescale, IgnoreTimeScale
			));
			Played = false;
		}

		public void stop()
		{
			iTween.Stop(this.gameObject);
		}

		void UIRotationTween_onstarttween()
		{
			OnStart.Invoke();
		}

		void UIRotationTween_onupdatetween(Vector3 eulerAngles)
		{
			if (UseLocalRotation)
				rectTransform.localRotation = Quaternion.Euler(eulerAngles);
			else
				rectTransform.rotation = Quaternion.Euler(eulerAngles);
		}

		void UIRotationTween_oncompletedtween()
		{
			OnCompleted.Invoke();
		}

		void UIRotationTween_onstartreversetween()
		{
			OnStartReverse.Invoke();
		}

		void UIRotationTween_oncompletedreversetween()
		{
			OnCompletedReverse.Invoke();
		}

	}

}