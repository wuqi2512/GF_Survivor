using GameFramework;

public class HeroData : EntityData
{
    private int m_EntityId = 0;

    public override int EntityId => m_EntityId;

    public int MaxHp => 100;

    public static HeroData Create(int entityId, int serialId)
    {
        HeroData heroData = ReferencePool.Acquire<HeroData>();
        heroData.m_EntityId = entityId;
        heroData.m_SerialId = serialId;

        return heroData;
    }

    public override void Clear()
    {
        base.Clear();

        m_EntityId = 0;
    }
}