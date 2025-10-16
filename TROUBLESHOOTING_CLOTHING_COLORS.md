# Troubleshooting Clothing Color Issues

## Problem: Clothes Appear Very Light and Color Functionality Not Working

### Root Causes Identified:
1. **Shader Parameter Mapping Errors** - Script was using enum values directly as array indices instead of correct array indices
2. **Wrong Parameter References** - Left/right tap materials were using shirt color parameter instead of sneaker color parameter
3. **Incorrect Array Index Usage** - SetColor and SetElementIndex methods had parameter mapping issues

### Solutions Applied:

#### 1. ✅ Created Dedicated Shaders with Unique Parameters
**Problem:** Both body and legs materials were using the same `_Color` parameter, causing conflicts
**Solution:** Created separate shaders with unique parameter names:
- **`BodyClothing_BuiltIn.shader`**: Uses `_BodyColor`, `_PatternColor`, `_ShieldColor`
- **`LegsClothing_BuiltIn.shader`**: Uses `_LegsColor`, `_SneakerColor`

#### 2. ✅ Fixed Shader Parameter Mapping
**Before (Problematic):**
```csharp
// Script was using enum values directly as array indices
BodyMaterial.SetColor(shaderParameterName[editingColorParamenter], data.Colors[colorIdx].data);
// editingColorParamenter was 0, 2, 4, 6 (enum values) but array indices should be 0, 2, 4, 6
```

**After (Fixed):**
```csharp
// Map enum values to correct array indices
int paramIndex = (editingColorParamenter == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR) ? 0 : 2;
BodyMaterial.SetColor(shaderParameterName[paramIndex], data.Colors[colorIdx].data);
```

#### 3. ✅ Fixed Parameter Mapping Issues
- **SetColor Method:** Fixed enum value to array index mapping
- **SetElementIndex Method:** Fixed texture parameter mapping
- **Left/Right Tap Materials:** Fixed to use sneaker color parameter instead of shirt color parameter
- **Debug Logging:** Added detailed logging to track parameter changes

#### 4. ✅ Created Debug Tools
- **ShaderParameterTest.cs:** Tests new dedicated shader parameters
- **ParameterMappingTest.cs:** Tests individual shader parameters
- **ColorTestDebugger.cs:** Tests color changes in real-time
- **Enhanced Logging:** Detailed console output for troubleshooting

#### 5. ✅ Created Manual Shader Assignment Tools
- **`ManualShaderAssigner.cs`**: Runtime script to assign correct shaders to materials
- **`ShaderParameterTest.cs`**: Tests the new dedicated shader parameters
- **Automatic shader finding**: Scripts automatically locate the correct shaders

### Specific Issues Fixed:

#### Issue 1: Body Color Not Changing
**Problem:** Script was using enum values (0, 2, 4, 6) directly as array indices
**Solution:** Added proper mapping from enum values to array indices
```csharp
// Before (WRONG):
BodyMaterial.SetColor(shaderParameterName[editingColorParamenter], data.Colors[colorIdx].data);

// After (CORRECT):
int paramIndex = (editingColorParamenter == (int)CustomizeData.CustomizeItems.SHIRT_BASE_COLOR) ? 0 : 2;
BodyMaterial.SetColor(shaderParameterName[paramIndex], data.Colors[colorIdx].data);
```

#### Issue 2: Shoe Color Changing Entire Pants
**Problem:** Left/right tap materials were using shirt color parameter (`shaderParameterName[0]`) instead of sneaker color parameter
**Solution:** Fixed to use correct sneaker color parameter (`shaderParameterName[4]`)
```csharp
// Before (WRONG):
LeftTapMaterial.SetColor(shaderParameterName[0], data.Colors[idx].data);
RightTapMaterial.SetColor(shaderParameterName[0], data.Colors[idx].data);

// After (CORRECT):
LeftTapMaterial.SetColor(shaderParameterName[4], data.Colors[idx].data);
RightTapMaterial.SetColor(shaderParameterName[4], data.Colors[idx].data);
```

#### Issue 3: Very Light Body Appearance
**Problem:** Shader parameters were not being set correctly due to mapping issues
**Solution:** Fixed all parameter mappings and added debug logging

### New Shader Structure:

#### BodyClothing_BuiltIn.shader
- **`_BodyColor`**: Controls the base color of the body/shirt
- **`_PatternMask`**: Texture mask for patterns
- **`_PatternColor`**: Color applied to patterns
- **`_Shield`**: Shield texture
- **`_ShieldColor`**: Color applied to shield

