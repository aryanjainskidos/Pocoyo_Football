using UnityEngine;

public class ColorTestDebugger : MonoBehaviour
{
    [Header("Debug Materials")]
    public Material bodyMaterial;
    public Material legsMaterial;
    
    [Header("Test Colors")]
    public Color testColor1 = Color.red;
    public Color testColor2 = Color.blue;
    public Color testColor3 = Color.green;
    
    [Header("Test Textures")]
    public Texture2D testPattern;
    public Texture2D testShield;
    public Texture2D testSneakerPattern;
    
    void Start()
    {
        if (bodyMaterial == null)
        {
            SkinnedMeshRenderer bodyRenderer = FindObjectOfType<SkinnedMeshRenderer>();
            if (bodyRenderer != null)
                bodyMaterial = bodyRenderer.material;
        }
        
        if (legsMaterial == null)
        {
            SkinnedMeshRenderer legsRenderer = FindObjectOfType<SkinnedMeshRenderer>();
            if (legsRenderer != null && legsRenderer != bodyMaterial)
                legsMaterial = legsRenderer.material;
        }
        
        Debug.Log("ColorTestDebugger initialized");
        if (bodyMaterial != null) Debug.Log($"Body material: {bodyMaterial.name} with shader: {bodyMaterial.shader.name}");
        if (legsMaterial != null) Debug.Log($"Legs material: {legsMaterial.name} with shader: {legsMaterial.shader.name}");
    }
    
    void Update()
    {
        // Test color changes with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestColorChange(testColor1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestColorChange(testColor2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestColorChange(testColor3);
        }
        
        // Test texture changes
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestPatternChange();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TestShieldChange();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            TestSneakerChange();
        }
        
        // Reset to white
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToWhite();
        }
    }
    
    void TestColorChange(Color color)
    {
        Debug.Log($"Testing color change to: {color}");
        
        if (bodyMaterial != null)
        {
            bodyMaterial.SetColor("_Color", color);
            Debug.Log($"Set body material _Color to {color}");
        }
        
        if (legsMaterial != null)
        {
            legsMaterial.SetColor("_Color", color);
            Debug.Log($"Set legs material _Color to {color}");
        }
    }
    
    void TestPatternChange()
    {
        if (testPattern != null && bodyMaterial != null)
        {
            bodyMaterial.SetTexture("_PatternMask", testPattern);
            bodyMaterial.SetColor("_PatternColor", Color.yellow);
            Debug.Log("Applied test pattern and yellow color");
        }
    }
    
    void TestShieldChange()
    {
        if (testShield != null && bodyMaterial != null)
        {
            bodyMaterial.SetTexture("_Shield", testShield);
            bodyMaterial.SetColor("_ShieldColor", Color.cyan);
            Debug.Log("Applied test shield and cyan color");
        }
    }
    
    void TestSneakerChange()
    {
        if (testSneakerPattern != null && legsMaterial != null)
        {
            legsMaterial.SetTexture("_SneakerPattern", testSneakerPattern);
            legsMaterial.SetColor("_SneakerColor", Color.magenta);
            Debug.Log("Applied test sneaker pattern and magenta color");
        }
    }
    
    void ResetToWhite()
    {
        Debug.Log("Resetting all colors to white");
        
        if (bodyMaterial != null)
        {
            bodyMaterial.SetColor("_Color", Color.white);
            bodyMaterial.SetColor("_PatternColor", Color.white);
            bodyMaterial.SetColor("_ShieldColor", Color.white);
        }
        
        if (legsMaterial != null)
        {
            legsMaterial.SetColor("_Color", Color.white);
            legsMaterial.SetColor("_SneakerColor", Color.white);
        }
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("Color Test Debugger");
        GUILayout.Label("Press 1, 2, 3 for test colors");
        GUILayout.Label("Press P for pattern test");
        GUILayout.Label("Press S for shield test");
        GUILayout.Label("Press N for sneaker test");
        GUILayout.Label("Press R to reset to white");
        
        if (GUILayout.Button("Test Red Color"))
            TestColorChange(Color.red);
        if (GUILayout.Button("Test Blue Color"))
            TestColorChange(Color.blue);
        if (GUILayout.Button("Test Green Color"))
            TestColorChange(Color.green);
        if (GUILayout.Button("Reset All"))
            ResetToWhite();
            
        GUILayout.EndArea();
    }
}
