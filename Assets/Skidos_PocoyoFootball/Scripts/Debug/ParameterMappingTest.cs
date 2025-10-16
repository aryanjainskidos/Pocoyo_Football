using UnityEngine;

public class ParameterMappingTest : MonoBehaviour
{
    [Header("Test Materials")]
    public Material bodyMaterial;
    public Material legsMaterial;
    
    [Header("Test Values")]
    public Color testBodyColor = Color.red;
    public Color testPatternColor = Color.blue;
    public Color testShieldColor = Color.green;
    public Color testSneakerColor = Color.yellow;
    
    void Start()
    {
        if (bodyMaterial == null || legsMaterial == null)
        {
            Debug.LogError("Please assign body and legs materials!");
            return;
        }
        
        Debug.Log("ParameterMappingTest initialized");
        Debug.Log($"Body material shader: {bodyMaterial.shader.name}");
        Debug.Log($"Legs material shader: {legsMaterial.shader.name}");
        
        // Test parameter mapping
        TestParameterMapping();
    }
    
    void TestParameterMapping()
    {
        Debug.Log("=== Testing Parameter Mapping ===");
        
        // Test body parameters
        Debug.Log("Testing Body Parameters:");
        bodyMaterial.SetColor("_Color", testBodyColor);
        Debug.Log($"Set _Color = {testBodyColor}");
        
        bodyMaterial.SetColor("_PatternColor", testPatternColor);
        Debug.Log($"Set _PatternColor = {testPatternColor}");
        
        bodyMaterial.SetColor("_ShieldColor", testShieldColor);
        Debug.Log($"Set _ShieldColor = {testShieldColor}");
        
        // Test legs parameters
        Debug.Log("Testing Legs Parameters:");
        legsMaterial.SetColor("_Color", testSneakerColor);
        Debug.Log($"Set _Color = {testSneakerColor}");
        
        legsMaterial.SetColor("_SneakerColor", testSneakerColor);
        Debug.Log($"Set _SneakerColor = {testSneakerColor}");
        
        Debug.Log("=== Parameter Mapping Test Complete ===");
    }
    
    void Update()
    {
        // Test with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bodyMaterial.SetColor("_Color", Color.red);
            Debug.Log("Set body _Color to RED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bodyMaterial.SetColor("_Color", Color.blue);
            Debug.Log("Set body _Color to BLUE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bodyMaterial.SetColor("_Color", Color.green);
            Debug.Log("Set body _Color to GREEN");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            legsMaterial.SetColor("_Color", Color.yellow);
            Debug.Log("Set legs _Color to YELLOW");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            legsMaterial.SetColor("_Color", Color.magenta);
            Debug.Log("Set legs _Color to MAGENTA");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            legsMaterial.SetColor("_Color", Color.cyan);
            Debug.Log("Set legs _Color to CYAN");
        }
        
        // Reset to white
        if (Input.GetKeyDown(KeyCode.R))
        {
            bodyMaterial.SetColor("_Color", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
            legsMaterial.SetColor("_Color", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
            Debug.Log("Reset all colors to WHITE");
        }
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 250));
        GUILayout.Label("Parameter Mapping Test");
        GUILayout.Label("Press 1,2,3 for body colors");
        GUILayout.Label("Press 4,5,6 for legs colors");
        GUILayout.Label("Press R to reset to white");
        
        if (GUILayout.Button("Test Body Red"))
            bodyMaterial.SetColor("_Color", Color.red);
        if (GUILayout.Button("Test Body Blue"))
            bodyMaterial.SetColor("_Color", Color.blue);
        if (GUILayout.Button("Test Body Green"))
            bodyMaterial.SetColor("_Color", Color.green);
        if (GUILayout.Button("Test Legs Yellow"))
            legsMaterial.SetColor("_Color", Color.yellow);
        if (GUILayout.Button("Test Legs Magenta"))
            legsMaterial.SetColor("_Color", Color.magenta);
        if (GUILayout.Button("Reset All"))
        {
            bodyMaterial.SetColor("_Color", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
            legsMaterial.SetColor("_Color", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
        }
            
        GUILayout.EndArea();
    }
}
