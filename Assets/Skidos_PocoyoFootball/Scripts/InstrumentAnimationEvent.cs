using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstrumentAnimationEvent : StateMachineBehaviour
{
    public static Animator otherAninator;
    public string AnimationClip;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (otherAninator != null)
            otherAninator.Play(AnimationClip, layerIndex, stateInfo.normalizedTime);
    }
}
