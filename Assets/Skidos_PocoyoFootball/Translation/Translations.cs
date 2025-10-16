using System.Collections;
using System.Xml;

using UnityEngine;
using UnityEngine.UI;

public class Translation {
	//Static Methods
	#region StaticMethods
	private static Translation instance = null;

	public static Translation GetInstance()
	{
		if (instance == null)
			instance = new Translation();
		return instance;
	}

	public static string NextStringId() {
		long i = 1;
		foreach (byte b in System.Guid.NewGuid().ToByteArray())
			i *= ((int)b + 1);
		return string.Format("{0:x}", i - System.DateTime.Now.Ticks);
	}
	#endregion

	//Methods
	private TranslationData translationData;

	public Translation() {
		SelectedLanguage = -1;
		translationData = Resources.Load<TranslationData>("languages") as TranslationData;
		if (translationData == null) {
			Debug.Log("Translations: Try create without TranslationData");
			return;
		}
	}

	public enum Language {
		LNG_NULL,
		LNG_ES, //Spanish
		LNG_EN, //English
		LNG_PT, //Portuguese
		LNG_CH, //Chinese
		LNG_JA, //Japanese
		LNG_KO, //Korean
		LNG_FR, //French
		LNG_IT, //Italian
		LNG_DE, //German
		LNG_RU, //Russian
		LNG_TK, //Turkist
		LNG_HI,	//Indonesian
		LNG_N_ES,	//Neutral Spanish

		LNG_SIZE
	};

	public System.Action LanguageChanged;

	public void SetSelectedLanguage(Language language)
    {
		SelectedLanguage = (int)language;
		if (SelectedLanguage <= 0)
			getSystemLanguage();
		if (LanguageChanged != null)
			LanguageChanged.Invoke();
    }

    public int SelectedLanguage;
	public string getSystemLanguage()
	{
		string strLang;
		//int saveLanguage = SGamePackageSave.GetInstance ().m_Language;
		//if (saveLanguage <= 0) 
		if (SelectedLanguage <= 0) 
		{
			SystemLanguage language;			
			language = Application.systemLanguage;

			switch (language) 
			{
			case SystemLanguage.Spanish:			strLang = "es"; SelectedLanguage = 1; break;
			case SystemLanguage.English:			strLang = "en"; SelectedLanguage = 2; break;
			case SystemLanguage.Portuguese:			strLang = "pt"; SelectedLanguage = 3; break;
			case SystemLanguage.ChineseSimplified:	strLang = "zh"; SelectedLanguage = 4; break;
			case SystemLanguage.Japanese:			strLang = "ja"; SelectedLanguage = 5; break;
			case SystemLanguage.Korean:				strLang = "ko"; SelectedLanguage = 6; break;
			case SystemLanguage.French:				strLang = "fr"; SelectedLanguage = 7; break;
			case SystemLanguage.Italian:			strLang = "it"; SelectedLanguage = 8; break;
			case SystemLanguage.German:				strLang = "de"; SelectedLanguage = 9; break;
			case SystemLanguage.Russian:			strLang = "ru"; SelectedLanguage = 10; break;
			case SystemLanguage.Turkish:			strLang = "tk"; SelectedLanguage = 11; break;
			case SystemLanguage.Indonesian:			strLang = "hi"; SelectedLanguage = 12; break;
			default: 								strLang = "en"; SelectedLanguage = 2; break;

            }
		}
		else 
		{
			string[] m_ArrayLanguages = { "none", "es", "en", "pt", "zh", "ja", "ko", "fr", "it", "de", "ru", "tk", "hi", "n_es" };            
			strLang = m_ArrayLanguages [SelectedLanguage];
		}

		if(translationData == null || translationData.Languages == null || !translationData.Languages.Exists( x => ( x.languageId == strLang ) ))
		{
			strLang = "en";
		}
			
		return strLang;
	}

	public void resetLanguage(GameObject root)
	{
		Transform[] ts = root.GetComponentsInChildren<Transform>(true);

		foreach(Transform t in ts)
		{
			TrText gameObj = t.GetComponent<TrText>();
			if (gameObj != null) 
				gameObj.resetTranslate ();

			TrImage imgObj = t.GetComponent<TrImage>();
			if(imgObj != null)
				imgObj.ResetImage();

			TrString strObj = t.GetComponent<TrString>();
			if(strObj != null)
				strObj.resetTranslate();
		}
	}

