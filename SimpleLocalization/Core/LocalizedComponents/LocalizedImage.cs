using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SimpleLocalization.Core.Components
{
    public class LocalizedImage : LocalizedComponent<Image>
    {
        public override Type GetDataType()
        {
            return typeof(Sprite);
        }

        protected override void OnLanguageChanged()
        {
            base.OnLanguageChanged();

            var newval = LocalizationManager.Instance.GetTranslation<Sprite>(PackageName, Key);
            if (newval != null)
                _component.sprite = newval;
        }
    } 
}
