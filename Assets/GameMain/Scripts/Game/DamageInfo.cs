using GameFramework;

public class DamageInfo : IReference
{
    public int Attacker;
    public int Defender;
    public float Damage;
    public float CriticalRate;
    public float CriticalMulti;

    public static DamageInfo Create(int attacker, int defender, float damage)
    {
        DamageInfo info = ReferencePool.Acquire<DamageInfo>();
        info.Attacker = attacker;
        info.Defender = defender;
        info.Damage = damage;

        return info;
    }

    public void Clear()
    {
        Attacker = 0;
        Defender = 0;
        Damage = 0f;
        CriticalRate = 0f;
        CriticalMulti = 1f;
    }
}