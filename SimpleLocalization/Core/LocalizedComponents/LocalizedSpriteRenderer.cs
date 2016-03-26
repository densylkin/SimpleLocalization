using System;
using UnityEngine;
using System.Collections;

namespace SimpleLocalization.Core.Components
{
    public class LocalizedSpriteRenderer : LocalizedComponent<SpriteRenderer>
    {
        public override Type GetDataType()
        {
            return typeof(Sprite);
        }

        protected override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            _component.sprite = LocalizationManager.Instance.GetTranslation<Sprite>(PackageName, Key);
        }
    }
    
}