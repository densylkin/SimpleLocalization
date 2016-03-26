using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SimpleLocalization.Core.Components
{
    public class LocalizedText : LocalizedComponent<Text>
    {
        public override Type GetDataType()
        {
            return typeof(string);
        }

        protected override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            if(KeyIndex != -1 && PackageIndex != -1)
                _component.text = LocalizationManager.Instance.GetTranslation<string>(PackageName, Key);
        }
    }
    
}