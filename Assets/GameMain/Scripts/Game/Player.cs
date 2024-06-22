using cfg;
using System.Collections.Generic;

public class Player
{
    private List<int> m_Equipments;
    private HeroData m_HeroData;
    private EquipmentForm m_EquipmentUIForm;

    public Player(HeroData heroData)
    {
        m_Equipments = new List<int>();
        m_HeroData = heroData;
    }

    public void SetEquipmentForm(EquipmentForm equipmentForm)
    {
        m_EquipmentUIForm = equipmentForm;
    }

    public void AddEquipment(int equipmentId)
    {
        m_Equipments.Add(equipmentId);
        m_EquipmentUIForm?.AddEquipment(equipmentId);
    }

    public void RemoveEquipment(int equipmentId)
    {
        m_Equipments.Remove(equipmentId);
        m_EquipmentUIForm?.RemoveEquipment(equipmentId);
    }

    public void Equip(int equipmentId)
    {
        if (!m_Equipments.Contains(equipmentId))
        {
            return;
        }

        int oldEquipmentId = m_HeroData.Equip(equipmentId);
        m_EquipmentUIForm?.Equip(equipmentId);
        RemoveEquipment(equipmentId);
        if (oldEquipmentId != -1)
        {
            AddEquipment(oldEquipmentId);
        }
    }

    public void Unequip(EquipmentType equipmentType)
    {
        int equipmentId = m_HeroData.Unequip(equipmentType);
        if (equipmentId != -1)
        {
            m_EquipmentUIForm?.Unequip(equipmentType);
            AddEquipment(equipmentId);
        }
    }

    public List<int> GetAllEquipment()
    {
        return m_Equipments;
    }
}