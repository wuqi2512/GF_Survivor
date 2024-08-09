using cfg;
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

    public int Count => m_Numerics.Count;

    public void AddNumeric(int key, float baseValue)
    {
        if (m_Numerics.ContainsKey(key))
        {
            Log.Error("Numeric '{0}' already exist.", key);
            return;
        }

        Numeric numeric = Numeric.Create();
        numeric.SetBase(baseValue);

        m_Numerics.Add(key, numeric);
    }

    public void AddModifier(Modifier modifier)
    {
        m_Numerics[(int)modifier.NumericType].AddModifier(modifier);
    }

    public void AddModifier(List<Modifier> modifiers)
    {
        foreach (Modifier modifier in modifiers)
        {
            AddModifier(modifier);
        }
    }

    public void RemoveModifier(Modifier modifier)
    {
        m_Numerics[(int)modifier.NumericType].RemoveModifier(modifier);
    }

    public void RemoveModifier(List<Modifier> modifiers)
    {
        foreach (Modifier modifier in modifiers)
        {
            RemoveModifier(modifier);
        }
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