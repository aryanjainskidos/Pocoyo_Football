using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WebCamBackground : MonoBehaviour
{
    public RawImage rawimage;
    public AspectRatioFitter imageFitter;

    private WebCamTexture webCamTexture;
    private WebCamDevice[] cam_devices;
    private int notFrontCameraIndex = 0;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Image uvRect
    Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
    Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

    void Start()
    {
        SetupCameras();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    void SetupCameras()
    {
        cam_devices = WebCamTexture.devices;
        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < cam_devices.Length; i++)
        {
            print("Webcam available: " + cam_devices[i].name);
            if (!cam_devices[i].isFrontFacing)
            {
                notFrontCameraIndex = i;
                webCamTexture = new WebCamTexture(cam_devices[notFrontCameraIndex].name, 1920, 1080, 30);
                rawimage.texture = webCamTexture;
                rawimage.material.mainTexture = webCamTexture;
                break;
            }
        }
    }

    public void PlayWebCam()
    {
        if(webCamTexture == null)
        {
            SetupCameras();
        }

        if (webCamTexture != null)
        {
            webCamTexture.Play();
            Debug.Log("Web Cam Connected : " + webCamTexture.deviceName + "\n");
        }
    }

    public void StopWebCam()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            Debug.Log("Web Cam Disconnected : " + webCamTexture.deviceName + "\n");
        }
    }

    // Make adjustments to image every frame to be safe, since Unity isn't 
    // guaranteed to report correct data as soon as device camera is started
    void Update()
    {
        if (webCamTexture == null) return;

        // Skip making adjustment for incorrect camera data
        if (webCamTexture.width < 100)
        {
            Debug.Log("Still waiting another frame for correct info...");
            return;
        }

        // Rotate image to show correct orientation 
        rotationVector.z = -webCamTexture.videoRotationAngle;
        rawimage.rectTransform.localEulerAngles = rotationVector;

        // Set AspectRatioFitter's ratio
        float videoRatio = (float)webCamTexture.width / (float)webCamTexture.height;
        imageFitter.aspectRatio = videoRatio;

        // Unflip if vertically flipped
        rawimage.uvRect = webCamTexture.videoVerticallyMirrored ? fixedRect : defaultRect;
    }
}
