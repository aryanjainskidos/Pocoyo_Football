using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrScriptableObject : ScriptableObject {
#if UNITY_EDITOR
	private TranslationData translationData;

	[ContextMenu("Update Translations")]
	public void UpdateTranslationFromEditor()
    {
		TrUpdate();
		UnityEditor.EditorUtility.SetDirty(this);
		UnityEditor.EditorUtility.SetDirty(translationData);
	}

	public void TrUpdate() {
		translationData = Resources.Load<TranslationData>("languages") as TranslationData;
		if (translationData == null)
		{
			Debug.Log("TrScriptableObject " + this.GetType().Name  + ": Try update without TranslationData");
			return;
		}

		ProcessTrTextFields(this.GetType(), this);
	}

    private string ProcessString(string stringId)
    {
		string GroupId = this.GetType().Name;
		int fidx = translationData.OriginalText.translations.FindIndex(x => (/*x.grougId == GroupId &&*/ x.stringId == stringId));
		if (fidx > -1) //Is already an id string
        {
			//Check and add to translations
            TranslationData.TranslationEntry entry = new TranslationData.TranslationEntry();
			entry.grougId = GroupId;
            entry.stringId = stringId;
            entry.translation = "";
            foreach (var lang in translationData.Languages)
            {
                if (!lang.translations.Exists(x => (/*x.grougId == entry.grougId &&*/ x.stringId == entry.stringId)))
                    lang.translations.Add(entry);
            }
        }
        else //This is not a id string
        {
            string original = stringId;

			stringId = Translation.NextStringId();

            TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
			newEntry.grougId = GroupId;
            newEntry.stringId = stringId;
            newEntry.translation = original;

            translationData.OriginalText.translations.Add(newEntry);

            newEntry.translation = "";
            foreach (var lang in translationData.Languages)
            {
                lang.translations.Add(newEntry);
            }
        }
        return stringId;
    }

	private object ProcessTrTextFields(System.Type type, object instance) {		
		foreach(System.Reflection.FieldInfo mInfo in type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)) {
			//The field is a string
			if(mInfo.FieldType == typeof(System.String) )
			{
				if( System.Attribute.GetCustomAttributes(mInfo, typeof(TrTextAttribute) ).Length > 0)
				{
					string stringId = mInfo.GetValue(instance) as string;
					stringId = ProcessString(stringId);
					mInfo.SetValue(instance, stringId);
				}
			}
			else if(mInfo.FieldType.IsArray)
			{
				System.Array array =  mInfo.GetValue(instance) as System.Array;
				int i = 0; 
				foreach(var value in array)
				{
					if(value.GetType() == typeof(System.String) )
					{
						if(System.Attribute.GetCustomAttributes(mInfo, typeof(TrTextAttribute)).Length > 0)
						{
							string stringId = value as string;
							stringId = ProcessString(stringId);
							array.SetValue(stringId, i);
						}
					}
					else
					{
						System.Type type2 = value.GetType();
						object obj = ProcessTrTextFields(type2, value);
						array.SetValue(obj, i);
					}
					i++;
					if(i == 43)
						Debug.Log("Stop");
				}
			}
			else if( mInfo.FieldType.IsNested )
			{
				//System.Type classType = mInfo.GetType();
				object obj = mInfo.GetValue(instance);
				if(obj != null) {
					obj = ProcessTrTextFields(obj.GetType(), obj);
					mInfo.SetValue(instance, obj);
				}
			}
			else if( mInfo.FieldType.IsClass )
			{
				//System.Type classType = mInfo.GetType();
				object obj = mInfo.GetValue(instance);
				if(obj != null) {
					obj = ProcessTrTextFields(obj.GetType(), obj);
					mInfo.SetValue(instance, obj);
				}
			}
		}

		return instance;
	}
#endif
}

