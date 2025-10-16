using UnityEngine;

public class Bootstrapper : MonoBehaviour
{   
    [SerializeField] private int sceneIndexToLoad = 0;
  

    void Start()
    {
        // Loads Intro (index 0) via Addressables
       
        SceneLoader.Instance.LoadScene(sceneIndexToLoad);

    }
}