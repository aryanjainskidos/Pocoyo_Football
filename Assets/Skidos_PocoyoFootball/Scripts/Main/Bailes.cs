using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bailes : MonoBehaviour
{
    bool firstTime = true;
    public GameObject Conffetti;

    private void Awake()
    {
        firstTime = true;
    }

    public void SetDance(int idx)
    {
        if(firstTime) { firstTime = false; return; }
        
        AnimationCtrl.instance.SetDance(idx);

        SoundBank_B.instance.StopAll("Celebration");
        SoundBank_B.instance.Play("Celebration", idx);

        int c = Random.Range(0, 100);
        Conffetti.SetActive(c < 25);
            

        //SoundBank_B.instance.StopAll("CelebrationSFX");
        //SoundBank_B.instance.Play("CelebrationSFX");
    }
}
