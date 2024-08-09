using GameFramework;
using GameFramework.Localization;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class JsonLocallizationHelper : DefaultLocalizationHelper
{
    public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
    {
        try
        {
            Dictionary<string, string> dictionary = Utility.Json.ToObject<Dictionary<string, string>>(dictionaryString);
            foreach (var item in dictionary)
            {
                if (!localizationManager.AddRawString(item.Key, item.Value))
                {
                    Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", item.Key);
                    return false;
                }
            }

            return true;
        }
        catch (Exception exception)
        {
            Log.Warning("Can not parse dictionary data with exception '{0}'.", exception.ToString());
            return false;
        }
    }
}