using UnityEngine;

public class ApplicationExit : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
  
    public void QuitGame()
    {
        Debug.Log("Quit Game called!");
        Application.Quit();
        audioSource.PlayOneShot(audioClip);

        // For testing inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}