using UnityEngine;
using GameFramework.ObjectPool;
using GameFramework;

public class DamageNumberItemObject : ObjectBase
{
    public static DamageNumberItemObject Create(object target)
    {
        var obj = ReferencePool.Acquire<DamageNumberItemObject>();
        obj.Initialize(target);

        return obj;
    }

    protected override void Release(bool isShutdown)
    {
        DamageNumberItem item = (DamageNumberItem)Target;
        if (item == null)
        {
            return;
        }

        Object.Destroy(item.gameObject);
    }
}
