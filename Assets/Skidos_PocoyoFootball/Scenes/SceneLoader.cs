using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zinkia;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneList sceneDatabase;
    public static SceneLoader Instance { get; private set; }

    // Keep track of loaded scenes (for unloading)
    private Dictionary<int, AsyncOperationHandle<SceneInstance>> loadedScenes = new Dictionary<int, AsyncOperationHandle<SceneInstance>>();

    private void Awake()
    {
        // Singleton pattern logic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep across scenes
    }

    // === OLD FUNCTIONS (unchanged so your existing calls don't break) ===
    public void LoadScene(int index)
    {
        StartCoroutine(AsynchronousLoad(sceneDatabase.sceneReferences[index], LoadSceneMode.Single, index));
        Debug.Log("inside the Loadscene ");
    }
    
    public void LoadByIndex(int index)
    {
        if (index < 0 || index >= sceneDatabase.sceneReferences.Count)
        {
            Debug.LogError($"Invalid scene index: {index}");
            return;
        }
        StartCoroutine(AsynchronousLoad(sceneDatabase.sceneReferences[index], LoadSceneMode.Single, index));
    }

    // === NEW FUNCTIONS ===
    public void LoadSceneSingle(int index)
    {
        StartCoroutine(AsynchronousLoad(sceneDatabase.sceneReferences[index], LoadSceneMode.Single, index));
    }

    public void LoadSceneAdditive(int index)
    {
        StartCoroutine(AsynchronousLoad(sceneDatabase.sceneReferences[index], LoadSceneMode.Additive, index));
    }

    public void UnloadScene(int index)
    {
        if (loadedScenes.ContainsKey(index))
        {
            Debug.Log($"Unloading scene at index {index}...");
            Addressables.UnloadSceneAsync(loadedScenes[index]);
            loadedScenes.Remove(index);
        }
        else
        {
            Debug.LogWarning($"Scene at index {index} is not loaded or already unloaded.");
        }
    }

    // === Updated to accept mode ===
    private IEnumerator AsynchronousLoad(AssetReference sceneReference, LoadSceneMode mode, int index)
    {   
        Debug.Log($"inside the iEnumertor function | Mode: {mode}");
        AsyncOperationHandle<SceneInstance> handle = sceneReference.LoadSceneAsync(mode, activateOnLoad: false);

        while (!handle.IsDone)
        {
            Debug.Log($"Loading progress: {handle.PercentComplete * 100f}%");
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene {sceneReference.RuntimeKey} loaded. Activating scene...");
            AsyncOperation activation = handle.Result.ActivateAsync();

            while (!activation.isDone)
            {
                yield return null;
            }

            // Store handle for unloading later
            if (mode == LoadSceneMode.Additive)
            {
                loadedScenes[index] = handle;
            }

            Debug.Log("Scene activated");
        }
        else
        {
            Debug.LogError($"Failed to load scene {sceneReference.RuntimeKey}");
        }
    }
}