using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class IAPControl : MonoBehaviour {
    
    public static Action GameIsBought;
    public static Action<int> TeamUnlocked;
    public static Action<int> TeamUnlockedError;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ProductPurchased(Product product)
    {
        string productId = product.definition.id;

        if (productId.Contains("noads"))
        {
            HideAds();
            return;
        }
        else if (productId.Contains("football.team"))
        {
            string teamNumber = productId.Substring(productId.Length - 1, 1);
            int teamIdx = int.Parse(teamNumber);

            uint statusFlags = SGamePackageSave.GetInstance().teamLockStatusFlags;
            statusFlags |= Convert.ToUInt32(1 << teamIdx);
            SGamePackageSave.GetInstance().teamLockStatusFlags = statusFlags;
            SSaveLoad.save();
            if (TeamUnlocked != null)
                TeamUnlocked.Invoke(teamIdx);
        }
    }

    public void ProductPurchasedError(Product product, PurchaseFailureReason pfr)
    {
        string productId = product.definition.id;
        if (productId.Contains("football.team"))
        {
            string teamNumber = productId.Substring(productId.Length - 1, 1);
            int teamIdx = int.Parse(teamNumber);
            if (TeamUnlockedError != null)
                TeamUnlockedError.Invoke(teamIdx);
        }
    }

    public void HideAds()
    {
        SGamePackageSave.GetInstance().m_IsGameBought = true;
        SSaveLoad.save();
        if(GameIsBought != null)
            GameIsBought.Invoke();
    }
}
