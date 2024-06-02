using GameFramework;
using GameFramework.Event;
using System;
using UnityGameFramework.Runtime;

public class ShowEntityInLevelEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(ShowEntityInLevelEventArgs).GetHashCode();

    public override int Id => EventId;

    public Type LogicType { get; private set; }
    public EntityData EntityData { get; private set; }
    public Action<Entity> ShowSuccess { get; private set; }

    public static ShowEntityInLevelEventArgs Create(Type logicType, EntityData entityData, Action<Entity> showSuccess = null)
    {
        var args = ReferencePool.Acquire<ShowEntityInLevelEventArgs>();
        args.LogicType = logicType;
        args.EntityData = entityData;
        args.ShowSuccess = showSuccess;

        return args;
    }

    public override void Clear()
    {
        LogicType = null;
        EntityData = null;
        ShowSuccess = null;
    }
}