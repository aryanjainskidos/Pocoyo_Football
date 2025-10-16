using UnityEngine;

public class VoiceOver : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public float reminderDelay = 10f;   
    private float lastInteractionTime;

    private bool firstTimePlayed = false;
    
    

    void Start()
    {
        PlayVoiceOver(); 
    }

    void Update()
    {
     
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            RegisterInteraction();
        }

      
        if (firstTimePlayed && Time.time - lastInteractionTime >= reminderDelay)
        {
            PlayVoiceOver();
        }
    }

    private void RegisterInteraction()
    {
        lastInteractionTime = Time.time;
    }

    public void PlayVoiceOver()
    {
        if (audioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

        firstTimePlayed = true;
        lastInteractionTime = Time.time;
    }
}