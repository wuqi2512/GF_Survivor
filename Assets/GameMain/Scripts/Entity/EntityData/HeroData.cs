using cfg;
using GameFramework;

public class HeroData : EntityData
{
    public override int EntityId => m_Hero.EntityId;

    public ChaAttribute ChaAttribute;
    public Hero m_Hero;

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
        ReferencePool.Release(ChaAttribute);
    }
}