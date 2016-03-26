using UnityEngine;
using System.Collections;
using UnityEditor;

public class CsvImportWindow : EditorWindow
{
    public static void Init()
    {
        var window = GetWindow<CsvImportWindow>();
        window.Show();
    }

    private void OnGUI()
    {
        
    }
}
