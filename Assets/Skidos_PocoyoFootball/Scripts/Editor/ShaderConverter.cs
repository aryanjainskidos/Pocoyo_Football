#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ShaderConverter : EditorWindow
{
    [MenuItem("Tools/Convert Shaders to Built-in")]
    public static void ShowWindow()
    {
        GetWindow<ShaderConverter>("Shader Converter");
    }

    void OnGUI()
    {
        GUILayout.Label("Convert ShaderGraph Materials to Built-in Shaders", EditorStyles.boldLabel);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Convert PocBody Material"))
        {
            ConvertPocBodyMaterial();
        }
        
        if (GUILayout.Button("Convert PocLegs Material"))
        {
            ConvertPocLegsMaterial();
        }
        
        if (GUILayout.Button("Convert All Materials"))
        {
            ConvertAllMaterials();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Instructions:", EditorStyles.boldLabel);
        GUILayout.Label("1. Click 'Convert All Materials' to update all materials");
        GUILayout.Label("2. The new shaders will use Standard Shader properties");
        GUILayout.Label("3. Clothing customization should work again");
    }
    
    void ConvertPocBodyMaterial()
    {
        Material pocBody = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/PocBody.mat");
        if (pocBody != null)
        {
            Shader newShader = Shader.Find("Custom/BodyClothing_BuiltIn");
            if (newShader != null)
            {
                pocBody.shader = newShader;
                EditorUtility.SetDirty(pocBody);
                Debug.Log("Converted PocBody material to BodyClothing_BuiltIn shader");
            }
            else
            {
                Debug.LogError("Could not find Custom/BodyClothing_BuiltIn shader");
            }
        }
        else
        {
            Debug.LogError("Could not find PocBody material");
        }
    }
    
    void ConvertPocLegsMaterial()
    {
        Material pocLegs = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/PocLegs.mat");
        if (pocLegs != null)
        {
            Shader newShader = Shader.Find("Custom/LegsClothing_BuiltIn");
            if (newShader != null)
            {
                pocLegs.shader = newShader;
                EditorUtility.SetDirty(pocLegs);
                Debug.Log("Converted PocLegs material to LegsClothing_BuiltIn shader");
            }
            else
            {
                Debug.LogError("Could not find Custom/LegsClothing_BuiltIn shader");
            }
        }
        else
        {
            Debug.LogError("Could not find Custom/LegsClothing_BuiltIn shader");
        }
    }
    
    void ConvertAllMaterials()
    {
        ConvertPocBodyMaterial();
        ConvertPocLegsMaterial();
        
        // Convert other materials as needed
        Material[] allMaterials = Resources.FindObjectsOfTypeAll<Material>();
        foreach (Material mat in allMaterials)
        {
            if (mat.shader.name.Contains("ShaderGraph"))
            {
                // Try to find appropriate Built-in shader
                if (mat.name.Contains("Body") || mat.name.Contains("Shirt"))
                {
                    Shader newShader = Shader.Find("Custom/BodyClothing_BuiltIn");
                    if (newShader != null)
                    {
                        mat.shader = newShader;
                        EditorUtility.SetDirty(mat);
                        Debug.Log($"Converted {mat.name} to BodyClothing_BuiltIn shader");
                    }
                }
                else if (mat.name.Contains("Legs") || mat.name.Contains("Sneaker"))
                {
                    Shader newShader = Shader.Find("Custom/LegsClothing_BuiltIn");
                    if (newShader != null)
                    {
                        mat.shader = newShader;
                        EditorUtility.SetDirty(mat);
                        Debug.Log($"Converted {mat.name} to LegsClothing_BuiltIn shader");
                    }
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("All materials converted to Built-in shaders!");
    }
}

#endif
