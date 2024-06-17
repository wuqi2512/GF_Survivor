using cfg;
using SimpleJSON;
using System;
using UnityGameFramework.Runtime;

public class LubanComponent : GameFrameworkComponent
{
    public Tables Tables { get; private set; }

    public void LoadLubanConfig(Func<string, JSONNode> loader, Action onLoadCompleteCallback)
    {
        Tables = new Tables(loader);
        onLoadCompleteCallback();
    }
}