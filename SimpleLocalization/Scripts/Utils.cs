using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public static class Utils
{
    [MenuItem("GameObject/Create Other/Localization Manager")]
    public static void CreateLocalizationManager()
    {
        if (GameObject.FindObjectOfType<LocalizationManager>() == null)
        {
            GameObject go = new GameObject();
            go.name = "LocalizationManager";
            go.AddComponent<LocalizationManager>();
            Selection.activeGameObject = go;
        }
        else
        {
            Debug.LogWarning("LocalizationManager is already in scene");
        }
    }

    #region Array_utils

    public static string[] GetColumn(this string[,] target, int row)
    {
        var columns = target.GetLength(1);
        var array = new string[columns];
        for (int i = 0; i < columns; ++i)
            array[i] = target[row, i];

        return array.Where(str => !string.IsNullOrEmpty(str)).ToArray();
    }

    public static string[] GetRow(this string[,] target, int column)
    {
        var rows = target.GetLength(0);
        var array = new string[rows];
        for (int i = 0; i < rows; ++i)
            array[i] = target[i, column];

       return array.Where(str => !string.IsNullOrEmpty(str)).ToArray();
    }

    #endregion

    #region GUI_Utils

    private static Texture2D MakeTex(Color col)
    {
        Color[] pix = new Color[1 * 1];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        result.hideFlags = HideFlags.HideAndDontSave;
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    public static GUIStyle BackgroundStyle(Color color)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = MakeTex(color);
        return style;
    }

    #endregion
}
