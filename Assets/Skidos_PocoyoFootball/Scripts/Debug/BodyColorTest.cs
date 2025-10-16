using UnityEngine;

public class BodyColorTest : MonoBehaviour
{
    [Header("Test Colors")]
    public Color testBlueColor = new Color(0.2f, 0.4f, 0.8f, 1f); // Medium blue
    public Color testDarkBlueColor = new Color(0.1f, 0.2f, 0.6f, 1f); // Dark blue
    public Color testLightBlueColor = new Color(0.6f, 0.8f, 1f, 1f); // Light blue
    
    [Header("Test Controls")]
    public bool testBlue = false;
    public bool testDarkBlue = false;
    public bool testLightBlue = false;
    public bool resetToWhite = false;
    
    private Material bodyMaterial;
    private Material legsMaterial;
    
    void Start()
    {
        // Find the Pocoyo character and get materials
        var pocoyo = GameObject.Find("Pocoyo");
        if (pocoyo != null)
        {
            var renderers = pocoyo.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (renderers.Length > 0)
            {
                bodyMaterial = renderers[0].material;
                if (renderers.Length > 1)
                    legsMaterial = renderers[1].material;
                    
                Debug.Log($"Found body material: {bodyMaterial.name} with shader: {bodyMaterial.shader.name}");
                if (legsMaterial != null)
                    Debug.Log($"Found legs material: {legsMaterial.name} with shader: {legsMaterial.shader.name}");
            }
        }
        
        if (bodyMaterial == null)
        {
            Debug.LogError("Could not find body material!");
        }
    }
    
    void Update()
    {
        if (testBlue)
        {
            testBlue = false;
            ApplyBlueColor(testBlueColor, "Medium Blue");
        }
        
        if (testDarkBlue)
        {
            testDarkBlue = false;
            ApplyBlueColor(testDarkBlueColor, "Dark Blue");
        }
        
        if (testLightBlue)
        {
            testLightBlue = false;
            ApplyBlueColor(testLightBlueColor, "Light Blue");
        }
        
        if (resetToWhite)
        {
            resetToWhite = false;
            ResetToWhite();
        }
    }
    
    void ApplyBlueColor(Color color, string colorName)
    {
        if (bodyMaterial == null)
        {
            Debug.LogError("Body material is null!");
            return;
        }
        
        Debug.Log($"=== APPLYING {colorName} TO BODY ===");
        Debug.Log($"Color: {color} (RGB: {color.r}, {color.g}, {color.b})");
        
        // Try different parameter names that might be used
        string[] possibleParams = { "_BodyColor", "_Color", "_BaseColor", "_MainColor" };
        
        foreach (string param in possibleParams)
        {
            if (bodyMaterial.HasProperty(param))
            {
                bodyMaterial.SetColor(param, color);
                Debug.Log($"Applied {colorName} to parameter: {param}");
            }
            else
            {
                Debug.Log($"Parameter {param} not found in shader");
            }
        }
        
        // Also try setting the main texture color
        if (bodyMaterial.HasProperty("_MainTex"))
        {
            Debug.Log("Material has _MainTex property");
        }
        
        // Log available properties (runtime-safe way)
        Debug.Log("Available shader properties:");
        var shader = bodyMaterial.shader;
        int propertyCount = shader.GetPropertyCount();
        for (int i = 0; i < propertyCount; i++)
        {
            string propertyName = shader.GetPropertyName(i);
            Debug.Log($"  {propertyName}");
        }
    }
    
    void ResetToWhite()
    {
        if (bodyMaterial == null) return;
        
        Debug.Log("=== RESETTING BODY TO WHITE ===");
        
        string[] possibleParams = { "_BodyColor", "_Color", "_BaseColor", "_MainColor" };
        
        foreach (string param in possibleParams)
        {
            if (bodyMaterial.HasProperty(param))
            {
                bodyMaterial.SetColor(param, Color.white);
                Debug.Log($"Reset parameter {param} to white");
            }
        }
    }
    
    [ContextMenu("Test Medium Blue")]
    public void TestMediumBlue()
    {
        ApplyBlueColor(testBlueColor, "Medium Blue");
    }
    
    [ContextMenu("Test Dark Blue")]
    public void TestDarkBlue()
    {
        ApplyBlueColor(testDarkBlueColor, "Dark Blue");
    }
    
    [ContextMenu("Test Light Blue")]
    public void TestLightBlue()
    {
        ApplyBlueColor(testLightBlueColor, "Light Blue");
    }
    
    [ContextMenu("Reset to White")]
    public void ResetToWhiteContext()
    {
        ResetToWhite();
    }
}
