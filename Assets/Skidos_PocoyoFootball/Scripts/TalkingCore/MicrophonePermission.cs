using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Android;

public class MicrophonePermission : MonoBehaviour {

    // subscribe to this callback to see if your permission was granted.
    public static bool showed = false;

    // for now, it only implements the external storage permission
    public enum AndroidPermission
    {
		RECORD_AUDIO	
    }

	public static bool IsPermitted(AndroidPermission permission){
        return Permission.HasUserAuthorizedPermission(Permission.Microphone);
    }
	
	public static bool shouldShowRequestPermissionRationale(AndroidPermission permission) 
    {
        return true;
    }

    public static void GrantPermission(AndroidPermission permission) 
    {
        showed = true;
        Permission.RequestUserPermission(Permission.Microphone);
    }
}
