//#if UNITY_PURCHASING
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

namespace Zinkia
{
    [AddComponentMenu("Unity IAP/IAP Controler")]
    public class IAPControler : MonoBehaviour
    {
        [HideInInspector]
        public string productId;

        [Tooltip("Consume the product immediately after a successful purchase")]
        public bool consumePurchase = true;

        [Tooltip("[Optional] Displays the localized title from the app store")]
        public Text titleText;

        [Tooltip("[Optional] Displays the localized description from the app store")]
        public Text descriptionText;

        [Tooltip("[Optional] Displays the localized price from the app store")]
        public Text priceText;

        void Start()
        {
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("IAPControl productId is empty");
            }

            if (!CodelessIAPStoreListener.Instance.HasProductInCatalog(productId))
            {
                Debug.LogWarning("The product catalog has no product with the ID \"" + productId + "\"");
            }
        }

        void OnEnable()
        {
            if (CodelessIAPStoreListener.initializationComplete) {
                UpdateText();
            }
        }

        public void PurchaseProduct()
        {
            Debug.Log("IAPControl.PurchaseProduct() with product ID: " + productId);

            CodelessIAPStoreListener.Instance.InitiatePurchase(productId);
        }
        
        public void UpdateText()
        {
            var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
            if (product != null)
            {
                if (titleText != null)
                {
                    titleText.text = product.metadata.localizedTitle;
                }

                if (descriptionText != null)
                {
                    descriptionText.text = product.metadata.localizedDescription;
                }

                if (priceText != null)
                {
                    priceText.text = product.metadata.localizedPriceString;
                }
            }
        }
    }
}
//#endif
