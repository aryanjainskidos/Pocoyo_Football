using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnimationCtrl : MonoBehaviour
{
    public static AnimationCtrl instance;

    public Animator PocoyoAnimator;
    public Vector2 BlinkTime = new Vector2(1.5f, 1.75f);
    public Vector2 InactivityTime = new Vector2(15f, 20f);
    public ShakeTween cameraShake;
    [Header("Instrument Animators")]
    public List<Animator> Instruments;
    [Header("Animator Controllers")]
    public List<RuntimeAnimatorController> animators;
    [Header("SoccerBallAnimator")]
    public Animator SoccerBall;
    public List<RuntimeAnimatorController> BallAnimators;
    [Header("TouchEvnet Controler")]
    public PhysicsRaycaster raycaster;
    [Header("UnityEvents")]
    public UnityEvent uiChanged;
    public UnityEvent AnimalPlayEnded;

    int lastInstrument = -1;
    Main.UIModes _uiMode = Main.UIModes.UIMODE_DEFAULT;
    Main.UIModes _lastUIMode = Main.UIModes.UIMODE_DEFAULT;
    bool welcome = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Animator ani in Instruments)
            ani.gameObject.SetActive(false);

        BallAnimationEvent.otherAninator = SoccerBall;

        SetDefaultAnimator();
    }

    public void SetUIMode(Main.UIModes uiMode)
    {
        if (uiMode == _lastUIMode) return;

        switch (_lastUIMode)
        {
            case Main.UIModes.UIMODE_DEFAULT:
                lastInstrument = -1;
                break;
            case Main.UIModes.UIMODE_VUVUCELA:
                lastInstrument = 0;
                SueltaBubucela();
                break;
            case Main.UIModes.UIMODE_BOMBO:
                lastInstrument = 1;
                SueltaBombo();
                break;
            case Main.UIModes.UIMODE_SILBATO:
                lastInstrument = 2;
                SueltaSilvato();
                break;
            case Main.UIModes.UIMODE_JEMBE:
                lastInstrument = 3;
                SueltaJembe();
                break;
            case Main.UIModes.UIMODE_BOCINA:
                lastInstrument = 4;
                SueltaBocina();
                break;
            case Main.UIModes.UIMODE_GAME:
                lastInstrument = -1;
                break;
            case Main.UIModes.UIMODE_BAILE:
                lastInstrument = -1;
                break;
            case Main.UIModes.UIMODE_CLOTHES:
                lastInstrument = 0;
                ExitDressAnimator();
                break;
            //case Main.UIModes.UIMODE_AR:
            //    lastInstrument = -1;
            //    break;
        }

        _lastUIMode = uiMode;
        _uiMode = uiMode;

        if (lastInstrument == -1)
            ChangeAnimationControler();
    }

    #region DEFAULT
    public void SetDefaultAnimator()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[(int)Main.UIModes.UIMODE_DEFAULT];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[(int)Main.UIModes.UIMODE_DEFAULT];

        raycaster.enabled = true;

        if (welcome)
        {
            welcome = false;

            AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
            if (aee != null)
                aee.EnterAnimation.AddListener(PlayDefaultEnded);

            PocoyoAnimator.SetTrigger("Welcome");

            TalkListen.instance.DisableTalking();
        }
        else
        {
            TalkListen.instance.EnableTalking();

            Invoke("PlayBlink", Random.Range(BlinkTime.x, BlinkTime.y));
            Invoke("PlayInactivity", Random.Range(InactivityTime.x, InactivityTime.y));
        }
    }

    public void PlayHeadTouch()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayDefaultEnded);

        PocoyoAnimator.SetTrigger("LaunchHeadPlay");
        
        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void PlayToeTouch()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayDefaultEnded);

        PocoyoAnimator.SetTrigger("LaunchToePlay");
        
        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void PlayHandTouch()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayDefaultEnded);

        PocoyoAnimator.SetTrigger("LaunchHandPlay");

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void PlayChestTouch()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayDefaultEnded);

        PocoyoAnimator.SetTrigger("LaunchTorsoPlay");

        Invoke("PlayChestCameraShake", 2f);

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void PlayInactivity()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayDefaultEnded);

        PocoyoAnimator.SetTrigger("LaunchInactivity");

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void PlayBlink()
    {
        PocoyoAnimator.SetTrigger("Blink");
        Invoke("PlayBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void PlayChestCameraShake()
    {
        int torsoPlayIdx = PocoyoAnimator.GetInteger("TorsoPlay");

        if(torsoPlayIdx == 2)
            cameraShake.PlayTween();
    }

    public void PlayDefaultEnded()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.RemoveListener(PlayDefaultEnded);

        //Enable listening
        TalkListen.instance.EnableTalking();

        //Enable touch events
        raycaster.enabled = true;

        Invoke("PlayBlink", Random.Range(BlinkTime.x, BlinkTime.y));
        Invoke("PlayInactivity", Random.Range(InactivityTime.x, InactivityTime.y));
    }

    public void StartListen()
    {
        PocoyoAnimator.SetBool("Listen", true);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        raycaster.enabled = false;
    }

    public void EndListen()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(ListenAnimEnded);

        PocoyoAnimator.SetBool("Listen", false);
    }

    void ListenAnimEnded()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.RemoveListener(ListenAnimEnded);

        TalkListen.instance.Speak();

        raycaster.enabled = true;
    }

    public void StartRepeat()
    {
        PocoyoAnimator.SetBool("Repeat", true);

        raycaster.enabled = false;
    }

    public void EndRepeat()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(RepeatAnimEnded);

        PocoyoAnimator.SetBool("Repeat", false);
    }

    void RepeatAnimEnded()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.RemoveListener(RepeatAnimEnded);

        TalkListen.instance.EnableTalking();

        Invoke("PlayBlink", Random.Range(BlinkTime.x, BlinkTime.y));
        Invoke("PlayInactivity", Random.Range(InactivityTime.x, InactivityTime.y));

        raycaster.enabled = true;
    }

    public void NotInMainScreen()
    {
        if(_uiMode == Main.UIModes.UIMODE_DEFAULT)
        {
            CancelInvoke("PlayBlink");
            CancelInvoke("PlayInactivity");
        }
    }

    public void BackToMainScreen()
    {
        if(_uiMode == Main.UIModes.UIMODE_DEFAULT)
        {
            Invoke("PlayBlink", Random.Range(BlinkTime.x, BlinkTime.y));
            Invoke("PlayInactivity", Random.Range(InactivityTime.x, InactivityTime.y));
        }
    }

    #endregion

    #region ANIMALS
    public void SetBallGameController()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[(int)Main.UIModes.UIMODE_GAME];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[(int)Main.UIModes.UIMODE_GAME];

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");
    }

    public void SetBallTouch(int idx)
    {
        PocoyoAnimator.SetInteger("ball_position", idx);
    }

    public void PlayBallGameResult(bool succes)
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.AddListener(PlayAnimalEnded);

        PocoyoAnimator.SetTrigger(succes ? "play" : "fail");
    }

    void PlayAnimalEnded()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if (aee != null)
            aee.EnterAnimation.RemoveListener(PlayAnimalEnded);

        AnimalPlayEnded.Invoke();
    }
    #endregion

    #region DANCE
    public void SetDanceController()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[(int)Main.UIModes.UIMODE_BAILE];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[(int)Main.UIModes.UIMODE_BAILE];

        SetDance(0);

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");
    }

    public void SetDance(int idx)
    {
        PocoyoAnimator.SetInteger("Dance", idx);
        PocoyoAnimator.SetTrigger("Launch");
    }
    #endregion

    #region INSTRUMENTOS
    public void PlayInstrument(int mode)
    {        
        PocoyoAnimator.SetInteger("Play", mode);
        PocoyoAnimator.SetTrigger("launchPlay");
    }

    public void StopInstrument()
    {
        //PocoyoAnimator.SetTrigger("stopPlay");
    }

    public void SuffocatedInstrument()
    {
        PocoyoAnimator.SetTrigger("suffocated");
    }

    //Piano
    public void CogeBubucela()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[1];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[1];
        Instruments[0].gameObject.SetActive(true);
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        InstrumentAnimationEvent.otherAninator = Instruments[0];

        TalkListen.instance.DisableTalking();

        //SoundBank.instance.PlayEnterInstrument(0);
        SoundBank_B.instance.Play("InstrumentEnter", 0);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void SueltaBubucela()
    {
        PocoyoAnimator.SetTrigger("Exit");
        //Instruments[0].SetTrigger("Exit");

        //SoundBank.instance.PlayExitInstrument(0);
        SoundBank_B.instance.Play("InstrumentExit", 0);
    }

    //Trompeta
    public void CogeBombo()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[2];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[2];
        Instruments[1].gameObject.SetActive(true);
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        InstrumentAnimationEvent.otherAninator = Instruments[1];

        TalkListen.instance.DisableTalking();

        //SoundBank.instance.PlayEnterInstrument(1);
        SoundBank_B.instance.Play("InstrumentEnter", 1);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void SueltaBombo()
    {
        PocoyoAnimator.SetTrigger("Exit");
        //Instruments[1].SetTrigger("Exit");

        //SoundBank.instance.PlayExitInstrument(1);
        SoundBank_B.instance.Play("InstrumentExit", 1);
    }

    //Platillos
    public void CogeSilvato()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[3];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[3];
        Instruments[2].gameObject.SetActive(true);
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        InstrumentAnimationEvent.otherAninator = Instruments[2];

        TalkListen.instance.DisableTalking();

        //SoundBank.instance.PlayEnterInstrument(2);
        SoundBank_B.instance.Play("InstrumentEnter", 2);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void SueltaSilvato()
    {
        PocoyoAnimator.SetTrigger("Exit");
        //Instruments[2].SetTrigger("Exit");

        //SoundBank.instance.PlayExitInstrument(2);
        SoundBank_B.instance.Play("InstrumentExit", 2);
    }

    //Guitarra
    public void CogeJembe()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[4];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[4];
        Instruments[3].gameObject.SetActive(true);
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        InstrumentAnimationEvent.otherAninator = Instruments[3];

        TalkListen.instance.DisableTalking();

        //SoundBank.instance.PlayEnterInstrument(3);
        SoundBank_B.instance.Play("InstrumentEnter", 3);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void SueltaJembe()
    {
        PocoyoAnimator.SetTrigger("Exit");
       // Instruments[3].SetTrigger("Exit");

        //SoundBank.instance.PlayExitInstrument(3);
        SoundBank_B.instance.Play("InstrumentExit", 3);
    }

    //Tambor
    public void CogeBocina()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[5];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[5];
        Instruments[4].gameObject.SetActive(true);
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        InstrumentAnimationEvent.otherAninator = Instruments[4];

        TalkListen.instance.DisableTalking();

        //SoundBank.instance.PlayEnterInstrument(4);
        SoundBank_B.instance.Play("InstrumentEnter", 4);

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");

        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    public void SueltaBocina()
    {
        PocoyoAnimator.SetTrigger("Exit");
        //Instruments[4].SetTrigger("Exit");

        //SoundBank.instance.PlayExitInstrument(4);
        SoundBank_B.instance.Play("InstrumentExit", 4);
    }

    public void PlayInstrumentBlink()
    {
        PocoyoAnimator.SetTrigger("Blink");
        Invoke("PlayInstrumentBlink", Random.Range(BlinkTime.x, BlinkTime.y));
    }

    #endregion

    public void SetDressAnimator()
    {
        PocoyoAnimator.runtimeAnimatorController = animators[(int)Main.UIModes.UIMODE_CLOTHES];
        if (SoccerBall) SoccerBall.runtimeAnimatorController = BallAnimators[(int)Main.UIModes.UIMODE_CLOTHES];

        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        aee.ExitAnimationEnded.AddListener(ChangeAnimationControler);

        TalkListen.instance.DisableTalking();

        CancelInvoke("PlayBlink");
        CancelInvoke("PlayInactivity");
    }

    public void PlayShirtAnimation()
    {
        PocoyoAnimator.SetInteger("Play", 0);
        PocoyoAnimator.SetTrigger("launchPlay");
    }

    public void PlayBootAnimation()
    {
        PocoyoAnimator.SetInteger("Play", 1);
        PocoyoAnimator.SetTrigger("launchPlay");
    }

    public void ExitDressAnimator()
    {
        PocoyoAnimator.SetTrigger("Exit");
    }

    ///
    public void ChangeAnimationControler()
    {
        AnimatorEndEvent aee = PocoyoAnimator.GetBehaviour<AnimatorEndEvent>();
        if(aee != null)
            aee.ExitAnimationEnded.RemoveListener(ChangeAnimationControler);

        if (lastInstrument > -1)
        {
            Instruments[lastInstrument].gameObject.SetActive(false);
            InstrumentAnimationEvent.otherAninator = null;

            CancelInvoke("PlayInstrumentBlink");
            uiChanged.Invoke();
        }

        raycaster.enabled = false;

        //Stop all timing events
        switch (_uiMode)
        {
            case Main.UIModes.UIMODE_DEFAULT:
                SetDefaultAnimator();
                break;
            case Main.UIModes.UIMODE_VUVUCELA:
                CogeBubucela();
                break;
            case Main.UIModes.UIMODE_BOMBO:                
                CogeBombo();
                break;
            case Main.UIModes.UIMODE_SILBATO:
                CogeSilvato();
                break;
            case Main.UIModes.UIMODE_JEMBE:
                CogeJembe();
                break;
            case Main.UIModes.UIMODE_BOCINA:
                CogeBocina();
                break;
            case Main.UIModes.UIMODE_GAME:
                SetBallGameController();
                break;
            case Main.UIModes.UIMODE_BAILE:
                SetDanceController();
                break;
            case Main.UIModes.UIMODE_CLOTHES:
                SetDressAnimator();
                break;
            //case Main.UIModes.UIMODE_AR:
            //    //SetDanceController();
            //    break;
        }
    }
}
