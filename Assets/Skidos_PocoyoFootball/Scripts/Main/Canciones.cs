using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Canciones : MonoBehaviour
{
    [System.Serializable]
    public class PlaySongEvent : UnityEvent<int> { }

    public List<Toggle> toggleList;
    public PlaySongEvent onSongSelected;

    public void ToggleChange(bool value)
    {
        if(value)
        {
            for(int i = 0; i < toggleList.Count; i++)
            {
                if (toggleList[i].isOn)
                {
                    onSongSelected.Invoke(i);
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < toggleList.Count; i++)
                if (toggleList[i].isOn) return;

            onSongSelected.Invoke(-1);
        }
    }
}
