using System;
using UnityEngine;
using System.Collections;
using SimpleLocalization.Core.Components;
using UnityEngine.UI;

namespace SimpleLocalization.Core.Components
{
    public class LocalizedFont : LocalizedComponent<Text>
    {
        public override Type GetDataType()
        {
            return typeof(Font);
        }

        protected override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            if (KeyIndex != -1 && PackageIndex != -1)
                _component.font = LocalizationManager.Instance.GetTranslation<Font>(PackageName, Key);
        }
    }
}
