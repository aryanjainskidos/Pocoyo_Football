using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject BlockGameObject;
    public List<GameObject> LockedElements;
    public List<GameObject> UnlockedElements;
    public List<GameObject> LockedVideoElements;
    public List<GameObject> UnlockedVideoElements;

    private void Start()
    {
        BlockGameObject.SetActive(false);
        UnlockMenuItems();
    }

    public void UnlockMenuItems()
    {
        bool unlocked = SGamePackageSave.GetInstance().m_IsGameBought;
        foreach (GameObject go in LockedElements)
            go.SetActive(!unlocked);
        foreach (GameObject go in UnlockedElements)
            go.SetActive(unlocked);
    }

    public void UnlockVideoMenuItems()
    {
        bool unlocked = SPermanetVariables.isVideoRewarded;
        bool unlocked2 = SPermanetVariables.isVideoRewarded2;
        bool unlocked3 = SPermanetVariables.isVideoRewarded3;
        
        LockedVideoElements[0].SetActive(!unlocked);
        LockedVideoElements[1].SetActive(!unlocked2);
        LockedVideoElements[2].SetActive(!unlocked3);

        UnlockedVideoElements[0].SetActive(unlocked);
        UnlockedVideoElements[1].SetActive(unlocked2);
        UnlockedVideoElements[2].SetActive(unlocked3);
    }
}
