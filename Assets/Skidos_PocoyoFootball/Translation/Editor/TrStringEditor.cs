#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(TrString))]
[CanEditMultipleObjects]
public class TrStringEditor : Editor {

	private static readonly string[] excludedFields = new string[] { "strings" };

	private SerializedProperty property;
	private ReorderableList list;
	private AnimBool IsExpanded;

	public void OnEnable()
	{
		property = serializedObject.FindProperty("strings");

		IsExpanded = new AnimBool(property.isExpanded);
		IsExpanded.speed = 1f;

		list = new ReorderableList(serializedObject, property, false, true, true, true);
		list.drawHeaderCallback += rect => property.isExpanded = EditorGUI.ToggleLeft(rect, property.displayName, property.isExpanded, EditorStyles.boldLabel);
		list.onCanRemoveCallback += (list) => { return list.count > 0; };
		list.drawElementCallback += this.drawElement;
		list.onAddCallback += this.addElement;
		list.elementHeightCallback += (idx) => { return Mathf.Max(EditorGUIUtility.singleLineHeight, EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(idx), GUIContent.none, true)) + 4.0f; };

	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		DrawPropertiesExcluding(serializedObject, excludedFields);

		IsExpanded.target = property.isExpanded;
		if ((!IsExpanded.value && !IsExpanded.isAnimating) || (!IsExpanded.value && IsExpanded.isAnimating))
		{
			EditorGUILayout.BeginHorizontal();
			property.isExpanded = EditorGUILayout.ToggleLeft(string.Format("{0}[]", property.displayName), property.isExpanded, EditorStyles.boldLabel);
			EditorGUILayout.LabelField(string.Format("size: {0}", property.arraySize));
			EditorGUILayout.EndHorizontal();
		}
		else {
			if (EditorGUILayout.BeginFadeGroup(IsExpanded.faded))
			{				
				list.DoLayoutList();
			}
			EditorGUILayout.EndFadeGroup();
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void drawElement(Rect rect, int index, bool active, bool focused)
	{
		EditorGUI.indentLevel ++;
		if (property.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Generic)
		{
			EditorGUI.LabelField(rect, property.GetArrayElementAtIndex(index).displayName);
		}	
		rect.height = EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index), GUIContent.none, true);
		rect.y += 1;
		EditorGUI.PropertyField(rect, property.GetArrayElementAtIndex(index), GUIContent.none, true);
		this.list.elementHeight = rect.height + 4.0f;
		EditorGUI.indentLevel --;
	}

	private void addElement(ReorderableList l)
	{  
		var index = l.serializedProperty.arraySize;
		l.serializedProperty.arraySize++;
		l.index = index;
		var element = l.serializedProperty.GetArrayElementAtIndex(index);
		element.FindPropertyRelative("OriginalText").stringValue = string.Empty;
		element.FindPropertyRelative("stringId").stringValue = Translation.NextStringId();
	}
}

#endif
