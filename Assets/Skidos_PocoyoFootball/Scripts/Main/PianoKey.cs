using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions_Football;

public class PianoKey : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public static bool KeyPressing = false;
    public static int Instrument = 0;

    public enum TwoKeyInstrument
    {
        KEY_VUVUCELA,
        KEY_BOMBO,
        KEY_SILBATO,
        KEY_DJEMBE,
        KEY_BOCINA,
    };

    public Image target;
    public Sprite UnpressedSprite;
    public Sprite PressedSprite;
    [Header("Audio")]
    public int instrumentMode = 0;
    public UIShakeTween background;
    public AudioSource audioTarget;
    public List<AudioClip> Sounds;

    private void Reset()
    {
        target = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        KeyPressing = true;
        target.overrideSprite = PressedSprite;
        AnimationCtrl.instance.PlayInstrument(instrumentMode);
        Play();
        background.PlayTween();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (KeyPressing)
        {
            target.overrideSprite = PressedSprite;
            AnimationCtrl.instance.PlayInstrument(instrumentMode);
            Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target.overrideSprite = UnpressedSprite;
        //Stop();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KeyPressing = false;
        target.overrideSprite = UnpressedSprite;
        //Stop();
        background.StopTween();
        background.transform.localScale = Vector3.one;

        foreach (GameObject go in eventData.hovered)
        {
            PianoKey pk = go.GetComponent<PianoKey>();
            if (pk != null)
                pk.target.overrideSprite = pk.UnpressedSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        target.overrideSprite = UnpressedSprite;
    }

    void Play()
    {
        audioTarget.Stop();
        audioTarget.clip = Sounds[Instrument];
        audioTarget.Play();
        CancelInvoke();
        Invoke("StopInstrument", audioTarget.clip.length);
    }

    void Stop()
    {
        //audioTarget.clip = null;
        audioTarget.Stop();        
    }

    void StopInstrument()
    {
        AnimationCtrl.instance.StopInstrument();
    }
}
