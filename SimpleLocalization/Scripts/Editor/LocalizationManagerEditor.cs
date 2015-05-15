using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    private LocalizationManager m_Target;
    private bool showTable = false;

    private Vector2 tableScroll = Vector2.zero;

    private GUIStyle style;
    private GUIStyle tableHeaderStyleWhite;
    private GUIStyle tableHeaderStyleGrey;
    private GUIStyle tableCellStyleWhite;
    private GUIStyle tableCellStyleGrey;

    
    private void OnEnable()
    {
        m_Target = target as LocalizationManager;
        style = Utils.BackgroundStyle(new Color(0, 1, 0, 0.5f));
        m_Target.LoadTranslation();

        tableHeaderStyleWhite = Utils.BackgroundStyle(new Color(1, 1, 1, 0.5f));
        tableHeaderStyleGrey = Utils.BackgroundStyle(new Color(0.5f, 0.5f, 0.5f, 0.5f));
        tableCellStyleGrey = Utils.BackgroundStyle(new Color(1, 1, 1, 0.3f)); ;
        tableCellStyleWhite = Utils.BackgroundStyle(new Color(0.5f, 0.5f, 0.5f, 0.3f));
    }

    public override void OnInspectorGUI()
    {
        Undo.RecordObject(m_Target, "LocalizayionManager");
        GUILayout.BeginHorizontal();

        m_Target.translationFile = EditorGUILayout.ObjectField("Csv", m_Target.translationFile, typeof(TextAsset), false) as TextAsset;
        if(GUILayout.Button("Load"))
            m_Target.LoadTranslation();

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
 
        if (m_Target.translation != null)
        {
            LanguageSwitch();
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.Space();

            showTable = EditorGUILayout.Foldout(showTable, "Translation grid preview");

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            LocalizationPreview();
        }
        else
            GUILayout.Label("No translation loasded");

        EditorUtility.SetDirty(m_Target);
    }

    private void LanguageSwitch()
    {
        m_Target.LangID = EditorGUILayout.Popup(m_Target.LangID, m_Target.languages);
    }

    private void LocalizationPreview()
    {
        if (showTable)
        {
            tableScroll = GUILayout.BeginScrollView(tableScroll);
            GUILayout.BeginHorizontal();

            for (int i = -1; i < m_Target.languages.Length; i++)
            {
                if (i == -1)
                {
                    GUILayout.BeginVertical(GUILayout.Width(120));
                    GUILayout.Label("Keys/Languages", style);

                    for (int j = 0; j < m_Target.keys.Length; j++)
                        GUILayout.Label(m_Target.keys[j], j % 2 == 1 ? tableHeaderStyleGrey : tableHeaderStyleWhite);

                    GUILayout.EndVertical();
                    continue;
                }

                GUILayout.BeginVertical();
                GUILayout.Label(m_Target.languages[i], i % 2 == 0 ? tableHeaderStyleGrey : tableHeaderStyleWhite);

                for (int j = 0; j < m_Target.keys.Length; j++)
                    GUILayout.Label(m_Target.GetWordTranslation(m_Target.keys[j], m_Target.languages[i]), j % 2 == 0 ? tableCellStyleGrey : tableCellStyleWhite);

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
    }
}
