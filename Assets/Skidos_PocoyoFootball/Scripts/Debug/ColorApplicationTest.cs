using UnityEngine;

public class ColorApplicationTest : MonoBehaviour
{
    [Header("Test Materials")]
    public Material bodyMaterial;
    public Material legsMaterial;
    
    [Header("Test Colors")]
    public Color testRed = Color.red;
    public Color testBlue = Color.blue;
    public Color testGreen = Color.green;
    public Color testYellow = Color.yellow;
    
    [Header("Test Methods")]
    public bool useDirectColor = true;
    public bool useTextureBlending = false;
    public bool useAlphaBlending = false;
    
    void Start()
    {
        if (bodyMaterial == null || legsMaterial == null)
        {
            Debug.LogError("Please assign body and legs materials!");
            return;
        }
        
        Debug.Log("=== COLOR APPLICATION TEST INITIALIZED ===");
        Debug.Log($"Body material: {bodyMaterial.name} | Shader: {bodyMaterial.shader.name}");
        Debug.Log($"Legs material: {legsMaterial.name} | Shader: {legsMaterial.shader.name}");
        
        // Test initial colors
        TestColorApplication();
    }
    
    void TestColorApplication()
    {
        Debug.Log("=== TESTING COLOR APPLICATION ===");
        
        if (useDirectColor)
        {
            // Method 1: Direct color application
            bodyMaterial.SetColor("_BodyColor", testRed);
            legsMaterial.SetColor("_LegsColor", testBlue);
            Debug.Log("Applied direct colors: Body=Red, Legs=Blue");
        }
        
        if (useTextureBlending)
        {
            // Method 2: Texture blending colors
            bodyMaterial.SetColor("_PatternColor", testGreen);
            bodyMaterial.SetColor("_ShieldColor", testYellow);
            legsMaterial.SetColor("_SneakerColor", testGreen);
            Debug.Log("Applied texture blending colors");
        }
        
        if (useAlphaBlending)
        {
            // Method 3: Alpha-based color mixing
            Color mixedColor = Color.Lerp(testRed, testBlue, 0.5f);
            bodyMaterial.SetColor("_BodyColor", mixedColor);
            Debug.Log($"Applied alpha-blended color: {mixedColor}");
        }
    }
    
    void Update()
    {
        // Test different color combinations
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bodyMaterial.SetColor("_BodyColor", testRed);
            Debug.Log("Set body to RED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bodyMaterial.SetColor("_BodyColor", testBlue);
            Debug.Log("Set body to BLUE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bodyMaterial.SetColor("_BodyColor", testGreen);
            Debug.Log("Set body to GREEN");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            legsMaterial.SetColor("_LegsColor", testRed);
            Debug.Log("Set legs to RED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            legsMaterial.SetColor("_LegsColor", testBlue);
            Debug.Log("Set legs to BLUE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            legsMaterial.SetColor("_LegsColor", testGreen);
            Debug.Log("Set legs to GREEN");
        }
        
        // Test pattern colors
        if (Input.GetKeyDown(KeyCode.P))
        {
            bodyMaterial.SetColor("_PatternColor", testYellow);
            Debug.Log("Set pattern to YELLOW");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bodyMaterial.SetColor("_ShieldColor", testYellow);
            Debug.Log("Set shield to YELLOW");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            legsMaterial.SetColor("_SneakerColor", testYellow);
            Debug.Log("Set sneaker to YELLOW");
        }
        
        // Test extreme colors
        if (Input.GetKeyDown(KeyCode.E))
        {
            bodyMaterial.SetColor("_BodyColor", Color.magenta);
            legsMaterial.SetColor("_LegsColor", Color.cyan);
            Debug.Log("Set extreme colors: Body=MAGENTA, Legs=CYAN");
        }
        
        // Reset to white
        if (Input.GetKeyDown(KeyCode.R))
        {
            bodyMaterial.SetColor("_BodyColor", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
            legsMaterial.SetColor("_LegsColor", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
            Debug.Log("Reset all colors to WHITE");
        }
        
        // Test shader switching
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestShaderSwitching();
        }
    }
    
    void TestShaderSwitching()
    {
        Debug.Log("=== TESTING SHADER SWITCHING ===");
        
        // Try the simple color-only shader
        Shader simpleShader = Shader.Find("Custom/SimpleColorOnly_BuiltIn");
        if (simpleShader != null)
        {
            bodyMaterial.shader = simpleShader;
            legsMaterial.shader = simpleShader;
            Debug.Log("Switched to SimpleColorOnly shader");
            
            // Apply strong colors
            bodyMaterial.SetColor("_BodyColor", Color.red);
            legsMaterial.SetColor("_LegsColor", Color.blue);
        }
        else
        {
            Debug.LogError("SimpleColorOnly shader not found!");
        }
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 400, 350));
        GUILayout.Label("Color Application Test");
        GUILayout.Label("Press 1,2,3 for body colors");
        GUILayout.Label("Press 4,5,6 for legs colors");
        GUILayout.Label("Press P for pattern, S for shield, N for sneaker");
        GUILayout.Label("Press E for extreme colors");
        GUILayout.Label("Press T to test shader switching");
        GUILayout.Label("Press R to reset to white");
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Test Red Body"))
            bodyMaterial.SetColor("_BodyColor", testRed);
        if (GUILayout.Button("Test Blue Body"))
            bodyMaterial.SetColor("_BodyColor", testBlue);
        if (GUILayout.Button("Test Green Body"))
            bodyMaterial.SetColor("_BodyColor", testGreen);
        if (GUILayout.Button("Test Red Legs"))
            legsMaterial.SetColor("_LegsColor", testRed);
        if (GUILayout.Button("Test Blue Legs"))
            legsMaterial.SetColor("_LegsColor", testBlue);
        if (GUILayout.Button("Test Green Legs"))
            legsMaterial.SetColor("_LegsColor", testGreen);
        if (GUILayout.Button("Test Extreme Colors"))
        {
            bodyMaterial.SetColor("_BodyColor", Color.magenta);
            legsMaterial.SetColor("_LegsColor", Color.cyan);
        }
        if (GUILayout.Button("Test Shader Switch"))
            TestShaderSwitching();
        if (GUILayout.Button("Reset All"))
        {
            bodyMaterial.SetColor("_BodyColor", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
            legsMaterial.SetColor("_LegsColor", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
        }
            
        GUILayout.EndArea();
    }
}
