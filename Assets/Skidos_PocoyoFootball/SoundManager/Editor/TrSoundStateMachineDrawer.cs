#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SoundManager
{
	[CustomEditor(typeof(TrSoundStateMachine))]
	[CanEditMultipleObjects]
	public class TrSoundStateMachineDrawer : Editor {
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
			TrSoundStateMachine stSound = (TrSoundStateMachine)target;

			serializedObject.Update();

			if(stSound.soundData != null && stSound.soundData.voicesCollection.Count > 0)
			{
				EditorGUILayout.LabelField(new GUIContent("Sound ID:", "Select a a sound id"));

				m_ValidIDs.Clear();
				m_ValidIDs.Add(" < NONE > ");
				foreach (var voices in stSound.soundData.voicesCollection[0].narrations) {
					m_ValidIDs.Add(voices.stringId);
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
						int currentIndex = string.IsNullOrEmpty(stSound.sound[i]) ? 0 : m_ValidIDs.IndexOf(stSound.sound[i]);
						int newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
						if (newIndex > 0 && newIndex < m_ValidIDs.Count) {
							m_soundIDProperty.GetArrayElementAtIndex(i).stringValue = m_ValidIDs[newIndex];
						} else {
							m_soundIDProperty.GetArrayElementAtIndex(i).stringValue = string.Empty;
						}
					}
				}
			}
			{
				EditorGUILayout.HelpBox("Need to Assign a TrSoundsData with data", MessageType.Error);
			}

			DrawPropertiesExcluding(serializedObject, excludedFields);

			serializedObject.ApplyModifiedProperties();
		}
	}
}

#endif


