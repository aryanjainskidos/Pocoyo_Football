using UnityEngine;
using System.Collections.Generic;

public class AudioDucker : MonoBehaviour
{
    [Header("Priority Audio Source (the main one)")]
    public AudioSource prioritySource; 

    [Header("Duck Settings")]
    [Range(0f, 1f)] public float duckVolume = 0.3f; 
    public float fadeSpeed = 2f; 

    private List<AudioSource> allSources = new List<AudioSource>();
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    void Start()
    {
        
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != prioritySource)
            {
                allSources.Add(src);
                originalVolumes[src] = src.volume;
            }
        }
    }

    void Update()
    {
        if (prioritySource == null) return;

       
        bool isPriorityPlaying = prioritySource.isPlaying;

        foreach (AudioSource src in allSources)
        {
            if (src == null) continue;

            float targetVolume = isPriorityPlaying ? duckVolume : originalVolumes[src];
            src.volume = Mathf.MoveTowards(src.volume, targetVolume, Time.deltaTime * fadeSpeed);
        }
    }
}