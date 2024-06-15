using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class ChaAttribute : IReference
{
    private Dictionary<int, Numeric> m_Numerics;

    public ChaAttribute()
    {
        m_Numerics = new Dictionary<int, Numeric>();
    }

    public void AddNumeric(int key, float baseValue)
    {
        if (m_Numerics.ContainsKey(key))
        {
            Log.Error("Numeric '{0}' already exist.", key);
            return;
        }

        Numeric numeric = Numeric.Create();
        numeric.Base = baseValue;

        m_Numerics.Add(key, numeric);
    }

    public Numeric this[NumericType key]
    {
        get => m_Numerics[(int)key];
    }


    public static ChaAttribute Create()
    {
        ChaAttribute chaAttribute = ReferencePool.Acquire<ChaAttribute>();

        return chaAttribute;
    }

    public void Clear()
    {
        foreach (var pair in m_Numerics)
        {
            ReferencePool.Release(pair.Value);
        }
        m_Numerics.Clear();
    }
}