	public void getString(Text component, string str)
	{
		string language = getSystemLanguage ();

		if(translationData == null || translationData.Languages == null)
		{
			component.text = str;
			return;
		}

		int langIdx = translationData.Languages.FindIndex(x => ( x.languageId == language ));
		if(langIdx > -1)
		{
			int strIdx = translationData.Languages[langIdx].translations.FindIndex( x => (x.stringId == str) );
			if(strIdx > -1 && !string.IsNullOrEmpty(translationData.Languages[langIdx].translations[strIdx].translation) )
			{
				component.text = translationData.Languages[langIdx].translations[strIdx].translation;
				return;
			}
		}

		int strOrigIdx = translationData.OriginalText.translations.FindIndex( x => (x.stringId == str) );
		if(strOrigIdx > -1)
			component.text = translationData.OriginalText.translations[strOrigIdx].translation;
		else
            //component.text = "WARNING! MISSING STRING";
            component.text = str;
    }

    public string getString(string str)
    {
        string language = getSystemLanguage();

		if(translationData == null || translationData.Languages == null)
		{			
			return str;
		}

        int langIdx = translationData.Languages.FindIndex(x => (x.languageId == language));
        if (langIdx > -1)
        {
            int strIdx = translationData.Languages[langIdx].translations.FindIndex(x => (x.stringId == str));
			if (strIdx > -1 && !string.IsNullOrEmpty(translationData.Languages[langIdx].translations[strIdx].translation))
			{
				return translationData.Languages[langIdx].translations[strIdx].translation;
            }
        }

        int strOrigIdx = translationData.OriginalText.translations.FindIndex(x => (x.stringId == str));
        if (strOrigIdx > -1)
            return translationData.OriginalText.translations[strOrigIdx].translation;
        else
            //return "WARNING! MISSING STRING";
            return str;
    }

    public string getStringByLanguage(string str, SystemLanguage sysLanguage = SystemLanguage.English)
    {
		if(translationData == null || translationData.Languages == null)
		{			
			return str;
		}

        string language = languageToString(sysLanguage);

        int langIdx = translationData.Languages.FindIndex(x => (x.languageId == language));
        if (langIdx > -1)
        {
            int strIdx = translationData.Languages[langIdx].translations.FindIndex(x => (x.stringId == str));
            if (strIdx > -1)
            {
                if (translationData.Languages[langIdx].translations[strIdx].translation == "")
                {
                    return str;
                }
                return translationData.Languages[langIdx].translations[strIdx].translation;
            }
        }

        int strOrigIdx = translationData.OriginalText.translations.FindIndex(x => (x.stringId == str));
        if (strOrigIdx > -1)
            return translationData.OriginalText.translations[strOrigIdx].translation;
        else
            //return "WARNING! MISSING STRING";
            return str;
    }

    public string languageToString(SystemLanguage language)
    {
        string strLang;

        switch (language) 
        {
        case SystemLanguage.Spanish:			strLang = "es"; break;
        case SystemLanguage.English:			strLang = "en"; break;
        case SystemLanguage.Portuguese:			strLang = "pt"; break;
        case SystemLanguage.ChineseSimplified:	strLang = "zh"; break;
        case SystemLanguage.Japanese:			strLang = "ja"; break;
        case SystemLanguage.Korean:				strLang = "ko"; break;
        case SystemLanguage.French:				strLang = "fr"; break;
        case SystemLanguage.Italian:			strLang = "it"; break;
        case SystemLanguage.German:				strLang = "de"; break;
        case SystemLanguage.Russian:			strLang = "ru"; break;
        case SystemLanguage.Turkish:			strLang = "tk"; break;
        default: 								strLang = "en"; break;
        }

        return strLang;
    }

    public bool isTranslatedString(string stringId) {
		if(translationData == null || translationData.Languages == null)
		{			
			return false;
		}

		return translationData.OriginalText.translations.Exists(x => (x.stringId == stringId));
	}

	public string getOriginalString(string stringId)
    {
		if (isTranslatedString(stringId))
			return translationData.OriginalText.translations.Find(x => (x.stringId == stringId)).translation;
		return stringId;
	}
}
