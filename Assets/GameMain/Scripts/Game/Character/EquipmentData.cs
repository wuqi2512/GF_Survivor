using cfg;
using GameFramework;
using System.Collections.Generic;

public class EquipmentData : IReference
{
    public int EquipmentId => m_EquipmentId;
    public EquipmentType EquipmentType => Equipment.EquipmentType;
    public Equipment Equipment;
    public List<Modifier> Modifiers
    {
        get
        {
            if (m_Modifiers == null)
            {
                m_Modifiers = new List<Modifier>();
                InitModifiers();
            }
            return m_Modifiers;
        }
    }

    private List<Modifier> m_Modifiers;
    private int m_EquipmentId;

    public static EquipmentData Create(int equipmentId)
    {
        EquipmentData equipmentData = ReferencePool.Acquire<EquipmentData>();
        equipmentData.m_EquipmentId = equipmentId;
        equipmentData.Equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);

        return equipmentData;
    }

    private void InitModifiers()
    {
        foreach (var modifierData in Equipment.Modifiers)
        {
            Modifier modifier = Modifier.Create(modifierData.NumericType, modifierData.ModifierType, modifierData.Value);
            Modifiers.Add(modifier);
        }
    }

    public void Clear()
    {
        Equipment = null;
        foreach (Modifier modifier in m_Modifiers)
        {
            ReferencePool.Release(modifier);
        }
        m_Modifiers = null;
    }
}