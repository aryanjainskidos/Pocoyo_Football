#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(TrTextAttribute))]
public class TrTextDrawer : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight (property) * 2;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		if( property.propertyType == SerializedPropertyType.String)
		{
			string original = TranslationOriginalString( property.stringValue );
			EditorGUI.PropertyField(new Rect( position.x, position.y, position.width, position.height/2), property, label, true);
			if(original != null)
			{
				EditorGUI.LabelField( new Rect( position.x, position.y + position.height/2, position.width, position.height/2), "Original Text", original);
			}
			else
			{
				EditorGUI.HelpBox( new Rect( position.x, position.y + position.height/2, position.width, position.height/2), "Need use the translation tool to create the string id", MessageType.Info);
			}
		}
		else
		{
			EditorGUI.HelpBox(position, "To use the TrText attribute " + label.text + " must be an string!", MessageType.Error);
		}
	}

	//Check in translation ScriptableObject
	private static TranslationData translationData;
	private string TranslationOriginalString( string id ) {
		if(!translationData)
			translationData = AssetDatabase.LoadAssetAtPath("Assets/Resources/languages.asset", typeof(TranslationData)) as TranslationData;

		if(!translationData)
			return null;

		int fidx = translationData.OriginalText.translations.FindIndex( x => ( x.stringId == id ) );
		if( fidx > -1 )
			return translationData.OriginalText.translations[fidx].translation;
		return null;
	}
}


#endif