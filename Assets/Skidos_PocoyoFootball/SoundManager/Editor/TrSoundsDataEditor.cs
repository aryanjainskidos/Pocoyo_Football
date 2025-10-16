#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEditor;
using UnityEditor.SceneManagement;

using System.IO; // File.Exists()
using NPOI.HSSF.UserModel; // XSSFWorkbook, XSSFSheet

namespace SoundManager
{
	[CustomEditor(typeof(TrSoundsData))]
	public class TrSoundsDataEditor : Editor {

		bool foloutOriginalData = false;
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			TrSoundsData trSoundData = (TrSoundsData)target;

			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField( "Number of Entries: " + (trSoundData.voicesCollection.Count > 0 ? trSoundData.voicesCollection[0].narrations.Count : 0 ));

			EditorGUILayout.LabelField( "Languages: ");
			foreach( var entry in trSoundData.voicesCollection )
			{
				EditorGUILayout.LabelField( "\t" + entry.languageId + " (" + entry.narrations.Count + ")" );
			}
			EditorGUILayout.Space();
			if( GUILayout.Button("Import from xls") )
			{
				Debug.Log("importing from xml");
				ImportExcel();
				EditorUtility.SetDirty(trSoundData);
			}

			EditorGUILayout.EndVertical();

			//DrawPropertiesExcluding(serializedObject, excludedFields);
			foloutOriginalData = EditorGUILayout.Foldout( foloutOriginalData, "Default Inspector" );
			if( foloutOriginalData ) {
				EditorGUILayout.Space();
				DrawPropertiesExcluding(serializedObject);
			}

			serializedObject.ApplyModifiedProperties();
		}

		HSSFWorkbook wb;
		HSSFSheet sh;
		void ImportExcel() {		
			string path = EditorUtility.OpenFilePanel("Import To Excel", "", "xls");
			if (path.Length != 0)
			{
				using (var fs = new FileStream (path, FileMode.Open, FileAccess.Read))
				{
					wb = new HSSFWorkbook (fs);
					sh = (HSSFSheet)wb.GetSheetAt(0);

					TrSoundsData trSoundData = (TrSoundsData)target;
					trSoundData.voicesCollection.Clear();

					HSSFRow labelRow = (HSSFRow)sh.GetRow (0);
					for(int c = 0; c < sh.GetRow (0).Cells.Count; c++)
					{
						TrSoundsData.Narration newNarrator = new TrSoundsData.Narration();
						newNarrator.languageId = labelRow.GetCell(c).StringCellValue;
						newNarrator.narrations = new List<TrSoundsData.AudioEntry>();
						trSoundData.voicesCollection.Add( newNarrator );
					}

					for(int r = 1; r <= sh.LastRowNum; r++){
						if(sh.GetRow (r) == null) continue;
						if (sh.GetRow (r).Cells.Count > 0) {
							if(sh.GetRow (r).GetCell(0) == null)
								continue;
							var audioId = sh.GetRow (r).GetCell (0).StringCellValue;

							//Is a filepath
							if(audioId.StartsWith("Assets/"))
                            {
								string filename = Path.GetFileNameWithoutExtension(audioId);
								string[] paths = audioId.Split("/"[0]);

								if (path.Length > 2)
								{
									audioId = paths[paths.Length - 2] + "/" + filename;
								}
								else
									audioId = filename;
                            }

							for(int c = 0; c < sh.GetRow (0).Cells.Count; c++)
							{
								var langId = labelRow.GetCell(c).StringCellValue;

								if( c > sh.GetRow (r).Cells.Count - 1)
								{
									Debug.Log("El nombre en lenguaje:[" + langId + "] para el id:[" + audioId + "] no tiene una celda valida en el archivo xls");
									Debug.Log(">> Insertando un hueco en lenguaje:[" + langId + "] para el id:[" + audioId + "]");
									InsertNewClip( audioId, langId, null);
									continue;
								}

								var audioName = sh.GetRow (r).GetCell (c).StringCellValue;

								if(string.IsNullOrEmpty( audioName ))
								{
									Debug.LogError("El nombre en lenguaje:[" + langId + "] para el id:[" + audioId + "] esta vacio o es null");
									Debug.Log(">> Insertando un hueco en lenguaje:[" + langId + "] para el id:[" + audioId + "]");
									InsertNewClip( audioId, langId, null);
									continue;
								}

								if(audioName.StartsWith("Assets/"))
                                {
									AudioClip audioClip = AssetDatabase.LoadAssetAtPath(audioName, typeof(AudioClip)) as AudioClip;
									InsertNewClip(audioId, langId, audioClip);
									continue;
                                }
								
								//Find audioclip by name
								string[] audiosGUID = AssetDatabase.FindAssets( audioName );
								if( audiosGUID.Length != 1 )
								{
									Debug.LogError("El nombre " + audioName + " no existe o esta duplicado y no puede usarse");
									Debug.Log(">> Insertando un hueco en lenguaje:[" + langId + "] para el id:[" + audioId + "]");
									InsertNewClip( audioId, langId, null);
								}
								else
								{
									AudioClip audioClip = AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath(audiosGUID[0]), typeof(AudioClip ) ) as AudioClip;
									InsertNewClip( audioId, langId, audioClip);
								}
							}
						}
					}
				}
			}
		}

		void InsertNewClip( string audioId, string langId, AudioClip audioClip)
		{
			TrSoundsData trSoundData = (TrSoundsData)target;

			int voicesIdx = trSoundData.voicesCollection.FindIndex(x => x.languageId == langId);
			if(voicesIdx > -1)
			{	
				TrSoundsData.Narration voices = trSoundData.voicesCollection[voicesIdx];
				TrSoundsData.AudioEntry newEntry = new TrSoundsData.AudioEntry();
				newEntry.stringId = audioId;
				newEntry.voice = audioClip;
				voices.narrations.Add(newEntry);
				trSoundData.voicesCollection[voicesIdx] = voices;
			}
		}
	}
}


#endif