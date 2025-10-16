using System;
using System.Collections;
using UnityEngine;

public class HandCoachAnimator : MonoBehaviour
{
    [Header("General Settings")] private bool useUI = true;
    private float AnimationSpeed = 1;

    private RectTransform rectTransform;

    [Header("Sprite Field")] public RectTransform handImage;
    
    [Header("Private Functionality")] private Coroutine currentAnimation;

    [SerializeField] private int configIndex;


    private void Awake()
    {
        if (useUI)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }
    
    private void OnEnable()
    {
        Debug.Log("HandCoachAnimator OnEnable called");
        
        if (HandCoachManager.Instance == null)
        {
            Debug.LogError("HandCoachManager.Instance is null!");
            return;
        }
        
        HandCoachConfig currentConfig = GetConfigByIndex();
        if (currentConfig == null)
        {
            Debug.LogError("Current config is null!");
            return;
        }
        
        Debug.Log($"Config loaded - screenKey: {currentConfig.screenKey}, animationType: {currentConfig.animationType}");
        
        if (!IsHandCoachShown(currentConfig.screenKey))
        {
            Debug.Log("First time showing hand coach - starting animation");
            PlayFromConfig(currentConfig);
        }
        else
        {
            Debug.Log("Hand coach already shown - hiding");
            transform.parent.gameObject.SetActive(false);
        }
    }

    private static bool IsHandCoachShown(string currentConfigScreenKey)
    {
       return  HandCoachManager.Instance.IsHandCoachComplete(currentConfigScreenKey);
    }

    private HandCoachConfig GetConfigByIndex()
    {
        return HandCoachManager.Instance.GetConfigByIndex(configIndex);
    }


    #region UI_CALLBAKCS

    public void OnButtonPressed()
    {
        SaveHandCoachComplete();
        Debug.Log("Prefab is Saved");
        transform.parent.gameObject.SetActive(false);
       
    }
    

    #endregion

    private void SaveHandCoachComplete()
    {
        HandCoachConfig currentConfig = GetConfigByIndex();
        HandCoachManager.Instance.MakeHandCoachComplete(currentConfig.screenKey);
    }


    private void PlayFromConfig(HandCoachConfig config)
    {
        if (config == null) 
        {
            Debug.LogError("Config is null in PlayFromConfig!");
            return;
        }

        Debug.Log($"Playing animation: {config.animationType}");

        switch (config.animationType)
        {
            case HandCoachConfig.AnimationType.Tap:
                AnimationSpeed = config.tapSpeed;
                Debug.Log($"Starting Tap animation - scale: {config.tapScale}, duration: {config.tapDuration}, speed: {config.tapSpeed}");
                StartNewAnimation(TapAnimation(config.tapScale, config.tapDuration));
                break;

            case HandCoachConfig.AnimationType.Swipe:
                AnimationSpeed = config.swipeSpeed;
                Debug.Log($"Starting Swipe animation - direction: {config.swipeDirection}, duration: {config.swipeDuration}, distance: {config.swipeDistance}");
                StartNewAnimation(SwipeAnimation(config.swipeDirection, config.swipeDuration, config.swipeDistance));
                break;

            case HandCoachConfig.AnimationType.Drag:
                AnimationSpeed = config.dragSpeed;
                Debug.Log($"Starting Drag animation - start: {config.dragStartPosition}, end: {config.dragEndPosition}, duration: {config.dragDuration}");
                StartNewAnimation(DragAnimation(config.dragStartPosition, config.dragEndPosition, config.dragDuration));
                break;

            case HandCoachConfig.AnimationType.Circle:
                AnimationSpeed = config.circleSpeed;
                Debug.Log($"Starting Circle animation - center: {config.circleCenter}, radius: {config.circleRadius}, speed: {config.circleSpeed}");
                StartNewAnimation(CircleAnimation(config.circleCenter, config.circleRadius, config.circleSpeed,
                    config.clockwise));
                break;
        }

    }

    private void StopAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = null;
    }


    private Vector3 Position
    {
        get => handImage.position;
        set => handImage.position = value;
    }



    #region ANIMATION_METHODS_COROUTINES

    private IEnumerator TapAnimation(float ScaleAmount, float Duration)
    {
        Vector3 originalScale = handImage.localScale;
        Vector3 targetScale = originalScale * ScaleAmount;

        while (true)
        {
            float halfDuration = Duration / 2f;
            float t = 0f;

            while (t < halfDuration)
            {
                t += Time.deltaTime * AnimationSpeed;
                handImage.localScale = Vector3.Lerp(originalScale, targetScale, t / halfDuration);
                yield return null;
            }

            t = 0f;
            while (t < halfDuration)
            {
                t += Time.deltaTime * AnimationSpeed;
                handImage.localScale = Vector3.Lerp(targetScale, originalScale, t / halfDuration);
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator SwipeAnimation(Vector3 Direction, float Duration, float Distance)
    {
        Vector3 StartPosition = Position;
        Vector3 EndPosition = StartPosition + Direction.normalized * Distance;

        while (true)
        {
            float t = 0f;
            while (t < Duration)
            {
                t += Time.deltaTime * AnimationSpeed;
                Position = Vector3.Lerp(StartPosition, EndPosition, t / Duration);
                yield return null;
            }

            t = 0f;
            while (t < Duration)
            {
                t += Time.deltaTime * AnimationSpeed;
                Position = Vector3.Lerp(EndPosition, StartPosition, t / Duration);
                yield return null;
            }
        }
    }

    private IEnumerator DragAnimation(Vector3 Start, Vector3 End, float Duration)
    {
        while (true)
        {
            float t = 0f;
            while (t < Duration)
            {
                t += Time.deltaTime * AnimationSpeed;
                Position = Vector3.Lerp(Start, End, t / Duration);
                yield return null;
            }

            t = 0f;
            while (t < Duration)
            {
                t += Time.deltaTime * AnimationSpeed;
                Position = Vector3.Lerp(End, Start, t / Duration);
                yield return null;
            }
        }
    }

    private IEnumerator CircleAnimation(Vector3 center, float radius, float speed, bool clockwise = true)
    {
        float angle = 0f;
        while (true)
        {
            float dir = clockwise ? -1f : 1f;
            angle += dir * speed * Time.deltaTime * AnimationSpeed;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Position = center + new Vector3(x, y, 0f);
            yield return null;
        }
    }


#endregion

    

    private void StartNewAnimation(IEnumerator animationRoutine)
    {
        StopAnimation();
        currentAnimation = StartCoroutine(animationRoutine);
    }
}
