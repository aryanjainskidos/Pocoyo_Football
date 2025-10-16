using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipationPanel : MonoBehaviour
{
    [Header("Data")]
    public CustomizeData data;
    [Header("Images")]
    public Image Base;
    public Image Pattern;
    public Image Shield;

    [ReadOnly]
    public int EquipationIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        EquipationIndex = transform.GetSiblingIndex();
        SetupEquipation();
        if (SGamePackageSave.GetInstance().CurrentUniform == EquipationIndex)
            GetComponent<Toggle>().isOn = true;
    }

    public void SetupEquipation()
    {
        int offset = EquipationIndex * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;
        //Base
        int baseColor = SGamePackageSave.GetInstance().CustomIdx[(int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR + offset];
        int pattern = SGamePackageSave.GetInstance().CustomIdx[(int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE + offset];
        int patternColor = SGamePackageSave.GetInstance().CustomIdx[(int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR + offset];
        int shield = SGamePackageSave.GetInstance().CustomIdx[(int)CustomizeData.CustomizeItems.SHIRT_SHIELD + offset];

        Base.color = data.Colors[baseColor].data;
        //Pattern
        Pattern.sprite = data.Patterns[pattern].Icon;
        Pattern.color = data.Colors[patternColor].data;
        if(Pattern.sprite == null)
            Pattern.color = new Color(1, 1, 1, 0);

        //Shield
        Shield.sprite = data.Shields[shield].Icon;
        if (Shield.sprite == null)
            Shield.color = new Color(1, 1, 1, 0);
        else
            Shield.color = Color.white;
    }
}
