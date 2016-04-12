using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleLocalization.Core;
using UnityEngine.UI;

namespace SimpleLocalization.UI
{
    public class LanguageSwitch : MonoBehaviour
    {
        private LocalizationManager _manager { get { return LocalizationManager.Instance; } }

        private Button _button;
        public Image Image;

        public List<Sprite> Sprites = new List<Sprite>();

        private void Start()
        {
            _button = gameObject.GetComponentInChildren<Button>();

            if (_button)
                _button.onClick.AddListener(SetLanguage);

            SetLanguage();
        }

        private void SetLanguage()
        {
            if (_manager == null || Sprites.Count - 1 < _manager._currentLanguageIndex)
                return;

            if(Image)
                Image.sprite = Sprites[_manager._currentLanguageIndex];
            LocalizationManager.NextLanguage();
        }
    }
}