using System;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace SimpleLocalization.Core
{
    public static class Constants
    {
        public static readonly Type[] SupportedTypes = 
        {
            typeof(string),
            typeof(AudioClip),
            typeof(Texture),
            typeof(GameObject),
            typeof(Sprite),
            typeof(Font)
        };

        public static bool IsSupported(this Type type)
        {
            return SupportedTypes.Contains(type);
        }

        public static ILocalizationData<string> ToTextLocaliztionData(this ILocalizationData data)
        {
            return data.DataType == typeof(string) ? data as ILocalizationData<string> : null;
        }

        public static ILocalizationData<AudioClip> ToAudioLocaliztionData(this ILocalizationData data)
        {
            return data.DataType == typeof(AudioClip) ? data as ILocalizationData<AudioClip> : null;
        }

        public static ILocalizationData<Texture> ToTextureLocaliztionData(this ILocalizationData data)
        {
            return data.DataType == typeof(Texture) ? data as ILocalizationData<Texture> : null;
        }

        public static ILocalizationData<GameObject> ToPrefabLocaliztionData(this ILocalizationData data)
        {
            return data.DataType == typeof(GameObject) ? data as ILocalizationData<GameObject> : null;
        }
    }
    
}