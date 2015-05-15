using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[ExecuteInEditMode]
[AddComponentMenu("Localization/Localization Manager")]
public class LocalizationManager : Singleton<LocalizationManager>
{
    #region Public_Fields

    public TextAsset translationFile = null;

    public string curLanguage;

    public string[,] translation;
    public string[] keys;
    public string[] languages;

    public static event Action OnLanguageUpdated; //fires when language is changed

    #endregion

    public int LangID
    {
        get
        {
            return Array.IndexOf(languages, curLanguage);
        }
        set
        {
            if (value != Array.IndexOf(languages, curLanguage))
                ChangeLanguage(value);
        }
    }

    private void Start()
    {
        LoadTranslation();
        OnLanguageUpdated();
    }

    /// <summary>
    /// Loads translation from file
    /// </summary>
    /// <returns></returns>
    public bool LoadTranslation()
    {
        if (translationFile == null)
            return false;

        translation = CsvReader.SplitCsvGrid(translationFile.text);

        keys = translation.GetColumn(0);
        languages = translation.GetRow(0);
        curLanguage = languages[0];

        return true;
    }

    /// <summary>
    /// Gets translation for key current language
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetWordTranslation(string key)
    {
        int keyIndex = Array.IndexOf(keys, key);
        int languageIndex = Array.IndexOf(languages, curLanguage);
        return translation[languageIndex + 1, keyIndex + 1];
    }

    /// <summary>
    /// Gets translation for key specific language
    /// </summary>
    /// <param name="key"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public string GetWordTranslation(string key, string language)
    {
        int keyIndex = Array.IndexOf(keys, key);
        int languageIndex = Array.IndexOf(languages, language);
        return translation[languageIndex + 1, keyIndex + 1];
    }


    /// <summary>
    /// Changes the current language
    /// </summary>
    /// <param name="langID"></param>
    public void ChangeLanguage(int langID)
    {
        curLanguage = languages[langID];
        OnLanguageUpdated();
    }
    
}

