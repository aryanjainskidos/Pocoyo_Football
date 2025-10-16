using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[AddComponentMenu("UI/Zinkia/Slider Lerp")]
public class SliderLerp : MonoBehaviour {

    [Tooltip("If timeLerp is equal 0 use speedLerp")]
    public float timeLerpInSeconds;
    [Tooltip("Use speedLerp if timelerp is equal 0")]
    public float speedLerp;
    public float timeToFinish;

    public System.Action finishLerp = null;

    private float targetValue;
    private Slider ownSlider;
    private bool isEnable = false;
    private bool isTime = true;

    // Use this for initialization
    void Awake () {

        ownSlider = GetComponent<Slider>();
    }

    public void init(float value)
    {
        targetValue = value;

        if (targetValue > 0)
        {
            if (timeLerpInSeconds > 0)
                isTime = true;
            else if (speedLerp > 0)
                isTime = false;
            else
                Debug.LogError("Time and Speed is 0 or less");

            isEnable = true;
        }
        else
            launchAction();
    }

    private void launchAction()
    {
        if (finishLerp != null)
            finishLerp();
    }

    private void Update()
    {
        if (isEnable)
        {
            if (targetValue > ownSlider.value)
            {
                if (isTime)
                    ownSlider.value += ((Time.deltaTime * targetValue) / timeLerpInSeconds);
                else
                    ownSlider.value += (Time.deltaTime * speedLerp);
            }
            else
            {
                isEnable = false;
                Invoke("launchAction", timeToFinish);
            }
        }
    }

}
