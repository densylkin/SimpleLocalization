using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Core.Components;
using UnityEditor;

namespace SimpleLocalization.Editor
{
    [CustomEditor(typeof(LocalizedFont))]
    public class LocalizdFontInspector : LocalizedComponentInspector<LocalizedFont>
    {
        protected override void GetKeys()
        {
            _keys = LocalizationManager.Instance.GetKeys<Font>(CurrentPackage);
        }
    }
}
