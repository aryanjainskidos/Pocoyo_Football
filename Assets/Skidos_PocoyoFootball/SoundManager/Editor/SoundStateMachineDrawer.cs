#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SoundManager
{
	[CustomEditor(typeof(SoundStateMachine))]
	[CanEditMultipleObjects]
	public class SoundStateMachineDrawer : Editor {
		private static readonly string[] excludedFields = new string[] { "m_Script" };

		private List<string> m_ValidIDs = new List<string>();
		private SerializedProperty m_soundIDProperty;
		private int index = 0;

		public void OnEnable()
		{
			m_soundIDProperty = serializedObject.FindProperty("sound");
		}

		public override void OnInspectorGUI()
		{
			SoundStateMachine stSound = (SoundStateMachine)target;

			serializedObject.Update();

			if(stSound.soundData != null && stSound.soundData.sounds.Count > 0)
			{

				m_ValidIDs.Clear();
				m_ValidIDs.Add(" < NONE > ");
				foreach (var sound in stSound.soundData.sounds) {
					m_ValidIDs.Add(sound.name);
				}

				index = m_soundIDProperty.arraySize;
				index = EditorGUILayout.IntField("Size:",index);

				if(index != m_soundIDProperty.arraySize)
				{
					while(index > m_soundIDProperty.arraySize)
						m_soundIDProperty.InsertArrayElementAtIndex(m_soundIDProperty.arraySize);
					while(index < m_soundIDProperty.arraySize)
						m_soundIDProperty.DeleteArrayElementAtIndex(m_soundIDProperty.arraySize-1);
				}

				if(m_soundIDProperty.arraySize > 0)
				{
					EditorGUILayout.LabelField(new GUIContent("Sound ID:", "Select a sound id"));

					for(int i = 0; i < stSound.sound.Count; i++)
					{
						int currentIndex = m_soundIDProperty.GetArrayElementAtIndex(i).intValue + 1;;
						int newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
						if (newIndex > 0 && newIndex < m_ValidIDs.Count) {
							m_soundIDProperty.GetArrayElementAtIndex(i).intValue = newIndex - 1;;
						} else {
							m_soundIDProperty.GetArrayElementAtIndex(i).intValue = -1;
						}
					}
				}
			}
			else
			{
				EditorGUILayout.HelpBox("Need to Assign a TrSoundsData with data", MessageType.Error);
			}

			DrawPropertiesExcluding(serializedObject, excludedFields);

			serializedObject.ApplyModifiedProperties();
		}
	}
}

#endif


