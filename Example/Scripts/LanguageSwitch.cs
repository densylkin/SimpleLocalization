using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;

public class LanguageSwitch : MonoBehaviour
{
    private LocalizationManager _manager;

    private void Start()
    {
        _manager = LocalizationManager.Instance;
    }

    private void OnGUI()
    {
        for (var i = 0; i < _manager.LanguagesCount; i++)
        {
            if (GUILayout.Button(_manager.LanguagesNames[i]))
                LocalizationManager.ChangeLanguage(i);
        }
    }
}
