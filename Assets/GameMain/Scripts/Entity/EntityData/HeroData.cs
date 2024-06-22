using cfg;
using GameFramework;

public class HeroData : EntityData
{
    public override int EntityId => m_Hero.EntityId;

    public ChaAttribute ChaAttribute;
    public Hero m_Hero;
    private EquipmentData[] m_Equipments;

    public HeroData()
    {
        m_Equipments = new EquipmentData[4];
    }

    private void LoadBaseAttribute()
    {
        ChaAttribute.AddNumeric((int)NumericType.MaxHp, m_Hero.MaxHp);
        ChaAttribute.AddNumeric((int)NumericType.Hp, m_Hero.MaxHp);
        ChaAttribute.AddNumeric((int)NumericType.MoveSpeed, m_Hero.MoveSpeed);
        ChaAttribute.AddNumeric((int)NumericType.AttackSpeed, m_Hero.AttackSpeed);
    }

    public static HeroData Create(int entityId, int serialId)
    {
        HeroData heroData = ReferencePool.Acquire<HeroData>();

        Hero hero = GameEntry.Luban.Tables.TbHero.GetOrDefault(entityId);
        heroData.m_Hero = hero;
        heroData.m_SerialId = serialId;
        heroData.ChaAttribute = ChaAttribute.Create();
        
        heroData.LoadBaseAttribute();

        return heroData;
    }

    public override void Clear()
    {
        base.Clear();

        m_Hero = null;
        for (int i = 0; i < m_Equipments.Length; i++)
        {
            if (m_Equipments[i] == null)
            {
                continue;
            }
            ReferencePool.Release(m_Equipments[i]);
            m_Equipments[i] = null;
        }
        ReferencePool.Release(ChaAttribute);
    }

    public int Equip(int equipmentId)
    {
        EquipmentData newEquipment = EquipmentData.Create(equipmentId);
        EquipmentType equipmentType = newEquipment.EquipmentType;
        int oldEquipmentId = -1;
        if (HasEquip(equipmentType))
        {
            oldEquipmentId = m_Equipments[(int)equipmentType].EquipmentId;
            Unequip(equipmentType);
        }
        m_Equipments[(int)newEquipment.EquipmentType] = newEquipment;
        foreach (Modifier modifier in newEquipment.Modifiers)
        {
            ChaAttribute.AddModifier(modifier);
        }

        return oldEquipmentId;
    }

    public bool HasEquip(EquipmentType equipmentType)
    {
        return m_Equipments[(int)equipmentType] != null;
    }

    public int Unequip(EquipmentType equipmentType)
    {
        EquipmentData equipmentData = m_Equipments[(int)equipmentType];
        int equipmentId = -1;
        if (equipmentData == null)
        {
            return equipmentId;
        }

        equipmentId = equipmentData.EquipmentId;
        m_Equipments[(int)equipmentType] = null;
        foreach (Modifier modifier in equipmentData.Modifiers)
        {
            ChaAttribute.RemoveModifier(modifier);
        }
        ReferencePool.Release(equipmentData);

        return equipmentId;
    }
}