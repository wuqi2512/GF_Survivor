using StarForce;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class BulletData
{
    private static Dictionary<string, BulletBehaviour> s_BulletBehaviours;
    static RaycastHit2D[] s_Hits = new RaycastHit2D[24];
    static Collider2D[] s_Collider2Ds = new Collider2D[24];
    static Targetable[] s_Targetables = new Targetable[24];
    static BulletData()
    {
        s_BulletBehaviours = new Dictionary<string, BulletBehaviour>();

        s_BulletBehaviours.Add("BouncingBall", new BulletBehaviour(BouncingBall_OnCreate, NormalTween, BouncingBall_OnHit, null));
        s_BulletBehaviours.Add("Kunai", new BulletBehaviour(Kunai_OnCrate, NormalTween, Kunai_OnHit, null));
        s_BulletBehaviours.Add("Bullet", new BulletBehaviour(Bullet_OnCreate, AccelerationTween, Bullet_OnHit, null));
        s_BulletBehaviours.Add("Bomb", new BulletBehaviour(Bomb_OnCreate, NormalTween, Bomb_OnHit, Bomb_OnDestroy));
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
        Targetable targetable = other.gameObject.GetComponent<Targetable>();
        BulletData bulletData = bulletLogic.BulletData;
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, bulletData.m_DRBullet.Damage);
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
        Targetable targetable = other.gameObject.GetComponent<Targetable>();
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, bulletData.m_DRBullet.Damage);
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
        Targetable targetable = other.gameObject.GetComponent<Targetable>();
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, bulletData.m_DRBullet.Damage);
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

    #region Bomb

    private static void Bomb_OnCreate(BulletLogic bulletLogic)
    {
        BlackBoard blackBoard = bulletLogic.BulletData.m_BlackBoard;
        blackBoard.SetData("ExplosionDamage", (VarInt32)10);
        blackBoard.SetData("ExplosionRadius", (VarSingle)0.7f);
        blackBoard.SetData("ExplosionForceMoveMagnitude", (VarSingle)5f);
        blackBoard.SetData("ExplosionForceMoveDuration", (VarSingle)0.3f);
        blackBoard.SetData("ExplosionEffectId", (VarInt32)(int)EffectType.ExplosionAnim);
    }

    private static void Bomb_OnHit(BulletLogic bulletLogic, Collider2D other)
    {
        BulletData bulletData = bulletLogic.BulletData;
        Targetable targetable = other.gameObject.GetComponent<Targetable>();
        if (targetable != null && bulletData.CanHitEntity(targetable))
        {
            CastDamage(bulletLogic.Id, targetable.Id, bulletData.m_DRBullet.Damage);
        }

        bulletLogic.DestroySelf();
    }

    private static void Bomb_OnDestroy(BulletLogic bulletLogic)
    {
        BlackBoard blackBoard = bulletLogic.BulletData.m_BlackBoard;
        int explosionDamage = blackBoard.GetData<VarInt32>("ExplosionDamage");
        float explosionRadius = blackBoard.GetData<VarSingle>("ExplosionRadius");
        int explosionEffectId = blackBoard.GetData<VarInt32>("ExplosionEffectId");
        float explosionForceMoveMagnitude = blackBoard.GetData<VarSingle>("ExplosionForceMoveMagnitude");
        float explosionForceMoveDuration = blackBoard.GetData<VarSingle>("ExplosionForceMoveDuration");
        int layerMask;
        switch (bulletLogic.BulletData.Camp)
        {
            case CampType.Player: layerMask = Constant.Layer.EnemyMask; break;
            default: Log.Warning("Explosion form {0} is not complecated.", bulletLogic.BulletData.Camp); return;
        }
        int count = Physics2D.OverlapCircleNonAlloc(bulletLogic.CachedTransform.position, explosionRadius, s_Collider2Ds, layerMask);
        int targetableCount = GetTargetables(s_Collider2Ds, count, s_Targetables);
        CastDamages(bulletLogic.Id, explosionDamage, s_Targetables, targetableCount);
        AddForceMove(s_Targetables, targetableCount, bulletLogic.CachedTransform.position, explosionForceMoveMagnitude, explosionForceMoveDuration);
        ShowEffect(explosionEffectId, bulletLogic.CachedTransform.position, null);
    }

    #endregion
}