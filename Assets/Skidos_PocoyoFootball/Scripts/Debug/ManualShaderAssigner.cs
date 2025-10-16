using UnityEngine;

public class ManualShaderAssigner : MonoBehaviour
{
    [Header("Materials to Assign")]
    public Material bodyMaterial;
    public Material legsMaterial;
    
    [Header("Shaders")]
    public Shader bodyShader;
    public Shader legsShader;
    
    void Start()
    {
        // Try to find the shaders if not assigned
        if (bodyShader == null)
            bodyShader = Shader.Find("Custom/BodyClothing_BuiltIn");
        if (legsShader == null)
            legsShader = Shader.Find("Custom/LegsClothing_BuiltIn");
            
        Debug.Log("=== MANUAL SHADER ASSIGNER INITIALIZED ===");
        Debug.Log($"Body shader found: {bodyShader != null}");
        Debug.Log($"Legs shader found: {legsShader != null}");
    }
    
    void Update()
    {
        // Assign shaders with key presses
        if (Input.GetKeyDown(KeyCode.B))
        {
            AssignBodyShader();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AssignLegsShader();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AssignAllShaders();
        }
    }
    
    public void AssignBodyShader()
    {
        if (bodyMaterial != null && bodyShader != null)
        {
            bodyMaterial.shader = bodyShader;
            Debug.Log($"Assigned {bodyShader.name} to {bodyMaterial.name}");
        }
        else
        {
            Debug.LogError("Cannot assign body shader - material or shader is null");
        }
    }
    
    public void AssignLegsShader()
    {
        if (legsMaterial != null && legsShader != null)
        {
            legsMaterial.shader = legsShader;
            Debug.Log($"Assigned {legsShader.name} to {legsMaterial.name}");
        }
        else
        {
            Debug.LogError("Cannot assign legs shader - material or shader is null");
        }
    }
    
    public void AssignAllShaders()
    {
        AssignBodyShader();
        AssignLegsShader();
        Debug.Log("All shaders assigned!");
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("Manual Shader Assigner");
        GUILayout.Label("Press B to assign body shader");
        GUILayout.Label("Press L to assign legs shader");
        GUILayout.Label("Press A to assign all shaders");
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Assign Body Shader"))
            AssignBodyShader();
        if (GUILayout.Button("Assign Legs Shader"))
            AssignLegsShader();
        if (GUILayout.Button("Assign All Shaders"))
            AssignAllShaders();
            
        GUILayout.EndArea();
    }
}
