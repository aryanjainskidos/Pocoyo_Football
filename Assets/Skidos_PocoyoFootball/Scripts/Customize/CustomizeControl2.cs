using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class CustomizeControl2 : MonoBehaviour
{
    public CustomizeData data;
    public ToggleGroup ColorGroup;
    public ToggleGroup TeamsGroup;
    public Toggle ShirtBaseColor;
    public ToggleGroup PatternsGroup;
    public ToggleGroup ShieldsGroup;
    public Toggle SneakerBaseColor;
    public ToggleGroup SnikersGroup;
    public List<EquipationPanel> Equipements;
    public PanelsControls PanelsCtrl;
    public GameObject SavePopup;
    public GameObject NoEditablePopup;
    [Header("Market")]
    public Zinkia.IAPControler TeamIAPControler;
    public Image TeamIAPImagen;
    public UnityEvent OpenTeamMaketPopup;
    public UnityEvent OpenMaketPopup;
    [Header("Pocoyo")]
    public SkinnedMeshRenderer BodyMesh;
    public SkinnedMeshRenderer LegsMesh;
    public MeshRenderer LeftTap;
    public MeshRenderer RightTap;

    public GameObject baseColorShirt;
    public GameObject baseColorSneakers;

    public Sprite SelectedSprite;

    private Material BodyMaterial;
    private Material LegsMaterial;
    private Material LeftTapMaterial;
    private Material RightTapMaterial;

    private int EditingElementIdx = -1;
    private int dataIndex = 0;
    
    // Addressable loading support
    private bool useAddressables = true; // Enable Addressables by default
    private Dictionary<string, AsyncOperationHandle> loadedAssets = new Dictionary<string, AsyncOperationHandle>();
    private CustomizeData loadedCustomizeData; // Store loaded CustomizeData
    private bool isCustomizeDataLoaded = false;
    
    // Property to get the appropriate data source
    private CustomizeData CurrentData
    {
        get
        {
            if (useAddressables && isCustomizeDataLoaded && loadedCustomizeData != null)
            {
                return loadedCustomizeData;
            }
            return data; // Fallback to existing data
        }
    }
    // Shader parameter names for Built-in pipeline - mapped to enum values
    private string[] shaderParameterName = new string[] { 
        "_BodyColor",       // SHIRT_BASE_COLOR (0) - Body material
        "_PatternMask",     // SHIRT_PATTERN_TEXTURE (1) - Body material
        "_PatternColor",    // SHIRT_PATTERN_COLOR (2) - Body material
        "_Shield",          // SHIRT_SHIELD (3) - Body material
        "_LegsColor",       // SNEAKER_BASE_COLOR (4) - Legs material
        "_SneakerPattern",  // SNEAKER_PATTERN_TEXTURE (5) - Legs material
        "_SneakerColor"     // SNEAKER_PATTERN_COLOR (6) - Legs material
    };
    
    // Parameter types: 0 = Color, 1 = Texture
    private int[] parameterTypes = new int[] { 0, 1, 0, 1, 0, 1, 0 };
    private int[] prev_state = new int[(int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE];


    void Awake()
    {
        SavePopup.SetActive(false);
        NoEditablePopup.SetActive(false);

        IAPControl.TeamUnlocked += UnlockEquipementSuccess;
        IAPControl.TeamUnlockedError += UnlockEquipementSuccessError;
        IAPControl.GameIsBought += PurchaseCompletedSuccess;

        BodyMaterial = BodyMesh.material;
        LegsMaterial = LegsMesh.material;

        LeftTapMaterial = LeftTap.material;
        RightTapMaterial = RightTap.material;

        Debug.Log("=== MATERIALS INITIALIZED ===");
        Debug.Log($"Body Material: {BodyMaterial.name} | Shader: {BodyMaterial.shader.name}");
        Debug.Log($"Legs Material: {LegsMaterial.name} | Shader: {LegsMaterial.shader.name}");
        Debug.Log($"Left Tap Material: {LeftTapMaterial.name} | Shader: {LeftTapMaterial.shader.name}");
        Debug.Log($"Right Tap Material: {RightTapMaterial.name} | Shader: {RightTapMaterial.shader.name}");

        FillWardrobeItems();
    }

    void Start()
    {
        // Load CustomizeData from Addressables if enabled
        if (useAddressables)
        {
            LoadCustomizeDataFromAddressables();
        }
        else
        {
            // Initialize the customization system with existing data
            InitializeCustomizationSystem();
        }
    }

    private void ApplyDefaultShield()
    {
        // Force apply default shield (index 0) to ensure materials are properly initialized
        if (CurrentData.Shields != null && CurrentData.Shields.Count > 0)
        {
            Texture2D defaultShield = CurrentData.Shields[0].texture;
            if (defaultShield != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[3], defaultShield);
                Debug.Log($"Applied DEFAULT SHIELD (index 0): {defaultShield.name}");
                
                // Force material update
                ForceMaterialRefresh(BodyMaterial);
            }
        }
    }

    private void ForceMaterialRefresh(Material material)
    {
        // Force Unity to recognize material changes
        if (material.HasProperty("_MainTex"))
        {
            Texture mainTex = material.GetTexture("_MainTex");
            material.SetTexture("_MainTex", mainTex);
        }
        
        // Mark material as dirty to force renderer update
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(material);
        #endif
        
        // Force renderer to update at runtime
        if (BodyMesh != null && BodyMesh.material == material)
        {
            BodyMesh.material = BodyMesh.material; // Force renderer refresh
        }
        if (LegsMesh != null && LegsMesh.material == material)
        {
            LegsMesh.material = LegsMesh.material; // Force renderer refresh
        }
    }
    
    // Addressable loading methods
    private void LoadAssetAsync<T>(AssetReferenceT<T> assetRef, string assetKey, System.Action<T> onLoaded) where T : UnityEngine.Object
    {
        if (assetRef == null || !assetRef.RuntimeKeyIsValid())
        {
            Debug.LogWarning($"Asset reference is null or invalid for key: {assetKey}");
            onLoaded?.Invoke(null);
            return;
        }
        
        // Check if already loaded
        if (loadedAssets.ContainsKey(assetKey) && loadedAssets[assetKey].IsDone)
        {
            T asset = loadedAssets[assetKey].Result as T;
            onLoaded?.Invoke(asset);
            return;
        }
        
        // Load asset asynchronously
        var loadOperation = assetRef.LoadAssetAsync<T>();
        loadedAssets[assetKey] = loadOperation;
        
        loadOperation.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                T asset = op.Result as T;
                Debug.Log($"Successfully loaded Addressable asset: {assetKey}");
                onLoaded?.Invoke(asset);
            }
            else
            {
                Debug.LogError($"Failed to load Addressable asset: {assetKey}, Status: {op.Status}");
                onLoaded?.Invoke(null);
            }
        };
    }
    
    private void UnloadAsset(string assetKey)
    {
        if (loadedAssets.ContainsKey(assetKey))
        {
            Addressables.Release(loadedAssets[assetKey]);
            loadedAssets.Remove(assetKey);
        }
    }
    
    private void UnloadAllAssets()
    {
        foreach (var kvp in loadedAssets)
        {
            Addressables.Release(kvp.Value);
        }
        loadedAssets.Clear();
    }
    
    // Load CustomizeData from Addressables
    private void LoadCustomizeDataFromAddressables()
    {
        if (isCustomizeDataLoaded)
        {
            Debug.Log("CustomizeData already loaded from Addressables");
            return;
        }
        
        Debug.Log("Loading CustomizeData from Addressables...");
        
        // Load the CustomizeData asset from Addressables
        var handle = Addressables.LoadAssetAsync<CustomizeData>("Assets/Data/CutomizeData.asset");
        loadedAssets["CustomizeData"] = handle;
        
        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                loadedCustomizeData = op.Result;
                isCustomizeDataLoaded = true;
                Debug.Log("CustomizeData loaded successfully from Addressables");
                
                // Initialize the customization system with loaded data
                InitializeCustomizationSystem();
            }
            else
            {
                Debug.LogError($"Failed to load CustomizeData from Addressables: {op.OperationException}");
                // Fallback to existing system
                FallbackToExistingSystem();
            }
        };
    }
    
    // Fallback method if Addressable loading fails
    private void FallbackToExistingSystem()
    {
        Debug.Log("Falling back to existing CustomizeData system");
        useAddressables = false;
        // Continue with existing initialization
        InitializeCustomizationSystem();
    }
    
    // Check if Addressable loading was successful
    public bool IsAddressableDataLoaded()
    {
        return useAddressables && isCustomizeDataLoaded && loadedCustomizeData != null;
    }
    
    // Get the current data source (for debugging)
    public CustomizeData GetCurrentDataSource()
    {
        return CurrentData;
    }
    
    // Manually reload Addressable data (useful for testing or content updates)
    public void ReloadAddressableData()
    {
        if (useAddressables)
        {
            Debug.Log("Manually reloading Addressable data...");
            isCustomizeDataLoaded = false;
            loadedCustomizeData = null;
            
            // Unload existing data
            if (loadedAssets.ContainsKey("CustomizeData"))
            {
                Addressables.Release(loadedAssets["CustomizeData"]);
                loadedAssets.Remove("CustomizeData");
            }
            
            // Reload
            LoadCustomizeDataFromAddressables();
        }
        else
        {
            Debug.Log("Addressables not enabled, cannot reload");
        }
    }
    
    // Initialize the customization system with loaded data
    private void InitializeCustomizationSystem()
    {
        Debug.Log("Initializing customization system...");
        
        // Apply default shield (index 0) and then change equipment
        ApplyDefaultShield();
        changeEquipement();
        
        Debug.Log("Customization system initialized successfully");
    }

    public void SetupTeamGroup()
    {
        int teamidx = SGamePackageSave.GetInstance().CurrentUniform;
        SetTeamToggle(teamidx);
        SetEquipement(teamidx);
    }

    private void OnDestroy()
    {
        IAPControl.TeamUnlocked -= UnlockEquipementSuccess;
        IAPControl.TeamUnlockedError -= UnlockEquipementSuccessError;
        IAPControl.GameIsBought -= PurchaseCompletedSuccess;
        
        // Clean up Addressable assets
        UnloadAllAssets();
    }

    public void FillWardrobeItems()
    {
        //Premium Team
        GameObject teamsGO = TeamsGroup.gameObject;
        RectTransform teamsContent = teamsGO.GetComponent<ScrollRect>().content;
        for (int i = 0; i < CurrentData.Teams.Count; i++)
        {
            CustomizeData.PremiumTeam ld = (CustomizeData.PremiumTeam)CurrentData.Teams[i];
            GameObject go = Instantiate(CurrentData.TeamsPrefab, teamsContent);
            go.name = ld.name;

            TeamPanel tp = go.GetComponent<TeamPanel>();

            uint teamFlag = SGamePackageSave.GetInstance().teamLockStatusFlags & (1u << i);

            tp.TeamIndex = i;
            tp.Base.sprite = (teamFlag == 0) ? ld.ui_selected : ld.ui_image;

            int idx = i + 3;            
            tp.toggle.onValueChanged.AddListener(SetEquipement);
            tp.toggle.group = TeamsGroup;
        }

        //Patterns
        GameObject patternsGO = PatternsGroup.gameObject;
        RectTransform patternContent = patternsGO.GetComponent<ScrollRect>().content;
        foreach(CustomizeData.LayerData ld in CurrentData.Patterns)
        {
            GameObject go = Instantiate(CurrentData.PatternsPrefab, patternContent);
            WadrobeItem wi =  go.GetComponent<WadrobeItem>();
            wi.unlocked = ld.ui_image;
            wi.locked = ld.ui_disabled;
            wi.SetLockState(!(SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked));

            //wi.element.sprite = (SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked) ?  ld.ui_image : ld.ui_disabled;

            wi.selected.sprite = ld.ui_selected;
                       
            wi.toggle.onValueChanged.AddListener(SetElementIndex);
            wi.toggle.group = PatternsGroup;            
        }
        
        //Shileds
        GameObject shieldsGO = ShieldsGroup.gameObject;
        RectTransform shiledsContent = shieldsGO.GetComponent<ScrollRect>().content;
        foreach (CustomizeData.LayerData ld in CurrentData.Shields)
        {
            GameObject go = Instantiate(CurrentData.ShieldsPrefab, shiledsContent);
            WadrobeItem wi = go.GetComponent<WadrobeItem>();
            wi.unlocked = ld.ui_image;
            wi.locked = ld.ui_disabled;
            wi.SetLockState(!(SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked));

            //wi.element.sprite = (SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked) ? ld.ui_image : ld.ui_disabled;

            //Germ�n
            //wi.selected.sprite = ld.ui_selected;
            wi.selected.sprite = SelectedSprite;

            wi.toggle.onValueChanged.AddListener(SetElementIndex);
            wi.toggle.group = ShieldsGroup;
        }

        //Snikers
        GameObject sniekersGO = SnikersGroup.gameObject;
        RectTransform sniekersContent = sniekersGO.GetComponent<ScrollRect>().content;
        foreach (CustomizeData.LayerData ld in CurrentData.Sneakers)
        {
            GameObject go = Instantiate(CurrentData.SneakersPrefab, sniekersContent);
            WadrobeItem wi = go.GetComponent<WadrobeItem>();
            wi.unlocked = ld.ui_image;
            wi.locked = ld.ui_disabled;
            wi.SetLockState(!(SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked));

            //wi.element.sprite = (SGamePackageSave.GetInstance().m_IsGameBought || !ld.locked) ? ld.ui_image : ld.ui_disabled;

            wi.selected.sprite = ld.ui_selected;

            wi.toggle.onValueChanged.AddListener(SetElementIndex);
            wi.toggle.group = SnikersGroup;
        }
    }

    public void SetEquipement(bool isOn)
    {
        if (isOn)
        {
            foreach (Toggle t in TeamsGroup.ActiveToggles())
            {
                if (t.isOn)
                {
                    int idx = t.transform.GetSiblingIndex();
                    Debug.Log("Setting equipenmnt " + idx.ToString());
                    SetEquipement(idx);
                    break;
                }
            }
        }
    }

    public void SetEquipement(int index)
    {
        if (index > 2)
        {
            int i = index - 3;
            uint teamFlag = SGamePackageSave.GetInstance().teamLockStatusFlags & (1u << i);
            if (teamFlag == 0)
            {
                CustomizeData.PremiumTeam pt = CurrentData.Teams[i];

                TeamIAPControler.productId = pt.name;
                TeamIAPImagen.overrideSprite = pt.ui_image;

                Debug.Log("Open Team Market Popup");
                OpenTeamMaketPopup.Invoke();
                return;
            }

            PanelsCtrl.DisablePanels();
        }
        else
            PanelsCtrl.EnablePanels();


        SGamePackageSave.GetInstance().CurrentUniform = index;
        SSaveLoad.save();
        changeEquipement();
    }

    public void UnlockEquipementSuccess(int teamIdx)
    {
        GameObject teamsGO = TeamsGroup.gameObject;
        TeamPanel[] tps = teamsGO.GetComponentsInChildren<TeamPanel>();
        CustomizeData.PremiumTeam ld = (CustomizeData.PremiumTeam)CurrentData.Teams[teamIdx];
        uint teamFlag = SGamePackageSave.GetInstance().teamLockStatusFlags & (1u << teamIdx);

        foreach (TeamPanel tp in tps)
        {
            if (tp.TeamIndex == teamIdx)
            {
                tp.Base.sprite = (teamFlag == 0) ? ld.ui_selected : ld.ui_image;
                SGamePackageSave.GetInstance().CurrentUniform = (teamIdx + 3);
                changeEquipement();
            }
        }
    }

    public void UnlockEquipementSuccessError(int teamIdx)
    {
        GameObject teamsGO = TeamsGroup.gameObject;
        Toggle[] ts = teamsGO.GetComponentsInChildren<Toggle>();
        ts[SGamePackageSave.GetInstance().CurrentUniform].isOn = true;
    }

    public void PurchaseCompletedSuccess()
    {
        //Unlock all shirt paterrn
        GameObject patternsGO = PatternsGroup.gameObject;
        WadrobeItem[] pwis = patternsGO.GetComponentsInChildren<WadrobeItem>();
        foreach (WadrobeItem wi in pwis)
            wi.SetLockState(false);
        //Unlock all sneakerPatters
        GameObject shieldsGO = ShieldsGroup.gameObject;
        WadrobeItem[] swis = shieldsGO.GetComponentsInChildren<WadrobeItem>();
        foreach (WadrobeItem wi in swis)
            wi.SetLockState(false);
        //Unlock all sneakerPatters
        GameObject sniekersGO = SnikersGroup.gameObject;
        WadrobeItem[] snwis = sniekersGO.GetComponentsInChildren<WadrobeItem>();
        foreach (WadrobeItem wi in snwis)
            wi.SetLockState(false);
    }

    public void changeEquipement()
    {
        int teamidx = SGamePackageSave.GetInstance().CurrentUniform;
        Debug.Log($"=== CHANGE EQUIPMENT - Team: {teamidx} ===");
        
        //SetTeamToggle(teamidx);

        if (teamidx < 3)
        {
            int offset = teamidx * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;
            Debug.Log($"Using offset: {offset}");

            Array.Copy(SGamePackageSave.GetInstance().CustomIdx, offset, prev_state, 0, prev_state.Length);

            //Base
            int arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR + offset;
            int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Color bodyColor = CurrentData.Colors[idx].data;
            BodyMaterial.SetColor(shaderParameterName[0], bodyColor);
            Debug.Log($"Set BODY base color: {shaderParameterName[0]} = {bodyColor} (RGB: {bodyColor.r}, {bodyColor.g}, {bodyColor.b}) (index: {idx})");
            Debug.Log($"Body material shader: {BodyMaterial.shader.name}");
            Debug.Log($"Body material has parameter {shaderParameterName[0]}: {BodyMaterial.HasProperty(shaderParameterName[0])}");
            
            // Check if the color is actually blue
            if (bodyColor.b > bodyColor.r && bodyColor.b > bodyColor.g)
            {
                Debug.Log($"✅ Color is BLUE (Blue: {bodyColor.b}, Red: {bodyColor.r}, Green: {bodyColor.g})");
            }
            else
            {
                Debug.LogWarning($"⚠️ Color is NOT blue! Blue: {bodyColor.b}, Red: {bodyColor.r}, Green: {bodyColor.g}");
            }

            // Force material refresh to make color change visible immediately
            ForceMaterialRefresh(BodyMaterial);

            //Pattern
            arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Texture2D patternTex = CurrentData.Patterns[idx].texture;
            if (patternTex != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[1], patternTex);
                Debug.Log($"Set BODY pattern texture: {shaderParameterName[1]} = {patternTex.name} (index: {idx})");
            }
            else
            {
                Debug.LogWarning($"Pattern texture is null for index {idx}");
            }
            SetShirtPatternToggle(idx);

            arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Color patternColor = CurrentData.Colors[idx].data;
            BodyMaterial.SetColor(shaderParameterName[2], patternColor);
            Debug.Log($"Set BODY pattern color: {shaderParameterName[2]} = {patternColor} (index: {idx})");

            //Shield
            arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_SHIELD + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Texture2D shieldTex = CurrentData.Shields[idx].texture;
            if (shieldTex != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[3], shieldTex);
                Debug.Log($"Set BODY shield texture: {shaderParameterName[3]} = {shieldTex.name} (index: {idx})");
            }
            else
            {
                Debug.LogWarning($"Shield texture is null for index {idx}");
            }
            SetShirtShieldToggle(idx);

            //Snikers
            arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Color sneakerColor = CurrentData.Colors[idx].data;
            LegsMaterial.SetColor(shaderParameterName[4], sneakerColor);
            LeftTapMaterial.SetColor(shaderParameterName[4], sneakerColor);
            RightTapMaterial.SetColor(shaderParameterName[4], sneakerColor);
            Debug.Log($"Set LEGS base color: {shaderParameterName[4]} = {sneakerColor} (index: {idx})");

            // Force material refresh to make color change visible immediately
            ForceMaterialRefresh(LegsMaterial);
            ForceMaterialRefresh(LeftTapMaterial);
            ForceMaterialRefresh(RightTapMaterial);

            arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Texture2D sneakerTex = CurrentData.Sneakers[idx].texture;
            if (sneakerTex != null)
            {
                LegsMaterial.SetTexture(shaderParameterName[5], sneakerTex);
                Debug.Log($"Set LEGS sneaker texture: {shaderParameterName[5]} = {sneakerTex.name} (index: {idx})");
            }
            else
            {
                Debug.LogWarning($"Sneaker texture is null for index {idx}");
            }
            SetSneakerPatternToggle(idx);

            arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR + offset;
            idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            Color sneakerPatternColor = CurrentData.Colors[idx].data;
            LegsMaterial.SetColor(shaderParameterName[6], sneakerPatternColor);
            Debug.Log($"Set LEGS sneaker pattern color: {shaderParameterName[6]} = {sneakerPatternColor} (index: {idx})");
        }
        else
        {
            teamidx -= 3;
            CustomizeData.PremiumTeam pt = CurrentData.Teams[teamidx];
            Debug.Log($"Using PREMIUM team: {pt.name}");

            //Base
            BodyMaterial.SetColor(shaderParameterName[0], Color.white);
            Debug.Log($"Set PREMIUM BODY base color: {shaderParameterName[0]} = White");

            //Pattern
            if (pt.shirtPattern != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[1], pt.shirtPattern);
                BodyMaterial.SetColor(shaderParameterName[2], Color.white);
                Debug.Log($"Set PREMIUM BODY pattern: {shaderParameterName[1]} = {pt.shirtPattern.name}, {shaderParameterName[2]} = White");
            }
            else
            {
                Debug.LogWarning($"Premium team shirt pattern is null for team {pt.name}");
            }

            //Shield 
            BodyMaterial.SetTexture(shaderParameterName[3], null);
            Debug.Log($"Set PREMIUM BODY shield: {shaderParameterName[3]} = null");

            //Snikers
            LegsMaterial.SetColor(shaderParameterName[4], Color.white);
            LeftTapMaterial.SetColor(shaderParameterName[4], pt.sneakerBaseColor);
            RightTapMaterial.SetColor(shaderParameterName[4], pt.sneakerBaseColor);
            Debug.Log($"Set PREMIUM LEGS base color: {shaderParameterName[4]} = White, Taps = {pt.sneakerBaseColor}");

            if (pt.sneakerPattern != null)
            {
                LegsMaterial.SetTexture(shaderParameterName[5], pt.sneakerPattern);
                LegsMaterial.SetColor(shaderParameterName[6], Color.white);
                Debug.Log($"Set PREMIUM LEGS pattern: {shaderParameterName[5]} = {pt.sneakerPattern.name}, {shaderParameterName[6]} = White");
            }
            else
            {
                Debug.LogWarning($"Premium team sneaker pattern is null for team {pt.name}");
            }
        }
    }

    public void SetColor(bool isOn)
    {
        if(isOn)
        {
            foreach (Toggle t in ColorGroup.ActiveToggles())
            {
                if (t.isOn)
                {
                    int idx = t.transform.GetSiblingIndex();
                    Debug.Log("Setting color " + idx.ToString());
                    SetColor(idx - 1);
                    break;
                }
            }
        }
    }

    public void SetElementIndex(bool isOn)
    {
        if (isOn)
        {            
            if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR)
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE;
                ShirtBaseColor.isOn = false; // .SetIsOnWithoutNotify(false);
            }

            if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE)
            {
                foreach (Toggle t in PatternsGroup.ActiveToggles())
                {
                    if (t.isOn)
                    {
                        int idx = t.transform.GetSiblingIndex();
                        Debug.Log("Setting Pattern " + idx.ToString());
                        SetElementIndex(idx);
                        break;
                    }
                }
            }

            if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_SHIELD)
            {
                foreach (Toggle t in ShieldsGroup.ActiveToggles())
                {
                    if (t.isOn)
                    {
                        int idx = t.transform.GetSiblingIndex();
                        Debug.Log("Setting Shield " + idx.ToString());
                        SetElementIndex(idx);
                        break;
                    }
                }
            }

            if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR)
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE;
                SneakerBaseColor.isOn = false; // SetIsOnWithoutNotify(false);
            }

            if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE)
            {
                foreach (Toggle t in SnikersGroup.ActiveToggles())
                {
                    if (t.isOn)
                    {
                        int idx = t.transform.GetSiblingIndex();
                        Debug.Log("Setting Snikers " + idx.ToString());
                        SetElementIndex(idx);
                        break;
                    }
                }
            }
        }
    }

    public void SetColor(int colorIdx)
    {
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_SHIELD) return; //No COLOR for Shields

        int editingColorParamenter = EditingElementIdx;
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE) editingColorParamenter = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR;
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE) editingColorParamenter = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR;


        if (editingColorParamenter == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR || editingColorParamenter == (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR)
        {
            // Map enum values to correct array indices
            int paramIndex = (editingColorParamenter == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR) ? 0 : 2;
            Color colorToApply = CurrentData.Colors[colorIdx].data;
            BodyMaterial.SetColor(shaderParameterName[paramIndex], colorToApply);
            Debug.Log($"Set body color: {shaderParameterName[paramIndex]} = {colorToApply} (RGB: {colorToApply.r}, {colorToApply.g}, {colorToApply.b})");
            Debug.Log($"Body material shader: {BodyMaterial.shader.name}");
            Debug.Log($"Body material has parameter {shaderParameterName[paramIndex]}: {BodyMaterial.HasProperty(shaderParameterName[paramIndex])}");
            
            // Check if the color is actually blue
            if (colorToApply.b > colorToApply.r && colorToApply.b > colorToApply.g)
            {
                Debug.Log($"✅ Applied BLUE color (Blue: {colorToApply.b}, Red: {colorToApply.r}, Green: {colorToApply.g})");
            }
            else
            {
                Debug.LogWarning($"⚠️ Applied NON-blue color! Blue: {colorToApply.b}, Red: {colorToApply.r}, Green: {colorToApply.g}");
            }
            
            // Force material refresh to make color change visible immediately
            ForceMaterialRefresh(BodyMaterial);
            
            AnimationCtrl.instance.PlayShirtAnimation();
        }

        if (editingColorParamenter == (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR)
        {
            // Sneaker base color affects legs material
            LegsMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[colorIdx].data);
            // Left and right tap materials use the same color
            LeftTapMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[colorIdx].data);
            RightTapMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[colorIdx].data);
            Debug.Log($"Set sneaker base color: {shaderParameterName[4]} = {CurrentData.Colors[colorIdx].data}");
            
            // Force material refresh to make color change visible immediately
            ForceMaterialRefresh(LegsMaterial);
            ForceMaterialRefresh(LeftTapMaterial);
            ForceMaterialRefresh(RightTapMaterial);
            
            AnimationCtrl.instance.PlayBootAnimation();
        }

        if ( editingColorParamenter == (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR)
        {
            // Sneaker pattern color affects legs material
            LegsMaterial.SetColor(shaderParameterName[6], CurrentData.Colors[colorIdx].data);
            Debug.Log($"Set sneaker pattern color: {shaderParameterName[6]} = {CurrentData.Colors[colorIdx].data}");
            
            // Force material refresh to make color change visible immediately
            ForceMaterialRefresh(LegsMaterial);
            
            AnimationCtrl.instance.PlayBootAnimation();
        }

        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;

        SGamePackageSave.GetInstance().CustomIdx[editingColorParamenter + offset] = colorIdx;
        
        Equipements[SGamePackageSave.GetInstance().CurrentUniform].SetupEquipation();
    }

    public void SetElementIndex(int index)
    {
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR) return; //No texture for base color
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR) return; //No texture for base color

        dataIndex = index;
        int saveDataIdx = EditingElementIdx;
        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;

        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE) //Pattern Texture
        {
            if(CurrentData.Patterns[dataIndex].locked && !SGamePackageSave.GetInstance().m_IsGameBought)
            {
                Debug.Log("Show General Market for shirt");
                OpenMaketPopup.Invoke();
                return;
            }
            if (CurrentData.Patterns[dataIndex].texture != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[1], CurrentData.Patterns[dataIndex].texture); // PatternMask
                Debug.Log($"Applied pattern texture: {CurrentData.Patterns[dataIndex].texture.name}");
                
                // Force material refresh to make texture change visible immediately
                ForceMaterialRefresh(BodyMaterial);
                
                AnimationCtrl.instance.PlayShirtAnimation();
            }
            else
            {
                Debug.LogWarning($"Pattern texture is null for pattern index {dataIndex}");
            }
        }
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SHIRT_SHIELD) //Shield Texture
        {
            if (CurrentData.Shields[dataIndex].locked && !SGamePackageSave.GetInstance().m_IsGameBought)
            {
                Debug.Log("Show General Market for Shields");
                OpenMaketPopup.Invoke();
                return;
            }

            if (CurrentData.Shields[dataIndex].texture != null)
            {
                BodyMaterial.SetTexture(shaderParameterName[3], CurrentData.Shields[dataIndex].texture); // Shield
                Debug.Log($"Applied shield texture: {CurrentData.Shields[dataIndex].texture.name}");
                
                // Force material refresh to make texture change visible immediately
                ForceMaterialRefresh(BodyMaterial);
                
                AnimationCtrl.instance.PlayShirtAnimation();
            }
            else
            {
                Debug.LogWarning($"Shield texture is null for shield index {dataIndex}");
            }
        }
        if (EditingElementIdx == (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE) //Sneakers Texture
        {
            if (CurrentData.Sneakers[dataIndex].locked && !SGamePackageSave.GetInstance().m_IsGameBought)
            {
                Debug.Log("Show General Market for Sneakers");
                OpenMaketPopup.Invoke();
                return;
            }

            if (CurrentData.Sneakers[dataIndex].texture != null)
            {
                LegsMaterial.SetTexture(shaderParameterName[5], CurrentData.Sneakers[dataIndex].texture); // SneakerPattern
                Debug.Log($"Applied sneaker texture: {CurrentData.Sneakers[dataIndex].texture.name}");
                
                // Force material refresh to make texture change visible immediately
                ForceMaterialRefresh(LegsMaterial);
                
                if( CurrentData.Sneakers[dataIndex].locked ) //Premium?
                {
                    LegsMaterial.SetColor(shaderParameterName[6], Color.white); // SneakerColor
                    PanelsCtrl.HideColorPanel();
                }
                else
                {
                    PanelsCtrl.ShowColorPanel();
                    //Snikers
                    int arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR + offset;
                    int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
                    LegsMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[idx].data);
                    LeftTapMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[idx].data);
                    RightTapMaterial.SetColor(shaderParameterName[4], CurrentData.Colors[idx].data);

                    arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR + offset;
                    idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
                    LegsMaterial.SetColor(shaderParameterName[6], CurrentData.Colors[idx].data);
                }
                AnimationCtrl.instance.PlayBootAnimation();
            }
            else
            {
                Debug.LogWarning($"Sneaker texture is null for sneaker index {dataIndex}");
            }
        }

        SGamePackageSave.GetInstance().CustomIdx[saveDataIdx + offset] = dataIndex;

        Equipements[SGamePackageSave.GetInstance().CurrentUniform].SetupEquipation();
    }

    int pre_tap = 0;
    public void SetEditTap(int tap)
    {
        if (tap == pre_tap) return;

        if (SGamePackageSave.GetInstance().CurrentUniform > 2)
        {
            //Show no editable popup
            NoEditablePopup.SetActive(true);
            return;
        }

        pre_tap = tap;

        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;
        for (var i = 0; i < prev_state.Length; i++)
        {
            //If we find values at any point that don't match, return false
            if (prev_state[i] != SGamePackageSave.GetInstance().CustomIdx[offset + i])
            {
                SavePopup.SetActive(true);  //Save Popup
                return;
            }
        }

        DoSetEditTap();
    }

    public void ReverseAndDoSetEditTap()
    {
        SavePopup.SetActive(false);

        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;

        Array.Copy(prev_state, 0, SGamePackageSave.GetInstance().CustomIdx, offset, prev_state.Length);

        changeEquipement();

        DoSetEditTap();
    }

    public void SaveAndDoSetEditTap()
    {
        SavePopup.SetActive(false);

        SSaveLoad.save();

        DoSetEditTap();
    }

    void DoSetEditTap()
    {
        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;

        Array.Copy(SGamePackageSave.GetInstance().CustomIdx, offset, prev_state, 0, prev_state.Length);

        if (pre_tap == 0) //Load/Save equipements
        {
            EditingElementIdx = -1;

            baseColorShirt.SetActive(false);
            baseColorSneakers.SetActive(false);
        }

        if (pre_tap == 1) //Shirt stuff
        {
            int arrayIndex = 0;
            if (ShirtBaseColor.isOn)
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR;
                arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR + offset;
            }
            else
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE;
                arrayIndex = (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR + offset;
            }
            
            int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            SetColorToggle(idx + 1); //Color Toggle has one children extra at the beginnig of the child list

            //GERMAN
            baseColorShirt.SetActive(true);
            baseColorSneakers.SetActive(false);
        }

        if(pre_tap == 2) //Shield 
        {
            EditingElementIdx = (int)CustomizeData.CustomizeItems.SHIRT_SHIELD;

            baseColorShirt.SetActive(false);
            baseColorSneakers.SetActive(false);
        }

        if(pre_tap == 3) //Sneakers
        {
            int arrayIndex = 0;
            if (SneakerBaseColor.isOn)
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR;
                arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR + offset;
            }
            else
            {
                EditingElementIdx = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE;
                arrayIndex = (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR + offset;
            }

            int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex];
            SetColorToggle(idx + 1); //Color Toggle has one children extra at the beginnig of the child list

            baseColorShirt.SetActive(false);
            baseColorSneakers.SetActive(true);
        }

        PanelsCtrl.SetPanel(pre_tap);
    }

    public void SetEditShirtBaseColor(bool isOn)
    {
        EditingElementIdx = isOn ? (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR : (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_TEXTURE;

        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;
        int arrayIndex = isOn ? (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR : (int)CustomizeData.CustomizeItems.SHIRT_PATTERN_COLOR;
        int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex + offset];
        SetColorToggle(idx + 1); //Color Toggle has one children extra at the beginnig of the child list
    }

    public void SetEditSneakersBaseColor(bool isOn)
    {
        EditingElementIdx = isOn ? (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR : (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_TEXTURE;

        int offset = SGamePackageSave.GetInstance().CurrentUniform * (int)CustomizeData.CustomizeItems.CUSTOMIZE_ITEM_SIZE;
        int arrayIndex = isOn ? (int)CustomizeData.CustomizeItems.SNEAKER_BASE_COLOR : (int)CustomizeData.CustomizeItems.SNEAKER_PATTERN_COLOR;
        int idx = SGamePackageSave.GetInstance().CustomIdx[arrayIndex + offset];
        SetColorToggle(idx + 1); //Color Toggle has one children extra at the beginnig of the child list
    }

    public void SetColorToggle(int index)
    {
        var Toggles = ColorGroup.transform.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in Toggles)
        {
            int idx = t.transform.GetSiblingIndex();
            if (idx == index)
            {
                Debug.Log("Setting color " + idx.ToString());
                ColorGroup.SetAllTogglesOff();
                t.SetIsOnWithoutNotify(true);
                ColorGroup.EnsureValidState();
                break;
            }
        }
    }

    public void SetTeamToggle(int index)
    {
        var Toggles = TeamsGroup.transform.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in Toggles)
        {
            int idx = t.transform.GetSiblingIndex();
            if (idx == index)
            {
                Debug.Log("Setting team " + idx.ToString());
                TeamsGroup.SetAllTogglesOff();
                t.SetIsOnWithoutNotify(true);
                TeamsGroup.EnsureValidState();
                break;
            }
        }
    }

    public void SetShirtPatternToggle(int index)
    {
        var Toggles = PatternsGroup.transform.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in Toggles)
        {
            int idx = t.transform.GetSiblingIndex();
            if (idx == index)
            {
                Debug.Log("Setting Pattern " + idx.ToString());
                PatternsGroup.SetAllTogglesOff();
                t.SetIsOnWithoutNotify(true);
                PatternsGroup.EnsureValidState();
                break;
            }
        }
    }

    public void SetShirtShieldToggle(int index)
    {
        var Toggles = ShieldsGroup.transform.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in Toggles)
        {
            int idx = t.transform.GetSiblingIndex();
            if (idx == index)
            {
                Debug.Log("Setting shield " + idx.ToString());
                ShieldsGroup.SetAllTogglesOff();
                t.SetIsOnWithoutNotify(true);
                ShieldsGroup.EnsureValidState();
                break;
            }
        }
    }

    public void SetSneakerPatternToggle(int index)
    {
        var Toggles = SnikersGroup.transform.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in Toggles)
        {
            int idx = t.transform.GetSiblingIndex();
            if (idx == index)
            {
                Debug.Log("Setting sneakers " + idx.ToString());
                SnikersGroup.SetAllTogglesOff();
                t.SetIsOnWithoutNotify(true);
                SnikersGroup.EnsureValidState();
                break;
            }
        }
    }

}
