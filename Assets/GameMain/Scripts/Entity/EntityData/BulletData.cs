using cfg;
using GameFramework;
using StarForce;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

[Serializable]
public partial class BulletData : EntityData
{
    public delegate void BulletOnCreate(BulletLogic bulletLogic);
    public delegate Vector3 BulletMoveTween(BulletLogic bulletLogic, float elapseSeconds);
    public delegate void BulletOnHit(BulletLogic bulletLogic, Collider2D other);
    public delegate void BulletOnDestroy(BulletLogic bulletLogic);

    protected Bullet m_Bullet;
    protected CampType m_CampType;
    /// <summary>
    /// 创建后多久可以命中实体
    /// </summary>
    protected float m_CanHitAfterCreated;
    /// <summary>
    /// 命中同一实体的间隔
    /// </summary>
    protected const float m_SameTargetInterval = 0.3f;
    /// <summary>
    /// 记录命中实体。Key: serialId, Value: activeTimer
    /// </summary>
    protected Dictionary<int, float> m_HitRecord;
    protected BlackBoard m_BlackBoard;
    protected float m_ActiveSconds;
    protected float m_Scale;
    protected BulletBehaviour m_BulletBehaviour;

    public BulletData()
    {
        m_HitRecord = new Dictionary<int, float>();
        m_Scale = 1f;
    }

    public override int EntityId => m_Bullet.EntityId;
    public CampType Camp => m_CampType;
    public float Scale => m_Scale;
    public BulletBehaviour Behaviour => m_BulletBehaviour;

    public static BulletData Create(int bulletId, int serialId, CampType campType, Vector3 position, Quaternion rotation)
    {
        BulletData bulletData = ReferencePool.Acquire<BulletData>();
        bulletData.m_Bullet = GameEntry.Luban.Tables.TbBullet.GetOrDefault(bulletId);
        bulletData.m_CampType = campType;
        bulletData.m_SerialId = serialId;
        bulletData.m_Position = position;
        bulletData.m_Rotation = rotation;

        bulletData.m_BlackBoard = BlackBoard.Create();
        if (!s_BulletBehaviours.TryGetValue(bulletData.m_Bullet.BehaviourKey, out bulletData.m_BulletBehaviour))
        {
            Log.Error("Can't find BulletBehaviour '{0}.'", bulletData.m_Bullet.BehaviourKey);
        }

        return bulletData;
    }

    public override void Clear()
    {
        base.Clear();

        m_Bullet = null;
        m_CampType = CampType.Unknown;
        m_CanHitAfterCreated = 0f;
        m_HitRecord.Clear();
        ReferencePool.Release(m_BlackBoard);
        m_BlackBoard = null;
        m_ActiveSconds = 0f;
        m_Scale = 1f;
    }

    public bool UpdateActiveTime(float elapseSeconds)
    {
        m_ActiveSconds += elapseSeconds;
        return m_ActiveSconds >= m_Bullet.ActiveTime;
    }

    public bool CanHitEntity(Targetable target)
    {
        if (m_CanHitAfterCreated > m_ActiveSconds)
        {
            return false;
        }

        RelationType relation = AIUtility.GetRelation(target.Camp, Camp);
        if (relation != RelationType.Hostile)
        {
            return false;
        }

        if (m_HitRecord.ContainsKey(target.Id))
        {
            if (m_HitRecord[target.Id] + m_SameTargetInterval > m_ActiveSconds)
            {
                return false;
            }
        }

        return true;
    }

    public void AddHitRecord(int serialId)
    {
        m_HitRecord[serialId] = m_ActiveSconds;
    }
}