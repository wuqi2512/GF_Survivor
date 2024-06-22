using cfg;
using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public partial class Numeric : IReference
{
    public float Value { get; private set; }
    public float Base { get; private set; }
    public float Add { get; private set; }
    public float Pct { get; private set; }
    private Dictionary<int, ModifierCollection> m_ModifierDic;

    public Numeric()
    {
        m_ModifierDic = new Dictionary<int, ModifierCollection>();
    }

    public float SetBase(float value)
    {
        Base = value;
        Update();
        return Base;
    }

    public void AddModifier(Modifier modifier)
    {
        ModifierType modifierType = modifier.ModifierType;
        ModifierCollection modifierCollection = null;
        if (!m_ModifierDic.TryGetValue((int)modifierType, out modifierCollection))
        {
            modifierCollection = ReferencePool.Acquire<ModifierCollection>();
            m_ModifierDic[(int)modifierType] = modifierCollection;
        }
        float value = modifierCollection.AddModifier(modifier);
        switch (modifierType)
        {
            case ModifierType.Add: Add = value; break;
            case ModifierType.Pct: Pct = value; break;
            default: Log.Error("Unknow ModifierType '{0}'.", modifierType); break;
        }
        Update();
    }

    public void RemoveModifier(Modifier modifier)
    {
        ModifierType modifierType = modifier.ModifierType;
        float value = m_ModifierDic[((int)modifierType)].RemoveModifier(modifier);
        switch (modifierType)
        {
            case ModifierType.Add: Add = value; break;
            case ModifierType.Pct: Pct = value; break;
            default: Log.Error("Unknow ModifierType '{0}'.", modifierType); break;
        }
        Update();
    }

    public void Update()
    {
        Value = (Base + Add) * (100f + Pct) / 100f;
    }

    public static Numeric Create()
    {
        return ReferencePool.Acquire<Numeric>();
    }

    public void Clear()
    {
        Value = 0f;
        Base = 0f;
        Add = 0f;
        Pct = 0f;
        foreach (var pair in m_ModifierDic)
        {
            ReferencePool.Release(pair.Value);
        }
        m_ModifierDic.Clear();
    }
}