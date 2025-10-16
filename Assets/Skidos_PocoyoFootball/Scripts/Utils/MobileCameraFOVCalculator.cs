using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCameraFOVCalculator : MonoBehaviour
{
    public float minAspectRatio = (9f / 19.5f);
    public float maxAspectRatio = (9f / 16f);

    public float minAspectRatioFOV = 47f;
    public float maxAspectRatioFOV = 35f;

    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = Mathf.Clamp(((1f * Screen.width) / (1f * Screen.height)), minAspectRatio, maxAspectRatio); //Max aspect ratio = 9:16 Min Aspect
        float aspectDelta = (screenAspect - maxAspectRatio) / (minAspectRatio - maxAspectRatio);
        float calculatedFOV = maxAspectRatioFOV + (aspectDelta * (minAspectRatioFOV - maxAspectRatioFOV));
        Camera.main.fieldOfView = Mathf.Clamp(calculatedFOV, maxAspectRatioFOV, minAspectRatioFOV); ;
    }
}
