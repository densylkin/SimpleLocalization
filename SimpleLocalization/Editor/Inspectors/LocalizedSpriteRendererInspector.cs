using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Core.Components;

namespace SimpleLocalization.Editor
{
    public class LocalizedSpriteRendererInspector : LocalizedComponentInspector<LocalizedSpriteRenderer>
    {
        protected override void GetKeys()
        {
            _keys = LocalizationManager.Instance.GetKeys<Sprite>(CurrentPackage);
        }
    }
    
}