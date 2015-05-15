using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Localization/Localized Label")]
public class LocalizedText : MonoBehaviour 
{
    private LocalizationManager LM { get { return LocalizationManager.Instance; } }
    private Text m_Text = null;

    public string key
    {
        get
        {
            return LM.keys[curKeyIndex];
        }
    }

    public int curKeyIndex = 0;

    public int KeyIndex
    {
        get
        {
            return curKeyIndex;
        }
        set
        {
            if (curKeyIndex != value && LocalizationManager.Instance)
            {
                curKeyIndex = value;
                UpdateText();
            }
        }
    } 

    private void OnEnable()
    {
        LocalizationManager.OnLanguageUpdated += UpdateText; 
    }

    private void OnDisable()
    {
        LocalizationManager.OnLanguageUpdated -= UpdateText; 
    }

    private void Awake()
    {
        m_Text = gameObject.GetComponent<Text>();
    }

    public void UpdateText()
    {
        m_Text.text = LM.GetWordTranslation(key);
        UnityEditor.EditorUtility.SetDirty(m_Text);
    }
}
