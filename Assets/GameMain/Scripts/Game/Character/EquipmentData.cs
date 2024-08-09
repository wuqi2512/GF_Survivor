using cfg;
using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class EquipmentData : IReference
{
    private int m_Level;
    private List<Modifier> m_Modifiers;
    private Equipment m_Equipment;

    public int EquipmentId => m_Equipment.Id;
    public EquipmentType EquipmentType => m_Equipment.EquipmentType;
    public List<Modifier> Modifiers
    {
        get
        {
            if (m_Modifiers == null)
            {
                m_Modifiers = new List<Modifier>();
                GetModifiers(m_Level, m_Modifiers);
            }
            return m_Modifiers;
        }
    }
    public int Level => m_Level;
    public Equipment Equipment => m_Equipment;

    public static EquipmentData Create(int equipmentId, int level = 1)
    {
        EquipmentData equipmentData = ReferencePool.Acquire<EquipmentData>();
        equipmentData.m_Level = level;
        equipmentData.m_Equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);
        if (equipmentData.m_Equipment == null)
        {
            Log.Error("Equipment '{0}' not exist.", equipmentId);
        }

        return equipmentData;
    }

    public void GetModifiers(int level, List<Modifier> list)
    {
        if (list == null)
        {
            return;
        }

        foreach (var modifier in list)
        {
            ReferencePool.Release(modifier);
        }
        list.Clear();

        foreach (var modifierData in m_Equipment.BaseAttributes)
        {
            Modifier modifier = Modifier.Create(modifierData.Numeric, modifierData.Modifier, modifierData.Values[level]);
            list.Add(modifier);
        }
    }

    public void Upgrade()
    {
        m_Level++;
        if (m_Modifiers == null)
        {
            m_Modifiers = new List<Modifier>();
        }
        GetModifiers(m_Level, m_Modifiers);
    }

    public bool CanUpgrade()
    {
        if (m_Level >= Constant.Game.EquipmentMaxLevel)
        {
            return false;
        }

        int cost = Constant.Game.EquipmentUpgradeCostCoin[Level];
        return GameEntry.Player.Coin >= cost;
    }

    public void Clear()
    {
        m_Level = 0;
        m_Equipment = null;
        if (m_Modifiers != null)
        {
            foreach (Modifier modifier in m_Modifiers)
            {
                ReferencePool.Release(modifier);
            }
            m_Modifiers.Clear();
        }
    }
}