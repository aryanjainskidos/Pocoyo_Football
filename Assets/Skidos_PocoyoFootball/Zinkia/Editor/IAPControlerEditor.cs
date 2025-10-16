#if UNITY_PURCHASING
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEditor.Purchasing;
using System.Collections.Generic;

namespace Zinkia
{
	[CustomEditor(typeof(IAPControler))]
	[CanEditMultipleObjects]
	public class IAPControlerEditor : Editor
	{
		private static readonly string[] excludedFields = new string[] { "m_Script" };
		private const string kNoProduct = "<None>";

		private List<string> m_ValidIDs = new List<string>();
		private SerializedProperty m_ProductIDProperty;

		public void OnEnable()
		{
			m_ProductIDProperty = serializedObject.FindProperty("productId");
		}

		public override void OnInspectorGUI()
		{
			IAPControler button = (IAPControler)target;

			serializedObject.Update();

			EditorGUILayout.LabelField(new GUIContent("Product ID:", "Select a product from the IAP catalog"));

			var catalog = ProductCatalog.LoadDefaultCatalog();

			m_ValidIDs.Clear();
			m_ValidIDs.Add(kNoProduct);
			foreach (var product in catalog.allProducts) {
				m_ValidIDs.Add(product.id);
			}

			int currentIndex = string.IsNullOrEmpty(button.productId) ? 0 : m_ValidIDs.IndexOf(button.productId);
			int newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
			if (newIndex > 0 && newIndex < m_ValidIDs.Count) {
				m_ProductIDProperty.stringValue = m_ValidIDs[newIndex];
			} else {
				m_ProductIDProperty.stringValue = string.Empty;
			}

			if (GUILayout.Button("IAP Catalog...")) {
				ProductCatalogEditor.ShowWindow();
			}

			DrawPropertiesExcluding(serializedObject, excludedFields);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif
