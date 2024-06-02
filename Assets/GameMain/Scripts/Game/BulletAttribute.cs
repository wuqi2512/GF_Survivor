public class BulletAttribute
{
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    /// <summary>
    /// 活跃时间
    /// </summary>
    public float ActiveTime { get; private set; }
    public float Scale { get; private set; }
    /// <summary>
    /// 弹射次数。碰到墙壁/屏幕边缘时触发
    /// </summary>
    public int Bounce { get; private set; }
    /// <summary>
    /// 穿透次数。
    /// </summary>
    public int Penetrate { get; private set; }
    /// <summary>
    /// 分裂次数。命中敌人（不管有没有穿透）时触发。分裂出来的一段时间内不会触发伤害，方向随机
    /// </summary>
    public int Splite { get; private set; }


    public BulletAttribute()
    {
        Damage = 0;
        MoveSpeed = 0;
        ActiveTime = 0f;
        Scale = 1f;
        Bounce = 0;
        Penetrate = 0;
        Splite = 0;
    }

    public BulletAttribute(int damage, float moveSpeed, float activeTime, float scale = 1, int bounce = 0, int penetrate = 0, int splite = 0)
    {
        Damage = damage;
        MoveSpeed = moveSpeed;
        ActiveTime = activeTime;
        Scale = scale;
        Bounce = bounce;
        Penetrate = penetrate;
        Splite = splite;
    }

    public static BulletAttribute operator +(BulletAttribute a, BulletAttribute b)
    {
        BulletAttribute result = new BulletAttribute();
        result.Damage = a.Damage + b.Damage;
        result.MoveSpeed = a.MoveSpeed + b.MoveSpeed;
        result.ActiveTime = a.ActiveTime + b.ActiveTime;
        result.Scale = a.Scale * b.Scale;
        result.Bounce = a.Bounce + b.Bounce;
        result.Penetrate = a.Penetrate + b.Penetrate;
        result.Splite = a.Splite + b.Splite;
        return result;
    }
}