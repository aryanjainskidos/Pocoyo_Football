using UnityEngine;

public class ShaderTestDebugger : MonoBehaviour
{
    [Header("Materials to Test")]
    public Material bodyMaterial;
    public Material legsMaterial;
    public Material leftTapMaterial;
    public Material rightTapMaterial;
    
    [Header("Test Colors")]
    public Color testBodyColor = Color.red;
    public Color testPatternColor = Color.blue;
    public Color testShieldColor = Color.green;
    public Color testLegsColor = Color.yellow;
    public Color testSneakerColor = Color.magenta;
    
    [Header("Test Textures")]
    public Texture2D testPattern;
    public Texture2D testShield;
    public Texture2D testSneakerPattern;
    
    [Header("Test Controls")]
    public bool testColors = false;
    public bool testTextures = false;
    public bool resetToDefault = false;
    
    private void Start()
    {
        // Auto-find materials if not assigned
        if (bodyMaterial == null)
        {
            var bodyRenderer = GameObject.Find("Pocoyo")?.GetComponentInChildren<SkinnedMeshRenderer>();
            if (bodyRenderer != null)
                bodyMaterial = bodyRenderer.material;
        }
        
        if (legsMaterial == null)
        {
            var legsRenderer = GameObject.Find("Pocoyo")?.GetComponentInChildren<SkinnedMeshRenderer>();
            if (legsRenderer != null && legsRenderer != bodyMaterial)
                legsMaterial = legsRenderer.material;
        }
        
        if (leftTapMaterial == null || rightTapMaterial == null)
        {
            var tapRenderers = GameObject.Find("Pocoyo")?.GetComponentsInChildren<MeshRenderer>();
            if (tapRenderers != null && tapRenderers.Length >= 2)
            {
                leftTapMaterial = tapRenderers[0].material;
                rightTapMaterial = tapRenderers[1].material;
            }
        }
        
        LogMaterialInfo();
    }
    
    private void Update()
    {
        if (testColors)
        {
            testColors = false;
            TestColors();
        }
        
        if (testTextures)
        {
            testTextures = false;
            TestTextures();
        }
        
        if (resetToDefault)
        {
            resetToDefault = false;
            ResetToDefault();
        }
    }
    
    private void LogMaterialInfo()
    {
        Debug.Log("=== SHADER TEST DEBUGGER ===");
        if (bodyMaterial != null)
            Debug.Log($"Body material: {bodyMaterial.name} with shader: {bodyMaterial.shader.name}");
        else
            Debug.LogWarning("Body material is null!");
            
        if (legsMaterial != null)
            Debug.Log($"Legs material: {legsMaterial.name} with shader: {legsMaterial.shader.name}");
        else
            Debug.LogWarning("Legs material is null!");
            
        if (leftTapMaterial != null)
            Debug.Log($"Left tap material: {leftTapMaterial.name} with shader: {leftTapMaterial.shader.name}");
        else
            Debug.LogWarning("Left tap material is null!");
            
        if (rightTapMaterial != null)
            Debug.Log($"Right tap material: {rightTapMaterial.name} with shader: {rightTapMaterial.shader.name}");
        else
            Debug.LogWarning("Right tap material is null!");
    }
    
    public void TestColors()
    {
        Debug.Log("=== TESTING COLORS ===");
        
        if (bodyMaterial != null)
        {
            bodyMaterial.SetColor("_BodyColor", testBodyColor);
            bodyMaterial.SetColor("_PatternColor", testPatternColor);
            bodyMaterial.SetColor("_ShieldColor", testShieldColor);
            Debug.Log($"Applied colors to body: Body={testBodyColor}, Pattern={testPatternColor}, Shield={testShieldColor}");
        }
        
        if (legsMaterial != null)
        {
            legsMaterial.SetColor("_LegsColor", testLegsColor);
            legsMaterial.SetColor("_SneakerColor", testSneakerColor);
            Debug.Log($"Applied colors to legs: Legs={testLegsColor}, Sneaker={testSneakerColor}");
        }
        
        if (leftTapMaterial != null)
        {
            leftTapMaterial.SetColor("_LegsColor", testLegsColor);
            Debug.Log($"Applied color to left tap: {testLegsColor}");
        }
        
        if (rightTapMaterial != null)
        {
            rightTapMaterial.SetColor("_LegsColor", testLegsColor);
            Debug.Log($"Applied color to right tap: {testLegsColor}");
        }
    }
    
    public void TestTextures()
    {
        Debug.Log("=== TESTING TEXTURES ===");
        
        if (bodyMaterial != null)
        {
            if (testPattern != null)
            {
                bodyMaterial.SetTexture("_PatternMask", testPattern);
                Debug.Log($"Applied pattern texture: {testPattern.name}");
            }
            else
            {
                Debug.LogWarning("Test pattern texture is null!");
            }
            
            if (testShield != null)
            {
                bodyMaterial.SetTexture("_Shield", testShield);
                Debug.Log($"Applied shield texture: {testShield.name}");
            }
            else
            {
                Debug.LogWarning("Test shield texture is null!");
            }
        }
        
        if (legsMaterial != null)
        {
            if (testSneakerPattern != null)
            {
                legsMaterial.SetTexture("_SneakerPattern", testSneakerPattern);
                Debug.Log($"Applied sneaker pattern texture: {testSneakerPattern.name}");
            }
            else
            {
                Debug.LogWarning("Test sneaker pattern texture is null!");
            }
        }
    }
    
    public void ResetToDefault()
    {
        Debug.Log("=== RESETTING TO DEFAULT ===");
        
        if (bodyMaterial != null)
        {
            bodyMaterial.SetColor("_BodyColor", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
            bodyMaterial.SetTexture("_PatternMask", null);
            bodyMaterial.SetTexture("_Shield", null);
            Debug.Log("Reset body material to default");
        }
        
        if (legsMaterial != null)
        {
            legsMaterial.SetColor("_LegsColor", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
            legsMaterial.SetTexture("_SneakerPattern", null);
            Debug.Log("Reset legs material to default");
        }
        
        if (leftTapMaterial != null)
        {
            leftTapMaterial.SetColor("_LegsColor", Color.white);
            Debug.Log("Reset left tap material to default");
        }
        
        if (rightTapMaterial != null)
        {
            rightTapMaterial.SetColor("_LegsColor", Color.white);
            Debug.Log("Reset right tap material to default");
        }
    }
    
    [ContextMenu("Log Material Info")]
    public void LogMaterialInfoContext()
    {
        LogMaterialInfo();
    }
    
    [ContextMenu("Test Colors")]
    public void TestColorsContext()
    {
        TestColors();
    }
    
    [ContextMenu("Test Textures")]
    public void TestTexturesContext()
    {
        TestTextures();
    }
    
    [ContextMenu("Reset to Default")]
    public void ResetToDefaultContext()
    {
        ResetToDefault();
    }
}
