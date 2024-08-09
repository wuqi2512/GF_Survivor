using cfg;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public partial class Player
{
    public delegate void OnAddEquipment(EquipmentData equipmentData);
    public delegate void OnRemoveEquipment(EquipmentData equipmentData);
    public delegate void OnEquipEquipment(EquipmentData oldEeuip, EquipmentData newEquip);
    public delegate void OnUnequipEquipment(EquipmentData equipmentData);
    public delegate void OnUpgradeEquipment(EquipmentData equipmentData);

    public event OnAddEquipment OnAddEquip;
    public event OnRemoveEquipment OnRemoveEquip;
    public event OnEquipEquipment OnEquip;
    public event OnUnequipEquipment OnUnequip;
    public event OnUpgradeEquipment OnUpgradeEquip;

    private int m_Coin;
    private int m_Diamond;
    private Hero m_Hero;
    private List<EquipmentData> m_EquipmentDatas = new List<EquipmentData>();
    private EquipmentData[] m_EquippedDatas = new EquipmentData[6];

    public int Coin => m_Coin;
    public int Diamond => m_Diamond;

    public void AddEquipment(int equipmentId)
    {
        EquipmentData equipmentData = EquipmentData.Create(equipmentId);
        m_EquipmentDatas.Add(equipmentData);
        OnAddEquip?.Invoke(equipmentData);
    }

    public void RemoveEquipment(EquipmentData equipmentData)
    {
        m_EquipmentDatas.Remove(equipmentData);
        OnRemoveEquip?.Invoke(equipmentData);
    }

    public void Equip(EquipmentData equipmentData)
    {
        if (!m_EquipmentDatas.Contains(equipmentData))
        {
            Log.Error("Player not have Equipment '{0}'.", equipmentData.ToString());
            return;
        }

        int index = (int)equipmentData.EquipmentType;
        EquipmentData oldEquipment = m_EquippedDatas[index];
        m_EquippedDatas[index] = equipmentData;
        OnEquip?.Invoke(oldEquipment, equipmentData);
    }

    public void Unequip(EquipmentType equipmentType)
    {
        int index = (int)equipmentType;
        EquipmentData equipmentData = m_EquippedDatas[index];
        if (equipmentData == null)
        {
            return;
        }

        m_EquippedDatas[index] = null;
        OnUnequip?.Invoke(equipmentData);
    }

    public void UpgradeEquipment(EquipmentData equipmentData)
    {
        if (equipmentData.Level >= Constant.Game.EquipmentMaxLevel)
        {
            return;
        }

        int costCoint = Constant.Game.EquipmentUpgradeCostCoin[equipmentData.Level];
        if (costCoint > m_Coin)
        {
            return;
        }

        m_Coin -= costCoint;
        equipmentData.Upgrade();
        OnUpgradeEquip?.Invoke(equipmentData);
    }

    public void AddCoin(int value)
    {
        m_Coin += value;
    }

    public void AddDiamond(int value)
    {
        m_Diamond += value;
    }

    public void GainDropItem(DropItemData dropItemData)
    {
        DropItemType dropItemType = dropItemData.DropItemType;
        int value = dropItemData.Value;
        switch (dropItemType)
        {
            case DropItemType.Coin: AddCoin(value); break;
            case DropItemType.Diamond: AddDiamond(value); break;
            case DropItemType.Equipment: AddEquipment(value); break;
        }
    }

    public bool IsEquipped(EquipmentData equipmentData)
    {
        return equipmentData == m_EquippedDatas[(int)equipmentData.EquipmentType];
    }

    public bool IsEquipped(EquipmentType equipmentType)
    {
        return m_EquippedDatas[(int)equipmentType] != null;
    }

    public EquipmentData GetEquipped(EquipmentType equipmentType)
    {
        return m_EquippedDatas[(int)equipmentType];
    }

    public List<EquipmentData> GetAllEquipment()
    {
        return m_EquipmentDatas;
    }

    public void GetChaAttribute(ChaAttribute chaAttribute)
    {
        chaAttribute.Clear();

        chaAttribute.AddNumeric((int)NumericType.MaxHp, m_Hero.MaxHp);
        chaAttribute.AddNumeric((int)NumericType.Hp, m_Hero.MaxHp);
        chaAttribute.AddNumeric((int)NumericType.MoveSpeed, m_Hero.MoveSpeed);
        chaAttribute.AddNumeric((int)NumericType.AttackSpeed, m_Hero.AttackSpeed);
        chaAttribute.AddNumeric((int)NumericType.Attack, m_Hero.Attack);
        chaAttribute.AddNumeric((int)NumericType.Defence, m_Hero.Defense);

        foreach (var item in m_EquippedDatas)
        {
            if (item != null)
            {
                chaAttribute.AddModifier(item.Modifiers);
            }
        }
    }

    public Hero GetHero()
    {
        return m_Hero;
    }
}