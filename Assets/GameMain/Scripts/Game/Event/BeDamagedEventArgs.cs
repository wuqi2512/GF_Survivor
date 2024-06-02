using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BeDamagedEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(BeDamagedEventArgs).GetHashCode();

    public override int Id => EventId;

    public int Damage { get; private set; }
    public int SerialId { get; private set; }
    public bool Dead { get; private set; }
    public Entity Entity { get; private set; }
    public Vector3 Position { get; private set; }

    public static BeDamagedEventArgs Create(int damage, int serialId, bool dead, Entity entity, Vector3 position)
    {
        BeDamagedEventArgs args = ReferencePool.Acquire<BeDamagedEventArgs>();
        args.Damage = damage;
        args.SerialId = serialId;
        args.Dead = dead;
        args.Entity = entity;
        args.Position = position;

        return args;
    }

    public override void Clear()
    {
        Damage = -1;
        SerialId = 0;
        Dead = false;
        Entity = null;
        Position = Vector3.zero;
    }
}