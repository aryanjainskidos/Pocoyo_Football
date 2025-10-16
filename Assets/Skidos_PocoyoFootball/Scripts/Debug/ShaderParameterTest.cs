// using UnityEngine;
//
// public class ShaderParameterTest : MonoBehaviour
// {
//     [Header("Test Materials")]
//     public Material bodyMaterial;
//     public Material legsMaterial;
//     
//     [Header("Test Colors")]
//     public Color testBodyColor = Color.red;
//     public Color testPatternColor = Color.blue;
//     public Color testShieldColor = Color.green;
//     public Color testLegsColor = Color.yellow;
//     public Color testSneakerColor = Color.magenta;
//     
//     void Start()
//     {
//         if (bodyMaterial == null || legsMaterial == null)
//         {
//             Debug.LogError("Please assign body and legs materials!");
//             return;
//         }
//         
//         Debug.Log("=== SHADER PARAMETER TEST INITIALIZED ===");
//         Debug.Log($"Body material: {bodyMaterial.name} | Shader: {bodyMaterial.shader.name}");
//         Debug.Log($"Legs material: {legsMaterial.name} | Shader: {legsMaterial.shader.name}");
//         
//         // Test all parameters
//         TestAllParameters();
//     }
//     
//     void TestAllParameters()
//     {
//         Debug.Log("=== TESTING ALL SHADER PARAMETERS ===");
//         
//         // Test Body Parameters
//         Debug.Log("Testing BODY Parameters:");
//         bodyMaterial.SetColor("_BodyColor", testBodyColor);
//         Debug.Log($"Set _BodyColor = {testBodyColor}");
//         
//         bodyMaterial.SetColor("_PatternColor", testPatternColor);
//         Debug.Log($"Set _PatternColor = {testPatternColor}");
//         
//         bodyMaterial.SetColor("_ShieldColor", testShieldColor);
//         Debug.Log($"Set _ShieldColor = {testShieldColor}");
//         
//         // Test Legs Parameters
//         Debug.Log("Testing LEGS Parameters:");
//         legsMaterial.SetColor("_LegsColor", testLegsColor);
//         Debug.Log($"Set _LegsColor = {testLegsColor}");
//         
//         legsMaterial.SetColor("_SneakerColor", testSneakerColor);
//         Debug.Log($"Set _SneakerColor = {testSneakerColor}");
//         
//         Debug.Log("=== PARAMETER TEST COMPLETE ===");
//     }
//     
//     void Update()
//     {
//         // Test with number keys
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             bodyMaterial.SetColor("_BodyColor", Color.red);
//             Debug.Log("Set body _BodyColor to RED");
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha2))
//         {
//             bodyMaterial.SetColor("_BodyColor", Color.blue);
//             Debug.Log("Set body _BodyColor to BLUE");
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha3))
//         {
//             bodyMaterial.SetColor("_BodyColor", Color.green);
//             Debug.Log("Set body _BodyColor to GREEN");
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha4))
//         {
//             legsMaterial.SetColor("_LegsColor", Color.yellow);
//             Debug.Log("Set legs _LegsColor to YELLOW");
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha5))
//         {
//             legsMaterial.SetColor("_LegsColor", Color.magenta);
//             Debug.Log("Set legs _LegsColor to MAGENTA");
//         }
//         if (Input.GetKeyDown(KeyCode.Alpha6))
//         {
//             legsMaterial.SetColor("_LegsColor", Color.cyan);
//             Debug.Log("Set legs _LegsColor to CYAN");
//         }
//         
//         // Test pattern colors
//         if (Input.GetKeyDown(KeyCode.P))
//         {
//             bodyMaterial.SetColor("_PatternColor", Color.orange);
//             Debug.Log("Set body _PatternColor to ORANGE");
//         }
//         if (Input.GetKeyDown(KeyCode.S))
//         {
//             bodyMaterial.SetColor("_ShieldColor", Color.purple);
//             Debug.Log("Set body _ShieldColor to PURPLE");
//         }
//         if (Input.GetKeyDown(KeyCode.N))
//         {
//             legsMaterial.SetColor("_SneakerColor", Color.brown);
//             Debug.Log("Set legs _SneakerColor to BROWN");
//         }
//         
//         // Reset to white
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             bodyMaterial.SetColor("_BodyColor", Color.white);
//             bodyMaterial.SetColor("_PatternColor", Color.white);
//             bodyMaterial.SetColor("_ShieldColor", Color.white);
//             legsMaterial.SetColor("_LegsColor", Color.white);
//             legsMaterial.SetColor("_SneakerColor", Color.white);
//             Debug.Log("Reset all colors to WHITE");
//         }
//     }
//     
//     void OnGUI()
//     {
//         GUILayout.BeginArea(new Rect(10, 10, 350, 300));
//         GUILayout.Label("Shader Parameter Test", GUI.skin.label);
//         GUILayout.Label("Press 1,2,3 for body colors");
//         GUILayout.Label("Press 4,5,6 for legs colors");
//         GUILayout.Label("Press P for pattern color");
//         GUILayout.Label("Press S for shield color");
//         GUILayout.Label("Press N for sneaker color");
//         GUILayout.Label("Press R to reset to white");
//         
//         GUILayout.Space(10);
//         
//         if (GUILayout.Button("Test Body Red"))
//             bodyMaterial.SetColor("_BodyColor", Color.red);
//         if (GUILayout.Button("Test Body Blue"))
//             bodyMaterial.SetColor("_BodyColor", Color.blue);
//         if (GUILayout.Button("Test Body Green"))
//             bodyMaterial.SetColor("_BodyColor", Color.green);
//         if (GUILayout.Button("Test Legs Yellow"))
//             legsMaterial.SetColor("_LegsColor", Color.yellow);
//         if (GUILayout.Button("Test Legs Magenta"))
//             legsMaterial.SetColor("_LegsColor", Color.magenta);
//         if (GUILayout.Button("Test Pattern Orange"))
//             bodyMaterial.SetColor("_PatternColor", Color.orange);
//         if (GUILayout.Button("Test Shield Purple"))
//             bodyMaterial.SetColor("_ShieldColor", Color.purple);
//         if (GUILayout.Button("Test Sneaker Brown"))
//             legsMaterial.SetColor("_SneakerColor", Color.brown);
//         if (GUILayout.Button("Reset All"))
//         {
//             bodyMaterial.SetColor("_BodyColor", Color.white);
//             bodyMaterial.SetColor("_PatternColor", Color.white);
//             bodyMaterial.SetColor("_ShieldColor", Color.white);
//             legsMaterial.SetColor("_LegsColor", Color.white);
//             legsMaterial.SetColor("_SneakerColor", Color.white);
//         }
//             
//         GUILayout.EndArea();
//     }
// }
