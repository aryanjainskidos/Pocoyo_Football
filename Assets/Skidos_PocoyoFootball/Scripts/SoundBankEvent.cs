using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBankEvent : StateMachineBehaviour
{
    public enum SoundBankType
    {
        SBT_INACTIVIY,
        SBT_HEADTOUCH,
        SBT_TOETOUCH,
        SBT_CHESTTOUCH,
        SBT_HANDTOUCH,
        SBT_BLINT,
        SBT_WELCOME,
    }
    public SoundBankType type;
    public bool useEnter = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (!useEnter) return;

        switch (type)
        {
            case SoundBankType.SBT_INACTIVIY:
                int inactivityIndex = animator.GetInteger("Inactivity");
                SoundBank.instance.Inactivity(inactivityIndex);
                break;
            case SoundBankType.SBT_HEADTOUCH:
                int headIndex = animator.GetInteger("HeadPlay");
                SoundBank.instance.HeadTouch(headIndex);
                break;
            case SoundBankType.SBT_TOETOUCH:
                int toeIndex = animator.GetInteger("ToePlay");
                SoundBank.instance.ToeTouch(toeIndex);
                break;
            case SoundBankType.SBT_CHESTTOUCH:
                int chestIndex = animator.GetInteger("TorsoPlay");
                SoundBank.instance.TorsoTouch(chestIndex);
                break;
            case SoundBankType.SBT_HANDTOUCH:
                SoundBank.instance.HandTouch();
                break;
            case SoundBankType.SBT_BLINT:
                SoundBank.instance.Blink();
                break;
            case SoundBankType.SBT_WELCOME:
                SoundBank.instance.PlayNormal(9);
                break;
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (useEnter) return;

        switch (type)
        {
            case SoundBankType.SBT_INACTIVIY:
                int inactivityIndex = animator.GetInteger("Inactivity");
                SoundBank.instance.Inactivity(inactivityIndex);
                break;
            case SoundBankType.SBT_HEADTOUCH:
                int headIndex = animator.GetInteger("HeadPlay");
                SoundBank.instance.HeadTouch(headIndex);
                break;
            case SoundBankType.SBT_TOETOUCH:
                int toeIndex = animator.GetInteger("ToePlay");
                SoundBank.instance.ToeTouch(toeIndex);
                break;
            case SoundBankType.SBT_CHESTTOUCH:
                int chestIndex = animator.GetInteger("TorsoPlay");
                SoundBank.instance.TorsoTouch(chestIndex);
                break;
            case SoundBankType.SBT_HANDTOUCH:
                SoundBank.instance.HandTouch();
                break;
            case SoundBankType.SBT_BLINT:                
                SoundBank.instance.Blink();
                break;
        }

    }
}
