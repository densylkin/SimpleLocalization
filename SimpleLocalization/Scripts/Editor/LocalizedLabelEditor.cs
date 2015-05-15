using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(LocalizedText))]
public class LocalizedLabelEditor : Editor
{
    private LocalizedText m_Target;

    private void OnEnable()
    {
        m_Target = target as LocalizedText;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label(m_Target.curKeyIndex + "/" + m_Target.KeyIndex);
        GUILayout.Label(m_Target.key);
        m_Target.KeyIndex = EditorGUILayout.Popup(m_Target.KeyIndex, LocalizationManager.Instance.keys.ToArray());
    }
}
