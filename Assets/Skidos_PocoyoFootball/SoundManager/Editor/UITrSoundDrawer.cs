#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UnityEditorInternal;
using UnityEditor.AnimatedValues;

namespace SoundManager
{
	[CustomEditor(typeof(UITrSound))]
	[CanEditMultipleObjects]
	public class UITrSoundDrawer : Editor {
		private static readonly string[] excludedFields = new string[] { "soundId" };

		private List<string> m_ValidIDs = new List<string>();

		private SerializedProperty property;
		private ReorderableList list;
		private AnimBool IsExpanded;

		public void OnEnable()
		{
			property = serializedObject.FindProperty("soundId");

			IsExpanded = new AnimBool(property.isExpanded);
			IsExpanded.speed = 1f;

			list = new ReorderableList(serializedObject, property, false, true, true, true);
			list.drawHeaderCallback += rect => property.isExpanded = EditorGUI.ToggleLeft(rect, property.displayName, property.isExpanded, EditorStyles.boldLabel);
			list.onCanRemoveCallback += (list) => { return list.count > 0; };
			list.drawElementCallback += this.drawElement;
			list.onAddCallback += this.addElement;
			list.elementHeightCallback += (idx) => { return EditorGUIUtility.singleLineHeight; };
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawPropertiesExcluding(serializedObject, excludedFields);

			UITrSound uiSound = (UITrSound)target;

			if(uiSound.soundData != null && uiSound.soundData.voicesCollection.Count > 0)
			{
				m_ValidIDs.Clear();
				m_ValidIDs.Add(" < NONE > ");
				foreach (var voices in uiSound.soundData.voicesCollection[0].narrations) {
					m_ValidIDs.Add(voices.stringId);
				}

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
			}
			else
			{
				EditorGUILayout.HelpBox("Need to Assign a SoundsData with data", MessageType.Error);
			}

			serializedObject.ApplyModifiedProperties();
		}
	
		private void drawElement(Rect rect, int index, bool active, bool focused)
		{
			EditorGUI.indentLevel ++;
			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 1;

			string currentString = property.GetArrayElementAtIndex(index).stringValue;

			int currentIndex = string.IsNullOrEmpty(currentString) ? 0 : m_ValidIDs.IndexOf(currentString);

			int newIndex = EditorGUI.Popup(rect, currentIndex, m_ValidIDs.ToArray());

			if (newIndex > 0 && newIndex < m_ValidIDs.Count) {
				property.GetArrayElementAtIndex(index).stringValue = m_ValidIDs[newIndex];
			} else {
				property.GetArrayElementAtIndex(index).stringValue = string.Empty;
			}

			this.list.elementHeight = rect.height + EditorGUIUtility.singleLineHeight;
			EditorGUI.indentLevel --;
		}

		private void addElement(ReorderableList l)
		{  
			var index = l.serializedProperty.arraySize;
			l.serializedProperty.arraySize++;
			l.index = index;
			var element = l.serializedProperty.GetArrayElementAtIndex(index);
			element.stringValue = string.Empty;
		}

	}
}


#endif


