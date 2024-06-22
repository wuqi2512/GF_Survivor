using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public static class ResourcesExtension
{
    private static Dictionary<string, Action<object>> m_Callbcks = new Dictionary<string, Action<object>>();
    private static LoadAssetCallbacks m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFailure);

    private static void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
    {
        Action<object> onSuccess = null;
        if (m_Callbcks.TryGetValue(assetName, out onSuccess))
        {
            onSuccess.Invoke(asset);
            m_Callbcks.Remove(assetName);
        }
    }

    private static void LoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        Log.Error("Load asset failure with '{0}'.", errorMessage);
    }

    public static void LoadAsset(this ResourceComponent resourceComponent, string assetName, Action<object> onLoadAssetSuccess)
    {
        m_Callbcks[assetName] = onLoadAssetSuccess;
        resourceComponent.LoadAsset(assetName, m_LoadAssetCallbacks);
    }
}