#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxDrawer : PropertyDrawer {

    HelpBoxAttribute helpBoxAttribute { get { return ((HelpBoxAttribute)attribute); } }

    // Here you must define the height of your property drawer. Called by Unity.
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        return base.GetPropertyHeight(prop, label) + helpBoxAttribute.helpHeight;
    }

    // Here you can define the GUI for your property drawer. Called by Unity.
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.PropertyField(position, prop, label, true);

        // Adjust the help box position to appear indented underneath the text field.
        Rect helpPosition = EditorGUI.IndentedRect(position);
        helpPosition.y += base.GetPropertyHeight(prop, label);
        helpPosition.height = helpBoxAttribute.helpHeight;
        DrawHelpBox(helpPosition, prop);
    }

    void DrawHelpBox(Rect position, SerializedProperty prop)
    {
        EditorGUI.HelpBox(position, helpBoxAttribute.helpMessage, MessageType.Info);
    }
}


#endif