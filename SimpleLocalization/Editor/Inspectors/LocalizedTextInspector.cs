using UnityEngine;
using UnityEditor;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Core.Components;

namespace SimpleLocalization.Editor
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextInspector : LocalizedComponentInspector<LocalizedText>
    {
        protected override void GetKeys()
        {
            _keys = LocalizationManager.Instance.GetKeys<string>(CurrentPackage);
        }
    }
    
}