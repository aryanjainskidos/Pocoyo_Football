using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text;

public class SSaveLoad {
    private static string m_FileName = "/SaveGame.dat";

    public static Action loadFinish;
    public static Action saveFinish;

	public static object pkgInstance = null;

    public static void save()
	{
        if (pkgInstance != null)
        {
            string jSon = JsonUtility.ToJson(pkgInstance);
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(jSon);
            string encodedText = Convert.ToBase64String(bytesToEncode);
            File.WriteAllText(Application.persistentDataPath + SSaveLoad.m_FileName, encodedText);
        }
        else
            Debug.Log("Save obj Failed");
    }

	public static void load(bool callback = true)
	{
        if (File.Exists (Application.persistentDataPath + SSaveLoad.m_FileName)) {
            string encodedText = File.ReadAllText(Application.persistentDataPath + SSaveLoad.m_FileName);
            byte[] decodedBytes = Convert.FromBase64String(encodedText);
            string jSon = Encoding.UTF8.GetString(decodedBytes);
			JsonUtility.FromJsonOverwrite(jSon, pkgInstance);
        }

        if(callback && loadFinish != null)
            loadFinish();
    }
	   
    public static void resetSave()
	{
		string path = Application.persistentDataPath + SSaveLoad.m_FileName;
		if (File.Exists (path)) {
			File.Delete (path);
			pkgInstance = null;
		} 
	}    
}