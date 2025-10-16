using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "CustomizeDataAddressable", menuName = "CustomizeDataAddressable", order = 7)]
public class CustomizeDataAddressable : TrScriptableObject
{
    public enum CustomizeItems
    {
        SHIRT_BASE_COLOR,
        SHIRT_PATTERN_TEXTURE,
        SHIRT_PATTERN_COLOR,
        SHIRT_SHIELD,
        SNEAKER_BASE_COLOR,
        SNEAKER_PATTERN_TEXTURE,
        SNEAKER_PATTERN_COLOR,
        CUSTOMIZE_ITEM_SIZE
    };

    [System.Serializable]
    public struct ColorData
    {
        public string name;
        public Color data;
    };

    [System.Serializable]
    public struct LayerDataAddressable
    {
        public string name;
        public AssetReferenceT<Texture2D> texture;
        public AssetReferenceT<Sprite> Icon;
        public AssetReferenceT<Sprite> ui_image;
        public AssetReferenceT<Sprite> ui_selected;
        public AssetReferenceT<Sprite> ui_disabled;
        public bool locked;
        
        // Helper method to get texture synchronously (for backward compatibility)
        public Texture2D GetTexture()
        {
            if (texture != null && texture.Asset != null)
                return texture.Asset as Texture2D;
            return null;
        }
        
        // Helper method to get sprite synchronously (for backward compatibility)
        public Sprite GetSprite()
        {
            if (ui_image != null && ui_image.Asset != null)
                return ui_image.Asset as Sprite;
            return null;
        }
    };

    [System.Serializable]
    public struct PremiumTeamAddressable
    {
        public string name;
        public AssetReferenceT<Sprite> ui_image;
        public AssetReferenceT<Sprite> ui_selected;
        public AssetReferenceT<Texture2D> shirtPattern;
        public Color sneakerBaseColor;        
        public AssetReferenceT<Texture2D> sneakerPattern;
        
        // Helper method to get shirt pattern texture synchronously
        public Texture2D GetShirtPattern()
        {
            if (shirtPattern != null && shirtPattern.Asset != null)
                return shirtPattern.Asset as Texture2D;
            return null;
        }
        
        // Helper method to get sneaker pattern texture synchronously
        public Texture2D GetSneakerPattern()
        {
            if (sneakerPattern != null && sneakerPattern.Asset != null)
                return sneakerPattern.Asset as Texture2D;
            return null;
        }
    }

    public List<ColorData> Colors;

    public AssetReferenceT<GameObject> PatternsPrefab;
    public List<LayerDataAddressable> Patterns;
    
    public AssetReferenceT<GameObject> ShieldsPrefab;
    public List<LayerDataAddressable> Shields;

    public AssetReferenceT<Texture2D> SneakersDefaultMask;
    public AssetReferenceT<GameObject> SneakersPrefab;
    public List<LayerDataAddressable> Sneakers;

    public AssetReferenceT<GameObject> TeamsPrefab;
    public List<PremiumTeamAddressable> Teams;
    
    // Helper method to get pattern by index
    public LayerDataAddressable GetPattern(int index)
    {
        if (index >= 0 && index < Patterns.Count)
            return Patterns[index];
        return default;
    }
    
    // Helper method to get shield by index
    public LayerDataAddressable GetShield(int index)
    {
        if (index >= 0 && index < Shields.Count)
            return Shields[index];
        return default;
    }
    
    // Helper method to get sneaker by index
    public LayerDataAddressable GetSneaker(int index)
    {
        if (index >= 0 && index < Sneakers.Count)
            return Sneakers[index];
        return default;
    }
    
    // Helper method to get team by index
    public PremiumTeamAddressable GetTeam(int index)
    {
        if (index >= 0 && index < Teams.Count)
            return Teams[index];
        return default;
    }
}
