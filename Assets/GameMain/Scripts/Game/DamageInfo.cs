using GameFramework;

public class DamageInfo : IReference
{
    public int Attacker;
    public int Defender;
    public float CriticalRate;
    public float CriticalMulti;
    public float Degree;
    public int Damage;

    public static DamageInfo Create(int attacker, int defender, float criticalRate, float criticalMulti, float degree, int damage)
    {
        DamageInfo info = ReferencePool.Acquire<DamageInfo>();
        info.Attacker = attacker;
        info.Defender = defender;
        info.CriticalRate = criticalRate;
        info.CriticalMulti = criticalMulti;
        info.Degree = degree;
        info.Damage = damage;

        return info;
    }

    public static DamageInfo Create(int attacker, int defender, float degree, int damage)
    {
        DamageInfo info = ReferencePool.Acquire<DamageInfo>();
        info.Attacker = attacker;
        info.Defender = defender;
        info.CriticalRate = 0f;
        info.CriticalMulti = 1f;
        info.Degree = degree;
        info.Damage = damage;

        return info;
    }

    public void Clear()
    {
        Attacker = 0;
        Defender = 0;
        CriticalRate = 0f;
        CriticalMulti = 0f;
        Degree = 0f;
        Damage = 0;
    }
}