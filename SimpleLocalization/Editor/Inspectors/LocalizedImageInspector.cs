using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Core.Components;
using UnityEditor;

namespace SimpleLocalization.Editor
{
    [CustomEditor(typeof(LocalizedImage))]
    public class LocalizedImageInspector : LocalizedComponentInspector<LocalizedImage>
    {
        protected override void GetKeys()
        {
            _keys = LocalizationManager.Instance.GetKeys<Sprite>(CurrentPackage);
        }
    }
    
}