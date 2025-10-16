using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutomizeData", menuName = "CustomizeData", order = 6)]
public class CustomizeData : TrScriptableObject
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
        //[TrText]
        public string name;
        public Color data;
    };

    [System.Serializable]
    public struct LayerData
    {
        //[TrText]
        public string name;
        public Texture2D texture;
        public Sprite Icon;
        public Sprite ui_image;
        public Sprite ui_selected;
        public Sprite ui_disabled;
        public bool locked;
    };

    [System.Serializable]
    public struct PremiumTeam
    {
        public string name;
        public Sprite ui_image;
        public Sprite ui_selected;
        public Texture2D shirtPattern;
        public Color sneakerBaseColor;        
        public Texture2D sneakerPattern;
    }

    public List<ColorData> Colors;

    public GameObject PatternsPrefab;
    public List<LayerData> Patterns;
    
    public GameObject ShieldsPrefab;
    public List<LayerData> Shields;

    public Texture2D SneakersDefaultMask;
    public GameObject SneakersPrefab;
    public List<LayerData> Sneakers;

    public GameObject TeamsPrefab;
    public List<PremiumTeam> Teams;
}
