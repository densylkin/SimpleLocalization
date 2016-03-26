using System;
using UnityEngine;
using System.Collections;
using System.IO;
using CsvHelper;
using SimpleLocalization.Core;
using System.Linq;

namespace SimpleLocalization.Helpers
{
    public static class CSVHelper
    {
        public static ILocalizationData<string> Import(string path)
        {
            var temp = new LocalizationData<string>();

            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvParser(reader))
                {
                    while (true)
                    {
                        var row = csv.Read();
                        var currentKey = "";

                        if (row == null)
                            break;

                        if (csv.Row == 1)
                        {
                            foreach (var s in row)
                            {
                                if (string.IsNullOrEmpty(s))
                                    continue;
                                var lang = (SystemLanguage) Enum.Parse(typeof (SystemLanguage), s);
                                temp.AddLanguage(lang);
                            }
                        }
                        else
                        {
                            if (row == null)
                                break;

                            for (var i = 0; i < row.Length; i++)
                            {
                                var s = row[i];
                                if (i == 0)
                                {
                                    currentKey = s;
                                    temp.AddKey(currentKey);
                                    continue;
                                }

                                temp[temp.Languages[i - 1], currentKey] = s;
                            }
                        }

                        if (row == null || row.Length == 0)
                            break;
                    }
                }
            }

            return temp;
        }

        public static void Export(this ILocalizationData data, string path)
        {
            if(data.DataType != typeof(string))
                return;

            using (var stream = new StreamWriter(path))
            {
                using (var writer = new CsvWriter(new CsvSerializer(stream)))
                {
                    writer.WriteField("");
                    foreach (var language in data.Languages)
                    {
                        writer.WriteField(language.ToString());
                    }
                    writer.NextRecord();
                    foreach (var key in data.Keys)
                    {
                        writer.WriteField(key);
                        foreach (var language in data.Languages)
                        {
                            writer.WriteField((string)data.GetTranslation(language, key));
                        }
                        writer.NextRecord();
                    }
                }
            }
        }
    } 
}
