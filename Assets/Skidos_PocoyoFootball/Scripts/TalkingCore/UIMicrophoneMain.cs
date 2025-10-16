using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI.Extensions_Football;

public class UIMicrophoneMain : MonoBehaviour {

    public GameObject buttonMicrophone;
    public GameObject adviceBalloon;
    public Action microphoneFinish;

    private TalkListen talkListener;
    private GameObject blockScreen;

    public void checkButtonMicrophoneShow(ref TalkListen listener, ref GameObject _blockScreen)
    {
        talkListener = listener;
        blockScreen = _blockScreen;
        //buttonMicrophone.GetComponent<UIPositionTween>().PlayTween();
        if (!MicrophonePermission.IsPermitted(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
        {
            if (MicrophonePermission.shouldShowRequestPermissionRationale(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
            {
                Debug.Log("SHOW BUTTON");
                if (buttonMicrophone != null)
                    buttonMicrophone.GetComponent<UIPositionTween>().PlayTween();
            }
            else
                Debug.Log("DONT SHOW BUTTON");

        }
        else
        {
            talkListener.enabled = true;
            SPermanetVariables.isMicrophonePermitted = true;
        }

    }

    public void showRequestPermission()
    {
        Debug.Log("MICROPHONE MAIN");

        Debug.Log("showRequestPermission");
        if (!MicrophonePermission.IsPermitted(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
        {
            Debug.Log("IsPermitted false");

            if (MicrophonePermission.shouldShowRequestPermissionRationale(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
            {
                Debug.Log("shouldShowRequestPermissionRationale true");

                launchPermission();
            }
            else
            {
                Debug.Log("shouldShowRequestPermissionRationale false");
                if(buttonMicrophone !=  null)
                    buttonMicrophone.GetComponent<UIPositionTween>().ReverseTween();
            }
        }
        else
        {
            Debug.Log("IsPermitted true");
            talkListener.enabled = true;
            SPermanetVariables.isMicrophonePermitted = true;
            if (buttonMicrophone != null)
                buttonMicrophone.GetComponent<UIPositionTween>().ReverseTween();
        }
    }

    public void closePocoyoBalloon()
    {
        if(blockScreen != null) blockScreen.SetActive(false);
        if (adviceBalloon != null) adviceBalloon.SetActive(false);
    }

    private void launchPermission()
    {
        Debug.Log("RECORD_AUDIO is launching ...");
        MicrophonePermission.GrantPermission(MicrophonePermission.AndroidPermission.RECORD_AUDIO);
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("------------>MicrophonePermission " + focus + " " + MicrophonePermission.showed);
        if (focus && MicrophonePermission.showed)
        {
            MicrophonePermission.showed = false;
            permissionFinish();
        }
    }

    private void permissionFinish()
    {
        Invoke("showAdvice", 0.1f);        
    }

    private void showAdvice()
    {
        Debug.Log("showAdvice");
        Debug.Log("showRequestPermission");
        if (!MicrophonePermission.IsPermitted(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
        {
            Debug.Log("IsPermitted false");
            if (!MicrophonePermission.shouldShowRequestPermissionRationale(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
            {
                Debug.Log("shouldShowRequestPermissionRationale FALSE");
                if (adviceBalloon != null) adviceBalloon.SetActive(true);
                if (buttonMicrophone != null) buttonMicrophone.GetComponent<UIPositionTween>().ReverseTween();
                if (blockScreen != null) blockScreen.SetActive(true);
            }
            else { Debug.Log("shouldShowRequestPermissionRationale true"); }

        }
        else
        {
            Debug.Log("IsPermitted true");
            talkListener.enabled = true;
            SPermanetVariables.isMicrophonePermitted = true;
            if (buttonMicrophone != null)
                buttonMicrophone.GetComponent<UIPositionTween>().ReverseTween();
        }
    }

    public void showButton()
    {
        if (buttonMicrophone != null)
            buttonMicrophone.GetComponent<UIPositionTween>().PlayTween();
    }

    public void hideButton()
    {
        if (buttonMicrophone != null)
            buttonMicrophone.GetComponent<UIPositionTween>().ReverseTween();
    }

}
