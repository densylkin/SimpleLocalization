using System;
using UnityEngine;
using System.Collections;

namespace SimpleLocalization.Core
{
    public interface ILocalizationPackage
    {
        bool AddLocalizationData(Type type);
        bool RemoveLocalizationData(Type type);
        ILocalizationData GetData(Type type);
        bool ContainsData(Type type);
    } 
}
