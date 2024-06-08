using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class BulletData
{
    private static Dictionary<string, BulletBehaviour> s_BulletBehaviours;

    static BulletData()
    {
        s_BulletBehaviours = new Dictionary<string, BulletBehaviour>();

        s_BulletBehaviours.Add("BouncingBall", new BulletBehaviour(BouncingBall_OnCreate, NormalTween, BouncingBall_OnHit, null));
        s_BulletBehaviours.Add("Kunai", new BulletBehaviour(Kunai_OnCrate, NormalTween, Kunai_OnHit, null));
        s_BulletBehaviours.Add("Bullet", new BulletBehaviour(Bullet_OnCreate, AccelerationTween, Bullet_OnHit, null));
    }

    static RaycastHit2D[] s_Hits = new RaycastHit2D[24];

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

    private static void CastDamage(int id, int targetId, float hitDegree, int damage)
    {
        DamageInfo damageInfo = DamageInfo.Create(id, targetId, hitDegree, damage);
        GameEntry.Event.Fire(null, DamageEventArgs.Create(damageInfo));
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


    #region BoucingBall
    private static void BouncingBall_OnCreate(BulletLogic bulletLogic)
    {
        BulletData bulletData = bulletLogic.BulletData;
        BlackBoard blackBoard = bulletData.m_BlackBoard;
        blackBoard.SetData("Bounce", (VarInt32)3);
        blackBoard.SetData("BounceCounter", (VarInt32)0);
    }

    private static void BouncingBall_OnHit(BulletLogic bulletLogic, Collider2D other)
    {
        Targetable targetable = other.gameObject.GetComponentInParent<Targetable>();
        BulletData bulletData = bulletLogic.BulletData;
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, 0f, bulletData.m_DRBullet.Damage);
            bulletLogic.DestroySelf();

            return;
        }

        if (other.gameObject.layer == Constant.Layer.WallId)
        {
            int bounce = bulletData.m_BlackBoard.GetData<VarInt32>("Bounce");
            int bounceCounter = bulletData.m_BlackBoard.GetData<VarInt32>("BounceCounter");

            if (bounceCounter < bounce)
            {
                BouncingOnHitWall(bulletLogic.CachedTransform);

                bounceCounter++;
                bulletData.m_BlackBoard.SetData("BounceCounter", (VarInt32)bounceCounter);
            }
            else
            {
                bulletLogic.DestroySelf();
            }
        }
    }

    #endregion

    #region Kunai

    private static void Kunai_OnCrate(BulletLogic bulletLogic)
    {
        BulletData bulletData = bulletLogic.BulletData;
        bulletData.m_BlackBoard.SetData("SpliteCount", (VarInt32)5);
        bulletData.m_BlackBoard.SetData("SpliteCounter", (VarInt32)0);
        bulletData.m_BlackBoard.SetData("SpliteChildCount", (VarInt32)3);
    }

    private static void Kunai_OnHit(BulletLogic bulletLogic, Collider2D other)
    {
        BulletData bulletData = bulletLogic.BulletData;
        Targetable targetable = other.gameObject.GetComponentInParent<Targetable>();
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, 0f, bulletData.m_DRBullet.Damage);
            bulletLogic.DestroySelf();

            int spliteCount = bulletData.m_BlackBoard.GetData<VarInt32>("SpliteCount");
            int spliteCounter = bulletData.m_BlackBoard.GetData<VarInt32>("SpliteCounter");
            if (spliteCounter < spliteCount)
            {
                int childCount = bulletData.m_BlackBoard.GetData<VarInt32>("SpliteChildCount");
                SpliteBulletsWithRandomRotation(bulletLogic, childCount);
            }

            return;
        }

        if (other.gameObject.layer == Constant.Layer.WallId)
        {
            bulletLogic.DestroySelf();
        }
    }

    #endregion

    #region Bullet

    private static void Bullet_OnCreate(BulletLogic bulletLogic)
    {
        BulletData bulletData = bulletLogic.BulletData;
        bulletData.m_BlackBoard.SetData("Acceleration", (VarSingle)2f);
        bulletData.m_BlackBoard.SetData("Penetrates", (VarInt32)5);
        bulletData.m_BlackBoard.SetData("PenetrateCounter", (VarInt32)0);
    }

    private static void Bullet_OnHit(BulletLogic bulletLogic, Collider2D other)
    {
        BulletData bulletData = bulletLogic.BulletData;
        Targetable targetable = other.gameObject.GetComponentInParent<Targetable>();
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, 0f, bulletData.m_DRBullet.Damage);
            bulletData.AddHitRecord(targetable.Id);

            int penetrates = bulletData.m_BlackBoard.GetData<VarInt32>("Penetrates");
            int penetrateCounter = bulletData.m_BlackBoard.GetData<VarInt32>("PenetrateCounter");

            if (penetrateCounter < penetrates)
            {
                bulletData.m_BlackBoard.SetData("PenetrateCounter", (VarInt32)(++penetrateCounter));
            }
            else
            {
                bulletLogic.DestroySelf();
            }

            return;
        }

        if (other.gameObject.layer == Constant.Layer.WallId)
        {
            bulletLogic.DestroySelf();
        }
    }

    #endregion
}