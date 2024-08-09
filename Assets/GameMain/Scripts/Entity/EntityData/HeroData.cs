using cfg;
using GameFramework;

public class HeroData : EntityData
{
    public override int EntityId => m_Hero.EntityId;

    public ChaAttribute ChaAttribute;
    public Hero m_Hero;

    public static HeroData Create(Hero hero, int serialId)
    {
        HeroData heroData = ReferencePool.Acquire<HeroData>();

        heroData.m_Hero = hero;
        heroData.m_SerialId = serialId;
        heroData.ChaAttribute = ChaAttribute.Create();
        GameEntry.Player.GetChaAttribute(heroData.ChaAttribute);
        
        return heroData;
    }

    public override void Clear()
    {
        base.Clear();

        m_Hero = null;
        ChaAttribute = null;
    }
}