#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

using System.IO; // File.Exists()
using NPOI.HSSF.UserModel; // XSSFWorkbook, XSSFSheet

public class TranslationEditor: EditorWindow {

	[MenuItem("Window/Translation")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(TranslationEditor));
	}

	public TranslationData translationData;

	public enum LanguagesIDS {
		Spanish,
		English,
		Portuguese,
		ChineseSimplified,
		Japanese,
		Korean,
		French,
		Italian,
		German,
		Russian,
		Turkish,
		Indonesian,
		Neutral,
	}

	private LanguagesIDS NewLanguage = LanguagesIDS.English;

	private List<string> groupIds;
	private List<string> translationIds;
	private List<string> stringIds;

	//GUI Variables
	private Rect leftTopPanel;
	private Rect vResizer;
	private Rect leftBottomPanel;
	private Rect hResizer;
	private Rect rigthPanel;

	private float vSizeRation = 0.5f;
	private bool vResizing;
	private float hSizeRation = 0.25f;
	private bool hResizing;

	private GUIStyle resizerStyle;

	private float leftWidth;
	private float rigthWidth;
	private float topHeigth;
	private float bottomHeigth;
	private float leftX;
	private float rigthX;
	private float topY;
	private float bottomY;

	void OnEnable() {
		resizerStyle = new GUIStyle();
		resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;

		groupIds = new List<string>();
		stringIds = new List<string>();
		translationIds = new List<string>();

		if(translationData == null)
		{
			translationData = AssetDatabase.LoadAssetAtPath("Assets/Resources/languages.asset", typeof(TranslationData)) as TranslationData;
			if(translationData) {
				foreach(var entry in translationData.OriginalText.translations)
				{
					if( !groupIds.Exists( x => x == entry.grougId ) )
						groupIds.Add(entry.grougId);
				}
			}
			else {
				translationData = CreateInstance<TranslationData>();
				translationData.OriginalText = new TranslationData.Language();
				translationData.OriginalText.translations= new List<TranslationData.TranslationEntry>();
				translationData.Languages = new List<TranslationData.Language>();
				AssetDatabase.CreateAsset(translationData, "Assets/Resources/languages.asset");
			}
		}
	}

	void OnGUI() 
	{
		if(groupIds.Count == 0) {
			Debug.Log("No groupIds");
		}

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Translation Editor", EditorStyles.boldLabel);
		if (GUILayout.Button("Add String")) 
			AddString();
		if (GUILayout.Button("Update Translations")) 
			UpdateTranslations();

		NewLanguage = (LanguagesIDS)EditorGUILayout.EnumPopup(NewLanguage);
		if (GUILayout.Button("Add Language")) 
			AddLanguage();

		if (GUILayout.Button("Remove Language")) 
			DelLanguage();

		if (GUILayout.Button("Export Excel")) 
			ExportExcel();

		if (GUILayout.Button("Import Excel")) 
			ImportExcel();
		

		GUILayout.EndHorizontal ();

		leftWidth = (position.width * hSizeRation);
		rigthWidth = (position.width * (1f - hSizeRation));
		topHeigth = (position.height * vSizeRation);
		bottomHeigth = position.height * (1.0f - vSizeRation);

		leftX = 0f;
		rigthX = (position.width * hSizeRation);
		topY = (EditorGUIUtility.singleLineHeight*2f);
		bottomY = (EditorGUIUtility.singleLineHeight*2f) + (position.height * vSizeRation);

		DrawLeftTopPanel();
		DrawVResizePanel();
		DrawLeftBottomPanel();
		DrawHResizePanel();
		DrawRightPanel();

		ProcessEvents(Event.current);

		if (GUI.changed) 
		{
			Repaint();
			if(translationData != null)
				EditorUtility.SetDirty(translationData);
		}
	}

	private void UpdateLists() {
		selected = -1;
		stringIndex = -1;
		stringID = "";
		groupIds.Clear();
		stringIds.Clear();
		translationIds.Clear();

		foreach(var entry in translationData.OriginalText.translations)
		{
			if( !groupIds.Exists( x => x == entry.grougId ) )
				groupIds.Add(entry.grougId);
		}
	}

	//Groups(scene name, file name)
	private int selected = -1;
	private Vector2 scrollPos = new Vector2(0,0);
	private void DrawLeftTopPanel()
	{
		leftTopPanel = new Rect(leftX, topY, leftWidth, topHeigth);

		GUILayout.BeginArea(leftTopPanel);
		GUILayout.Box("Groups", GUILayout.Width(leftWidth) );
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(topHeigth) );
		int prev_selected = selected;
		for(int i = 0; i <  groupIds.Count; ++i)
		{
			if( GUILayout.Toggle((selected == i), groupIds[i], (selected == i) ? EditorStyles.boldLabel : EditorStyles.label) )
				selected = i;
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();

		if(prev_selected != selected) {
			//Clear and fill with group strings
			stringIndex = -1;
			stringID ="";
			stringIds.Clear();
			translationIds.Clear();
			string groupId = groupIds[selected];
			foreach( var entry in translationData.OriginalText.translations)
			{
				if(entry.grougId == groupId)
				{
					translationIds.Add(entry.translation);
					stringIds.Add(entry.stringId);
				}
			}
		}
	}

	//Original Strings
	private int stringIndex = -1;
	private Vector2 scrollPos2 = new Vector2(0,0);
	private void DrawLeftBottomPanel()
	{
		leftBottomPanel = new Rect(leftX, bottomY + 5f, leftWidth, bottomHeigth - 5f );

		GUILayout.BeginArea(leftBottomPanel);
		scrollPos2 = GUILayout.BeginScrollView(scrollPos2, true, true, GUILayout.Height(bottomHeigth - 35f));

		GUILayout.BeginHorizontal();
		GUILayout.Label("Original Strings", GUILayout.Width(leftWidth/2) );
		foreach( var language in translationData.Languages)
		{
			Color prev = GUI.color;
			GUI.color = languageColor(language.languageId);
			GUILayout.Label(language.languageId, GUILayout.Width(20));
			GUI.color = prev;
		}
		GUILayout.EndHorizontal();

		int prev_selected = stringIndex;
		bool remove_text = false;
		for(int i = 0; i < translationIds.Count; ++i)
		{
			GUILayout.BeginHorizontal();
			if( GUILayout.Button( "X", GUILayout.Width(EditorGUIUtility.singleLineHeight) ) )
			{
				stringIndex = i;
				remove_text = true;
				Debug.Log("Remove " + stringIndex.ToString());
			}	
			if( GUILayout.Toggle((stringIndex == i), translationIds[i], (stringIndex == i) ? EditorStyles.boldLabel : EditorStyles.label, GUILayout.Width(leftWidth/2 - EditorGUIUtility.singleLineHeight) ) )
				stringIndex = i;

			foreach( var language in translationData.Languages)
			{
				Color prev = GUI.backgroundColor;
				GUI.backgroundColor = languageColor(language.languageId);
				foreach( var entry in language.translations )
					if( entry.stringId == stringIds[i] )
						EditorGUILayout.Toggle( entry.translation.Length != 0, GUILayout.Width(20) );
				GUI.backgroundColor = prev;
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.EndScrollView();
		GUILayout.EndArea();

		if(prev_selected != stringIndex) {
			if(remove_text)
			{
				Debug.Log("Removing " + stringIndex.ToString());
				int fidx = translationData.OriginalText.translations.FindIndex( x => ( x.grougId == groupIds[selected] && x.stringId == stringIds[stringIndex] ) );
				if( fidx > -1 )
					translationData.OriginalText.translations.RemoveAt(fidx);

				foreach(var lang in translationData.Languages)
				{
					int idx = lang.translations.FindIndex( x => ( x.grougId == groupIds[selected] && x.stringId == stringIds[stringIndex] ) );
					if( idx > -1 )
						lang.translations.RemoveAt(idx);
				}
					
				EditorUtility.SetDirty(translationData);
				UpdateLists();
			}
			else
				stringID = stringIds[stringIndex];
		}
	}

	//Translation
	private string stringID ="";
	private Vector2 scrollPos3 = new Vector2(0,0);
	private void DrawRightPanel()
	{
		rigthPanel = new Rect(rigthX + 5f, topY, rigthWidth - 5f, position.height );

		GUILayout.BeginArea(rigthPanel);
		scrollPos3 = GUILayout.BeginScrollView(scrollPos3, true, true, GUILayout.Height(position.height - 35f));

		for( int t = 0; t < translationData.OriginalText.translations.Count; t++)
		//foreach( var entry in translationData.OriginalText.translations )
		{
			var entry = translationData.OriginalText.translations[t];
			if( entry.stringId == stringID )
			{
				EditorGUILayout.LabelField("Original Strings" + " (" + entry.stringId + ")" );
				EditorGUILayout.BeginHorizontal();
				entry.translation = EditorGUILayout.TextField( entry.translation );
				if(GUILayout.Button("Copy list"))
					EditorGUIUtility.systemCopyBuffer = "\"" + entry.stringId + "\", //" + entry.translation;
				if(GUILayout.Button("Copy alone"))
					EditorGUIUtility.systemCopyBuffer = "\"" + entry.stringId + "\"; //" + entry.translation;
				EditorGUILayout.EndHorizontal();
				translationData.OriginalText.translations[t] = entry;
			}
		}

		EditorGUI.BeginChangeCheck();

		if(selected > -1 && stringID.Length > 0)
		{
			for(int l = 0; l < translationData.Languages.Count; l++)
			//foreach( var language in translationData.Languages)
			{
				var language = translationData.Languages[l];

				Color prev = GUI.color;
				GUI.color = languageColor(language.languageId);
				GUI.Box( new Rect( GUILayoutUtility.GetLastRect().xMin, GUILayoutUtility.GetLastRect().yMax, rigthWidth - 5f, EditorGUIUtility.singleLineHeight * 3.5f ), "" );
				GUI.color = prev;

				EditorGUILayout.LabelField( language.languageId );
				for( int t = 0; t < language.translations.Count; t++)
				//foreach( var entry in language.translations )
				{
					var entry = language.translations[t];
					if( entry.stringId == stringID )
					{
						entry.translation = EditorGUILayout.TextField( entry.translation, GUILayout.Height( EditorGUIUtility.singleLineHeight * 2.5f) );
						language.translations[t] = entry;
					}
				}

				translationData.Languages[l] = language;
			}
		}
		if( EditorGUI.EndChangeCheck() )
			EditorUtility.SetDirty(translationData);

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	#region ResizeMethods
	private void DrawVResizePanel() {
		vResizer = new Rect(leftX, bottomY - 5f, leftWidth, 10f);

		GUILayout.BeginArea(vResizer,resizerStyle);
		GUILayout.EndArea();

		EditorGUIUtility.AddCursorRect(vResizer, MouseCursor.ResizeVertical);
	}

	private void DrawHResizePanel() {
		hResizer = new Rect(rigthX - 5f, topY, 10f, position.height);

		GUILayout.BeginArea(hResizer,resizerStyle);
		GUILayout.EndArea();

		EditorGUIUtility.AddCursorRect(hResizer, MouseCursor.ResizeHorizontal);
	}

	private void ProcessEvents(Event e)
	{
		switch(e.type)
		{
		case EventType.MouseDown:
			if( e.button == 0)
			{
				if(vResizer.Contains(e.mousePosition))
					vResizing = true;
				if(hResizer.Contains(e.mousePosition))
					hResizing = true;
			}
			break;

		case EventType.MouseUp:
			hResizing = false;
			vResizing = false;
			break;
		}

		Resize(e);
	}

	private void Resize(Event e)
	{
		if(hResizing)
		{
			hSizeRation = e.mousePosition.x / position.width;
			if(hSizeRation < 0.20f)
				hSizeRation = 0.20f;
			if(hSizeRation > 0.80f)
				hSizeRation = 0.80f;
			Repaint();
		}

		if(vResizing)
		{
			vSizeRation = (e.mousePosition.y - topY) / position.height;
			if(vSizeRation < 0.25f)
				vSizeRation = 0.25f;
			if(vSizeRation > 0.75f)
				vSizeRation = 0.75f;
			Repaint();
		}
	}
	#endregion
	void AddString() {
		NewStringEditor window = CreateInstance<NewStringEditor>();
		window.position = new Rect(Screen.width/2, Screen.height/2, 250, 150);
		window.ShowPopup();
	}

	void UpdateTranslations() {
		if(translationData == null)
		{
			translationData = AssetDatabase.LoadAssetAtPath("Assets/Resources/languages.asset", typeof(TranslationData)) as TranslationData;
			if(translationData) {
				foreach(var entry in translationData.OriginalText.translations)
				{
					if( !groupIds.Exists( x => x == entry.grougId ) )
						groupIds.Add(entry.grougId);
				}
			}
			else {
				translationData = CreateInstance<TranslationData>();
				translationData.OriginalText = new TranslationData.Language();
				translationData.OriginalText.translations= new List<TranslationData.TranslationEntry>();
				translationData.Languages = new List<TranslationData.Language>();
				AssetDatabase.CreateAsset(translationData, "Assets/Resources/languages.asset");
			}
		}
		//Save current project scene
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

		//Prefabs with Translate label object
		//var guids = AssetDatabase.FindAssets("l:Translate");
		//foreach (var guid in guids)
		//{
		//	string assetPath = AssetDatabase.GUIDToAssetPath(guid);
		//	Debug.Log("open " + assetPath);
		//	GameObject so = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

		//	var trTextList = so.GetComponentsInChildren(typeof(TrText), true) as TrText[];
		//	foreach (var trtext in trTextList)
		//	{
		//		string groupId = trtext.groupId;

		//		Text originalText = trtext.gameObject.GetComponent<Text>();
		//		string originalStr = originalText.text;

		//		if (!translationData.OriginalText.translations.Exists(x => (x.grougId == groupId && x.translation == originalStr && x.stringId == trtext.stringId)))
		//		{
		//			TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
		//			newEntry.grougId = groupId;
		//			newEntry.stringId = trtext.stringId;
		//			newEntry.translation = originalStr;
		//			translationData.OriginalText.translations.Add(newEntry);

		//			newEntry.translation = "";
		//			foreach (var language in translationData.Languages)
		//			{
		//				language.translations.Add(newEntry);
		//			}
		//		}
		//	}

		//	var trStringList = so.GetComponentsInChildren(typeof(TrString), true) as TrString[];
		//	foreach (var trstring in trStringList)
		//	{
		//		string groupId = trstring.groupId;

		//		foreach (var str in trstring.strings)
		//		{
		//			string originalStr = str.OriginalText;

		//			if (!translationData.OriginalText.translations.Exists(x => (x.grougId == groupId && x.translation == originalStr && x.stringId == str.stringId)))
		//			{
		//				TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
		//				newEntry.grougId = groupId;
		//				newEntry.stringId = str.stringId;
		//				newEntry.translation = originalStr;
		//				translationData.OriginalText.translations.Add(newEntry);

		//				newEntry.translation = "";
		//				foreach (var language in translationData.Languages)
		//				{
		//					language.translations.Add(newEntry);
		//				}
		//			}
		//		}
		//	}

		//	EditorUtility.SetDirty(so);
		//	EditorUtility.SetDirty(translationData);
		//}

		//Get project build settings scenes
		foreach ( var scene in EditorBuildSettings.scenes )
		{
			EditorSceneManager.OpenScene( scene.path, OpenSceneMode.Single );
			Debug.Log("Opening Scene: " + scene.path );

			var trTextList = Resources.FindObjectsOfTypeAll( typeof(TrText) ) as TrText[];
			foreach( var trtext in trTextList)
			{
				string groupId = trtext.groupId;

				Text originalText = trtext.gameObject.GetComponent<Text>();
				string originalStr = originalText.text;

				if( ! translationData.OriginalText.translations.Exists( x => ( x.grougId == groupId && x.translation == originalStr && x.stringId == trtext.stringId ) ) )
				{					
					TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
					newEntry.grougId = groupId;
					newEntry.stringId = trtext.stringId;
					newEntry.translation = originalStr;
					translationData.OriginalText.translations.Add(newEntry);

					newEntry.translation = "";
					foreach(var language in translationData.Languages)
					{
						language.translations.Add(newEntry);
					}
				}
			}

			var trStringList = Resources.FindObjectsOfTypeAll( typeof(TrString) ) as TrString[];
			foreach( var trstring in trStringList)
			{
				string groupId = trstring.groupId;

				foreach( var str in trstring.strings)
				{
					string originalStr = str.OriginalText;

					if( ! translationData.OriginalText.translations.Exists( x => ( x.grougId == groupId && x.translation == originalStr && x.stringId == str.stringId ) ) )
					{					
						TranslationData.TranslationEntry newEntry = new TranslationData.TranslationEntry();
						newEntry.grougId = groupId;
						newEntry.stringId = str.stringId;
						newEntry.translation = originalStr;
						translationData.OriginalText.translations.Add(newEntry);

						newEntry.translation = "";
						foreach(var language in translationData.Languages)
						{
							language.translations.Add(newEntry);
						}
					}
				}
			}
		}

		EditorUtility.SetDirty(translationData);

		//for each script in project ???

		//Scriptable object
		var guids = AssetDatabase.FindAssets ("t:TrScriptableObject");
		foreach(var guid in guids) {
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			Debug.Log("open " + assetPath );
			TrScriptableObject so = (TrScriptableObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TrScriptableObject));
			so.TrUpdate();
			EditorUtility.SetDirty(so);
			EditorUtility.SetDirty(translationData);
		}

		foreach(var entry in translationData.OriginalText.translations)
		{
			if( !groupIds.Exists( x => x == entry.grougId ) )
				groupIds.Add(entry.grougId);
		}

		EditorUtility.SetDirty(translationData);
	}

	void AddLanguage()
	{
		//Add a copy of original language to list
		string strLang = "en";
		switch (NewLanguage) 
		{
		case LanguagesIDS.Spanish:			strLang = "es"; break;
		case LanguagesIDS.English:			strLang = "en"; break;
		case LanguagesIDS.Portuguese:		strLang = "pt"; break;
		case LanguagesIDS.ChineseSimplified:strLang = "zh"; break;
		case LanguagesIDS.Japanese:			strLang = "ja"; break;
		case LanguagesIDS.Korean:			strLang = "ko"; break;
		case LanguagesIDS.French:			strLang = "fr"; break;
		case LanguagesIDS.Italian:			strLang = "it"; break;
		case LanguagesIDS.German:			strLang = "de"; break;
		case LanguagesIDS.Russian:			strLang = "ru"; break;
		case LanguagesIDS.Turkish:			strLang = "tk"; break;
		case LanguagesIDS.Indonesian:		strLang = "hi"; break;
		case LanguagesIDS.Neutral:			strLang = "n_es"; break;
		default: 							strLang = "en"; break;
		};

		if( !translationData.Languages.Exists( x => (x.languageId == strLang) ) )
		{			
			TranslationData.Language newLang = new TranslationData.Language();
			newLang.languageId = strLang;
			newLang.translations = new List<TranslationData.TranslationEntry>( translationData.OriginalText.translations );
			for(int i = 0; i < newLang.translations.Count; i++)
			{
				TranslationData.TranslationEntry entry =  newLang.translations[i];
				entry.translation = "";
				newLang.translations[i] = entry;
			}

			translationData.Languages.Add(newLang);
			EditorUtility.SetDirty(translationData);
		}
	}

	void DelLanguage() 
	{
		string strLang = "en";
		switch (NewLanguage) 
		{
		case LanguagesIDS.Spanish:			strLang = "es"; break;
		case LanguagesIDS.English:			strLang = "en"; break;
		case LanguagesIDS.Portuguese:		strLang = "pt"; break;
		case LanguagesIDS.ChineseSimplified:strLang = "zh"; break;
		case LanguagesIDS.Japanese:			strLang = "ja"; break;
		case LanguagesIDS.Korean:			strLang = "ko"; break;
		case LanguagesIDS.French:			strLang = "fr"; break;
		case LanguagesIDS.Italian:			strLang = "it"; break;
		case LanguagesIDS.German:			strLang = "de"; break;
		case LanguagesIDS.Russian:			strLang = "ru"; break;
		case LanguagesIDS.Turkish:			strLang = "tk"; break;
		case LanguagesIDS.Indonesian:		strLang = "hi"; break;
		case LanguagesIDS.Neutral:			strLang = "n_es"; break;
		default: 							strLang = "en"; break;
		};

		int fidx = translationData.Languages.FindIndex( x => (x.languageId == strLang) );
		if( fidx > -1 )
		{			
			if( EditorUtility.DisplayDialog("Remove Language", "Are you sure you want to remove " + NewLanguage.ToString() + " language?", "Of course", "No, my godness") )
			{
				translationData.Languages.RemoveAt(fidx);
				EditorUtility.SetDirty(translationData);
			}
		}
	}

	HSSFWorkbook wb;
	HSSFSheet sh;

	void ExportExcel() {
		if(groupIds.Count == 0)
		{
			UpdateTranslations();
		}

		var path = EditorUtility.SaveFilePanel("Export To Excel", "", "language.xls", "xls");
		if (path.Length != 0)
		{
			wb = new HSSFWorkbook();
			sh = (HSSFSheet)wb.CreateSheet("Translations");

			//Add first row with column names
			HSSFRow labelRow = (HSSFRow)sh.CreateRow(0);

			HSSFCell idCell = (HSSFCell)labelRow.CreateCell(0);
			idCell.SetCellValue("id");

			HSSFCell originalCell = (HSSFCell)labelRow.CreateCell(1);
			originalCell.SetCellValue("original");

			int i = 2;
			foreach(var language in translationData.Languages)
			{
				//Column with language id
				labelRow.CreateCell(i).SetCellValue(language.languageId);
				i++;
			}

			foreach( string groudId in groupIds)
			{
				foreach(var entry in translationData.OriginalText.translations)
				{
					if(entry.grougId != groudId) continue;

					string stringId = entry.stringId;
					HSSFRow newRow = (HSSFRow)sh.CreateRow(sh.PhysicalNumberOfRows);

					HSSFCell id = (HSSFCell)newRow.CreateCell(0);
					id.SetCellValue(stringId);

					HSSFCell original = (HSSFCell)newRow.CreateCell(1);
					original.SetCellValue(entry.translation);

					int c = 2;
					foreach(var language in translationData.Languages)
					{
						if( language.translations.Exists( x => (x.stringId == stringId)) )
						{
							var entryLang = language.translations.Find( x => (x.stringId == stringId));
							newRow.CreateCell(c).SetCellValue( entryLang.translation );
						}
						c++;
					}
				}

				//Add empty row
				sh.CreateRow(sh.PhysicalNumberOfRows);
			}

			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				wb.Write(fs);
				fs.Close();
			}
		}
	}

	void ImportExcel() {
		string path = EditorUtility.OpenFilePanel("Import To Excel", "", "xls");
		if (path.Length != 0)
		{
			using (var fs = new FileStream (path, FileMode.Open, FileAccess.Read)) {
				wb = new HSSFWorkbook (fs);
				sh = (HSSFSheet)wb.GetSheet ("Translations");

				HSSFRow labelRow = (HSSFRow)sh.GetRow (0);

				for(int r = 1; r <= sh.LastRowNum; r++){
					if(sh.GetRow (r) == null) continue;
					if (sh.GetRow (r).Cells.Count > 0) {
						if(sh.GetRow (r).GetCell(0) == null)
							continue;
						var stringId = sh.GetRow (r).GetCell (0).StringCellValue;
						
						var originalString = sh.GetRow(r).GetCell(1).StringCellValue;
						for (int l = 0; l < translationData.OriginalText.translations.Count; l++)
						{
							TranslationData.TranslationEntry entry = translationData.OriginalText.translations[l];
							if (entry.stringId == stringId && entry.translation != originalString)
							{
								entry.translation = originalString;
								translationData.OriginalText.translations[l] = entry;
								break;
							}
						}


						for (int c = 2; c < sh.GetRow (r).Cells.Count; c++) {
							if(labelRow.GetCell (c) == null)
								continue;
							var langId = labelRow.GetCell (c).StringCellValue;

							if (translationData.Languages.Exists (l => (l.languageId == langId))) {
								var language = translationData.Languages.Find (l => (l.languageId == langId));

								for (int l = 0; l < language.translations.Count; l++) {
									TranslationData.TranslationEntry entry = language.translations [l];
									if (entry.stringId == stringId) {
										if(sh.GetRow (r).GetCell (c) != null)
										{
											entry.translation = sh.GetRow (r).GetCell (c).StringCellValue;
											language.translations [l] = entry;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	#region UTILS
	private Color languageColor( string strLang ) {
		Color ret = new Color();
		switch (strLang)
		{
		case "es": ret = Color.blue;					break;
		case "en": ret = Color.cyan;					break;
		case "pt": ret = Color.gray;					break;
		case "zh": ret = Color.green;					break;
		case "ja": ret = Color.magenta;					break;
		case "ko": ret = Color.red;						break;
		case "fr": ret = Color.white;					break;
		case "it": ret = Color.yellow;					break;
		case "de": ret = new Color(1.0f, 0.5f, 0.0f);	break;
		case "ru": ret = new Color(0.0f, 0.5f, 0.5f);	break;
		case "tk": ret = new Color(0.5f, 0.0f, 0.5f);	break;
		case "hi": ret = new Color(0.5f, 0.5f, 0.0f);	break;
		case "n_es": ret = new Color(1.0f, 0.0f, 0.5f);	break;
		};
		return ret;
	}
	#endregion
}


#endif