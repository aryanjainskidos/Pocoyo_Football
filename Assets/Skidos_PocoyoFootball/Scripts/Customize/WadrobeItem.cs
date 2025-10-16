using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WadrobeItem : MonoBehaviour
{
    public Image element;
    public Image selected;
    public Toggle toggle;

    public Sprite unlocked;
    public Sprite locked;

    public void SetLockState(bool isLocked)
    {
        element.sprite = isLocked ? locked : unlocked;
    }
}
