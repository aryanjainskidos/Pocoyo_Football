using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMicrophoneIntro : MonoBehaviour {

    public GameObject pocoyoBalloon;
    public Action microphoneFinish;

    public void showMicrophoneDialog()
    {
        checkPermissions();
    }

    public void closePocoyoBalloon()
    {
        if(pocoyoBalloon != null)
            pocoyoBalloon.SetActive(false);
        launchPermission();
    }

    private void checkPermissions()
    {
        if (MicrophonePermission.IsPermitted(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
        {
            Debug.Log("RECORD_AUDIO is already permitted!!");
            SPermanetVariables.isMicrophonePermitted = true;
            permissionFinish();
        }
        else
        {
            SPermanetVariables.isMicrophonePermitted = false;
            //aqui cargamos la partida para ver si el microfono esta cancelado y no querermos que funcione
            SSaveLoad.load(false);

            Debug.Log("MicrophoneShow" + SGamePackageSave.GetInstance().m_IsMicrophoneShow);

            if (SGamePackageSave.GetInstance().m_IsMicrophoneShow)
            {
                if (MicrophonePermission.shouldShowRequestPermissionRationale(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
                {
                    if (pocoyoBalloon != null)
                        pocoyoBalloon.SetActive(true);
                    else
                        launchPermission();
                }
                else
                    permissionFinish();
            }
            else
            {
                SPermanetVariables.isMicrophoneShow = true;
                if(pocoyoBalloon != null)
                    pocoyoBalloon.SetActive(true);
                else
                    launchPermission();
            }

        }
    }

    private void launchPermission()
    {
        Debug.Log("RECORD_AUDIO is launching ...");
        MicrophonePermission.GrantPermission(MicrophonePermission.AndroidPermission.RECORD_AUDIO);
    }

    private void permissionFinish()
    {
        if (microphoneFinish != null)
        {
            Debug.Log("-------------------------------->permissionFinish");
            microphoneFinish();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("------------>MicrophonePermission " + focus + " " + MicrophonePermission.showed);
        if (focus && MicrophonePermission.showed)
        {
            MicrophonePermission.showed = false;

            if (MicrophonePermission.IsPermitted(MicrophonePermission.AndroidPermission.RECORD_AUDIO))
            {
                Debug.Log("RECORD_AUDIO is permitted!!");
                SPermanetVariables.isMicrophonePermitted = true;
            }

            permissionFinish();
        }
    }

}
