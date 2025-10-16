#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class ZinkiaUtils {

    private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
    private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    private const string kKnobPath = "UI/Skin/Knob.psd";
    private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";

    [MenuItem("GameObject/UI/Zinkia/Slider Lerp")]
    private static void CreateComponentUITrText()
    {
        GameObject go = new GameObject("SliderLerp", typeof(RectTransform), typeof(Slider), typeof(SliderLerp));
        go.transform.SetParent(Selection.activeGameObject.transform);

        //Background Image
        GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
        background.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        background.GetComponent<Image>().type = Image.Type.Sliced;
        background.GetComponent<Image>().fillCenter = true;
        background.transform.SetParent(go.transform);

        //FillArea
        GameObject fillArea = new GameObject("Fill Area", typeof(RectTransform));
        fillArea.transform.SetParent(go.transform);

        //Hijo Fill Area
        GameObject fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        fill.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        fill.GetComponent<Image>().type = Image.Type.Sliced;
        fill.GetComponent<Image>().fillCenter = true;
        fill.transform.SetParent(fillArea.transform);

        //handleArea
        GameObject handleArea = new GameObject("Handle Slide Area", typeof(RectTransform));
        handleArea.transform.SetParent(go.transform);

        //hijo handleArea
        GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
        handle.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
        handle.transform.SetParent(handleArea.transform);

        //añadimos el target graphic, fill rect y handle rect al padre
        go.GetComponent<Slider>().targetGraphic = handle.GetComponent<Image>();
        go.GetComponent<Slider>().fillRect = fill.GetComponent<RectTransform>();
        go.GetComponent<Slider>().handleRect = handle.GetComponent<RectTransform>();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/UI/Zinkia/Slider Lerp", true)]
    private static bool ValidateComponentUITrText()
    {
        return (Selection.activeGameObject != null) && (Selection.activeGameObject.GetComponent<RectTransform>() != null);
    }
}

#endif

