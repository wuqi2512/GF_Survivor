using GameFramework;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class BulletData
{
    private static Vector3 AccelerationTween(BulletLogic bulletLogic, float elapseSeconds)
    {
        BulletData bulletData = bulletLogic.BulletData;
        float curSpeed = bulletData.m_DRBullet.MoveSpeed + bulletData.m_BlackBoard.GetData<VarSingle>("Acceleration") * bulletData.m_ActiveSconds;
        return new Vector3(curSpeed * elapseSeconds, 0f, 0f);
    }

    private static Vector3 NormalTween(BulletLogic bulletLogic, float elapseSeconds)
    {
        BulletData bulletData = bulletLogic.BulletData;
        return new Vector3(bulletData.m_DRBullet.MoveSpeed * elapseSeconds, 0f, 0f);
    }

    private static void BouncingOnHitWall(Transform transform)
    {
        Vector2 originDir = transform.right;
        Physics2D.RaycastNonAlloc(transform.position, originDir, s_Hits, 10f, 1 << Constant.Layer.WallMask);
        RaycastHit2D hit = s_Hits[0];
        Vector2 dir = originDir - (originDir * hit.normal) * hit.normal * 2;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private static void ShowEffect(int effectId, Transform follow, Action<Entity> onSuccess)
    {
        EffectData effectData = EffectData.Create(effectId, GameEntry.Entity.GenerateSerialId(), follow);
        GameEntry.Event.Fire(null, ShowEntityInLevelEventArgs.Create(typeof(EffectAnimator), effectData, onSuccess));
    }

    private static void ShowEffect(int effectId, Vector3 position, Action<Entity> onSuccess)
    {
        EffectData effectData = EffectData.Create(effectId, GameEntry.Entity.GenerateSerialId(), position);
        GameEntry.Event.Fire(null, ShowEntityInLevelEventArgs.Create(typeof(EffectAnimator), effectData, onSuccess));
    }

    private static void CastDamage(int attackerId, int targetId, int damage)
    {
        DamageInfo damageInfo = DamageInfo.Create(attackerId, targetId, damage);
        GameEntry.Event.Fire(null, DamageEventArgs.Create(damageInfo));
    }

    private static void CastDamages(int attackId, int damage, Targetable[] targetables, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CastDamage(attackId, targetables[i].Id, damage);
        }
    }

    private static void AddForceMove(Targetable[] targetables, int count, Vector3 position, float moveMagnitude, float duration)
    {
        for (int i = 0; i < count; i++)
        {
            Targetable target = targetables[i];
            Vector3 dir = (target.CachedTransform.position - position).normalized;
            target.AddForceMove(new MovePreorder(duration, dir * moveMagnitude));
        }
    }

    private static int GetTargetables(Collider2D[] collider2Ds, int count, Targetable[] targetables)
    {
        int i = 0;
        for (; i < count; i++)
        {
            Collider2D other = collider2Ds[i];
            Targetable targetable = other.gameObject.GetComponent<Targetable>();
            if (targetable != null)
            {
                targetables[i] = targetable;
            }
        }

        return i;
    }

    private static void SpliteBulletsWithRandomRotation(BulletLogic bulletLogic, int count = 1)
    {
        BulletData bulletData = bulletLogic.BulletData;

        for (int i = 0; i < count; i++)
        {
            BulletData newBulletData = BulletData.Create(bulletLogic.BulletData.m_DRBullet.Id, GameEntry.Entity.GenerateSerialId(),
                bulletData.Camp, bulletLogic.CachedTransform.position, Quaternion.Euler(0f, 0f, Utility.Random.GetRandomFloat(0, 360)));
            newBulletData.m_CanHitAfterCreated = 0.2f;
            GameEntry.Event.Fire(null, ShowEntityInLevelEventArgs.Create(typeof(BulletLogic), newBulletData));
        }
    }
}