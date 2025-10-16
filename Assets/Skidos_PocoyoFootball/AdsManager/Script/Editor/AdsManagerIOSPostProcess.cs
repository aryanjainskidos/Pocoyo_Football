#if UNITY_IOS && UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using UnityEditor.iOS.Xcode;

public class AdsManagerIOSPostProcess
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string path)
    {

        if (buildTarget == BuildTarget.iOS)
        {

            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;

            Debug.Log(">> Automation, plist ... <<");

            //Delete execute on background
            rootDict.values.Remove("UIApplicationExitsOnSuspend");

            // example of changing a value:
            rootDict.SetString("GADApplicationIdentifier", Admob.AppId_iOS);
            rootDict.SetString("NSCalendarsUsageDescription","Calendar used for purposes of daily reward");
            rootDict.SetString("NSPhotoLibraryUsageDescription" ,"Photo library used for purposes of user experience");
            rootDict.SetString("NSLocationWhenInUseUsageDescription" ,"Location used for purposes of leaderboard");
            rootDict.SetString("NSMicrophoneUsageDescription" ,"Microphone used to talk with Pocoyo");

            // example of adding a boolean key...
            // < key > ITSAppUsesNonExemptEncryption </ key > < false />
            //rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            // Check if SKAdNetworkItems already exists
            PlistElementArray SKAdNetworkItems = null;
            if (rootDict.values.ContainsKey("SKAdNetworkItems"))
            {
                SKAdNetworkItems = rootDict.values["SKAdNetworkItems"] as PlistElementArray;
            }
            else
            {
                SKAdNetworkItems = rootDict.CreateArray("SKAdNetworkItems");
            }

            // string plistContent = File.ReadAllText(plistPath);
            // if (!plistContent.Contains(IronSourceConstants.IRONSOURCE_SKAN_ID_KEY))
            // {
            //     PlistElementDict SKAdNetworkIdentifierDict = SKAdNetworkItems.AddDict();
            //     SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", IronSourceConstants.IRONSOURCE_SKAN_ID_KEY); //IronSource
                /* NOT TESTED YET
                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", "4pfyvq9l8r.skadnetwork"); //AdColony
                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork"); //AdMob
                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", "ludvb6z3bs.skadnetwork"); //AppLovin
                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork"); //UnityAds
                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", "gta9lk7p23.skadnetwork"); //Vungle
                */
            // }
            // File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif
