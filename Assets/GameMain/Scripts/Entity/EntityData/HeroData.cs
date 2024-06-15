using GameFramework;

public class HeroData : EntityData
{
    private int m_EntityId = 0;

    public override int EntityId => m_EntityId;

    public ChaAttribute ChaAttribute;

    private void LoadBaseAttribute()
    {
        ChaAttribute.AddNumeric((int)NumericType.MaxHp, 100f);
        ChaAttribute.AddNumeric((int)NumericType.Hp, 100f);
        ChaAttribute.AddNumeric((int)NumericType.MoveSpeed, 5f);
        ChaAttribute.AddNumeric((int)NumericType.AttackSpeed, 1f);
    }

    public static HeroData Create(int entityId, int serialId)
    {
        HeroData heroData = ReferencePool.Acquire<HeroData>();
        heroData.m_EntityId = entityId;
        heroData.m_SerialId = serialId;
        heroData.ChaAttribute = ChaAttribute.Create();
        heroData.LoadBaseAttribute();

        return heroData;
    }

    public override void Clear()
    {
        base.Clear();

        m_EntityId = 0;
        ReferencePool.Release(ChaAttribute);
    }
}