#### LegsClothing_BuiltIn.shader
- **`_LegsColor`**: Controls the base color of the legs/pants
- **`_SneakerPattern`**: Texture for sneaker patterns
- **`_SneakerColor`**: Color applied to sneaker patterns

**Key Benefit:** No more parameter conflicts between body and legs materials!

### How to Use the Manual Shader Assignment:

#### Option 1: Runtime Script Assignment
1. **Add `ManualShaderAssigner` script** to any GameObject in your scene
2. **Assign materials**: Drag `PocBody.mat` and `PocLegs.mat` to the script fields
3. **Run the game** and press:
   - **B** key to assign body shader
   - **L** key to assign legs shader  
   - **A** key to assign both shaders

#### Option 2: Manual Material Assignment
1. **Select `PocBody.mat`** in the Project window
2. **In Inspector**: Change Shader to `Custom/BodyClothing_BuiltIn`
3. **Select `PocLegs.mat`** in the Project window
4. **In Inspector**: Change Shader to `Custom/LegsClothing_BuiltIn`

#### Option 3: Code Assignment
```csharp
// In any script
Material bodyMat = GetComponent<Renderer>().material;
Material legsMat = GetComponent<Renderer>().material;

bodyMat.shader = Shader.Find("Custom/BodyClothing_BuiltIn");
legsMat.shader = Shader.Find("Custom/LegsClothing_BuiltIn");
```

### Step-by-Step Fix Process:

#### Step 1: Convert Materials
1. Open Unity Editor
2. Go to `Tools > Convert Shaders to Built-in`
3. Click "Convert All Materials"
4. Check Console for success messages

#### Step 2: Verify Shader Assignment
- **PocBody.mat** should use `Custom/SimpleClothing_BuiltIn`
- **PocLegs.mat** should use `Custom/SimpleClothing_BuiltIn`
- Check Material Inspector for shader assignment

#### Step 3: Test Color Changes
1. Add `ColorTestDebugger` script to any GameObject
2. Assign body and legs materials in inspector
3. Press number keys 1,2,3 to test colors
4. Check Console for debug messages

#### Step 4: Test Clothing Store
1. Enter the clothing customization area
2. Try changing shirt colors
3. Try changing pattern colors
4. Try changing sneaker colors

### Debug Information:

#### Console Messages to Look For:
```
ColorTestDebugger initialized
Body material: PocBody with shader: Custom/SimpleClothing_BuiltIn
Legs material: PocLegs with shader: Custom/SimpleClothing_BuiltIn
Testing color change to: Red
Set body material _Color to Red
Set legs material _Color to Red
```

#### Material Inspector Check:
- **Shader:** Should show `Custom/SimpleClothing_BuiltIn`
- **Properties:** Should show all color and texture properties
- **Values:** Colors should be visible and editable

### Common Issues and Solutions:

#### Issue 1: Materials Still Pink
**Solution:** 
- Check if shader compiled successfully
- Verify shader name in Material Inspector
- Re-run ShaderConverter tool

#### Issue 2: Colors Not Changing
**Solution:**
- Verify material is using correct shader
- Check Console for SetColor debug messages
- Use ColorTestDebugger to test manually

#### Issue 3: Very Light Appearance
**Solution:**
- Check if `_Color` property is set to white (1,1,1,1)
- Verify texture assignments
- Check lighting settings

#### Issue 4: Pattern/Shield Not Visible
**Solution:**
- Verify texture assignments in Material Inspector
- Check if `_PatternColor` or `_ShieldColor` are set
- Use ColorTestDebugger to test texture changes

### Testing Checklist:

- [ ] Materials converted to `SimpleClothing_BuiltIn` shader
- [ ] ColorTestDebugger script added and working
- [ ] Number keys 1,2,3 change colors visibly
- [ ] Console shows debug messages
- [ ] Clothing store color changes work
- [ ] Pattern textures display correctly
- [ ] Shield textures display correctly
- [ ] Sneaker patterns display correctly

### If Still Not Working:

1. **Check Console Errors** - Look for shader compilation errors
2. **Verify Material Properties** - Ensure all properties are assigned
3. **Test with Simple Colors** - Use ColorTestDebugger with bright colors
4. **Check Lighting** - Ensure scene has proper lighting
5. **Rebuild Project** - Sometimes Unity needs a fresh build

### Final Notes:

The `SimpleClothing_BuiltIn` shader is designed to be:
- **Simple** - Easy to debug and understand
- **Compatible** - Works with Built-in Render Pipeline
- **Functional** - All clothing features work properly
- **Maintainable** - Easy to modify and extend

If you continue having issues, the debug script will help identify exactly where the problem is occurring.
