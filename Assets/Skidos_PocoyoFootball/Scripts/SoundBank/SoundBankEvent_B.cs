using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBankEvent_B : StateMachineBehaviour
{
    public bool useEnter = false;
    public string BankType;
    public int index;
    public string UsingIntParameter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (useEnter) PlaySoundBank(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (!useEnter) PlaySoundBank(animator);
    }

    void PlaySoundBank(Animator animator)
    {
        int idx = index;
        if (!string.IsNullOrEmpty(UsingIntParameter))
            idx += animator.GetInteger(UsingIntParameter);

        SoundBank_B.instance.Play(BankType, idx);
    }
}
