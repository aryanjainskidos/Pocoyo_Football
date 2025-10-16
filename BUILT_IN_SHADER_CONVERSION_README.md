# Built-in Shader Conversion for Pocoyo Football Game

## Overview
This project has been converted from URP (Universal Render Pipeline) to Built-in Render Pipeline. The custom ShaderGraph shaders that handled clothing customization have been replaced with Built-in compatible shaders.

## What Was Changed

### 1. New Built-in Shaders Created
- `DinamicObjects_BuiltIn.shader` - For body/shirt customization
- `LayersDinamicObjects_BuiltIn.shader` - For layered objects
- `LegsLayersDinamicObjects_BuiltIn.shader` - For legs/sneaker customization
- `Unlit_FacialExpresions_BuiltIn.shader` - For facial expressions

### 2. Script Updates
- `CustomizeControl2.cs` - Updated to use Built-in shader properties
- Shader parameter names changed to match Standard Shader:
  - `_BaseColor` → `_Color`
  - `_PatternMask` → `_PatternMask` (kept for clarity)
  - `_PatternColor` → `_Color` (reused)
  - `_Shield` → `_Shield` (kept for clarity)
  - `_SneakerPattern` → `_SneakerPattern` (kept for clarity)

### 3. Editor Tool
- `ShaderConverter.cs` - Editor script to convert materials automatically

## How to Use

### Step 1: Convert Materials
1. Open Unity Editor
2. Go to `Tools > Convert Shaders to Built-in`
3. Click "Convert All Materials" button
4. This will automatically assign the new Built-in shaders to your materials

### Step 2: Verify Shader Assignment
Check that these materials are using the new shaders:
- `PocBody.mat` → `Custom/DinamicObjects_BuiltIn`
- `PocLegs.mat` → `Custom/LegsLayersDinamicObjects_BuiltIn`

### Step 3: Test Clothing Customization
The clothing store should now work with:
- Shirt base color changes
- Pattern texture changes
- Pattern color changes
- Shield texture changes
- Sneaker base color changes
- Sneaker pattern texture changes
- Sneaker pattern color changes

## Shader Properties

### DinamicObjects_BuiltIn
- `_MainTex` - Base texture
- `_Color` - Base color
- `_PatternMask` - Pattern mask texture
- `_PatternColor` - Pattern color
- `_Shield` - Shield texture
- `_ShieldColor` - Shield color
- `_Glossiness` - Smoothness
- `_Metallic` - Metallic value

### LegsLayersDinamicObjects_BuiltIn
- `_MainTex` - Base texture
- `_Color` - Base color
- `_PatternMask` - Pattern mask texture
- `_PatternColor` - Pattern color
- `_SneakerPattern` - Sneaker pattern texture
- `_SneakerColor` - Sneaker color
- `_Glossiness` - Smoothness
- `_Metallic` - Metallic value
- `_NormalMap` - Normal map texture
- `_NormalStrength` - Normal map strength

## Troubleshooting

### If Clothing Still Doesn't Work:
1. Check Console for errors
2. Verify materials are using the new shaders
3. Ensure shader properties are properly assigned
4. Check that textures are assigned to the correct properties

### Common Issues:
- **Pink Materials**: Shader not found or compiled incorrectly
- **No Color Changes**: Material not using the new shader
- **Missing Textures**: Texture properties not assigned in material

## Benefits of Built-in Pipeline
- Better compatibility with older Unity versions
- Simpler shader compilation
- No URP dependencies
- Standard shader properties that are well-documented

## Reverting Changes
If you need to go back to URP:
1. Restore the original ShaderGraph files
2. Revert `CustomizeControl2.cs` changes
3. Reassign URP shaders to materials
4. Switch back to URP in Project Settings

## Support
For issues or questions about the Built-in shader conversion, check:
1. Unity Console for error messages
2. Material Inspector for shader assignments
3. Shader compilation status in Console
