using UnityEngine;
using System.Collections.Generic;

public class HandCoachManager : MonoBehaviour
{
    #region Serialiazed Fields
    
    [Header("Hand Coach Keys")]
    [SerializeField] private List<HandCoachConfig> handCoachConfigs;
    
    #endregion
    public static HandCoachManager Instance { get; private set; }

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
       DontDestroyOnLoad(gameObject);
    }

    public HandCoachConfig GetConfigByIndex(int index)
    {
        if (handCoachConfigs == null || handCoachConfigs.Count == 0)
        {
            Debug.LogError("HandCoachConfigs list is empty!");
            return null;
        }
        
        if (index < 0 || index >= handCoachConfigs.Count)
        {
            Debug.LogError($"Index {index} is out of range! List has {handCoachConfigs.Count} elements.");
            return null;
        }
        
        Debug.Log($"Getting config at index {index}, screenKey: {handCoachConfigs[index].screenKey}");
        return handCoachConfigs[index];
    }

   
    public bool IsHandCoachComplete(string screenKey)
    {
        return PlayerPrefs.GetInt(screenKey, 0) == 1;
    }

  
    public void MakeHandCoachComplete(string screenKey)
    {
        PlayerPrefs.SetInt(screenKey, 1);
        PlayerPrefs.Save();
    }

   
    public void ResetAllHandCoaches()
    {
        foreach (var config in handCoachConfigs)
        {
            PlayerPrefs.SetInt(config.screenKey, 0);
        }
        PlayerPrefs.Save();
    }
}