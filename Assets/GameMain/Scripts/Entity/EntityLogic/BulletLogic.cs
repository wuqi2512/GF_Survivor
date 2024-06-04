using StarForce;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BulletLogic : EntityLogicWithData, IPause
{
    [SerializeField] private BulletData m_BulletData;
    private AttributeCounter m_AttributeCounter;
    protected bool m_IsDestroyed;
    protected bool m_Pause;
    /// <summary>
    /// 创建后多久可以命中实体
    /// </summary>
    protected float m_CanHitAfterCreated;
    /// <summary>
    /// 记录命中实体。Key: serialId, Value: activeTimer
    /// </summary>
    protected Dictionary<int, float> m_HitRecord;
    /// <summary>
    /// 命中同一实体的间隔
    /// </summary>
    protected float m_SameHitInterval = 0.5f;

    public CampType Camp => m_BulletData.Camp;
    private BulletAttribute m_BulletAttribute => m_BulletData.BulletAttri;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_AttributeCounter = new AttributeCounter();
        m_HitRecord = new Dictionary<int, float>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_BulletData = userData as BulletData;
        if (m_BulletData == null)
        {
            Log.Error("BulletData is invalid.");
        }

        float scale = m_BulletData.BulletAttri.Scale;
        CachedTransform.localScale = new Vector3(scale, scale, 1f);
        switch (m_BulletData.Camp)
        {
            case CampType.Player: gameObject.SetLayerRecursively(Constant.Layer.PlayerBullet); break;
        }
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause && m_IsDestroyed)
        {
            return;
        }

        m_AttributeCounter.ActiveTime += Time.deltaTime;
        if (m_AttributeCounter.ActiveTime > m_BulletAttribute.ActiveTime)
        {
            GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
            m_IsDestroyed = true;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (m_Pause && m_IsDestroyed)
        {
            return;
        }

        CachedTransform.Translate(CachedTransform.right * m_BulletAttribute.MoveSpeed * Time.fixedDeltaTime, Space.World);
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_BulletData = null;
        m_AttributeCounter.Clear();
        m_IsDestroyed = false;
        m_Pause = false;
        m_CanHitAfterCreated = 0f;
        m_HitRecord.Clear();
    }

    public void SetCanHitAfterCreated(float canHitAfterCreated)
    {
        m_CanHitAfterCreated = canHitAfterCreated;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Pause && m_IsDestroyed)
        {
            return;
        }

        Targetable targetable = other.gameObject.GetComponentInParent<Targetable>();
        // 撞到墙壁
        if (targetable == null)
        {
            if (m_AttributeCounter.Bounce < m_BulletAttribute.Bounce)
            {
                m_AttributeCounter.Bounce++;
                Vector2 originDir = CachedTransform.right;
                RaycastHit2D hit = Physics2D.Raycast(CachedTransform.position, originDir, 10, Constant.Layer.Wall);
                Vector2 dir = originDir - (originDir * hit.normal) * hit.normal * 2;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                CachedTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
                m_IsDestroyed = true;
            }
            return;
        }

        if (m_CanHitAfterCreated > m_AttributeCounter.ActiveTime)
        {
            return;
        }

        RelationType relation = AIUtility.GetRelation(targetable.Camp, this.Camp);
        if (relation != RelationType.Hostile)
        {
            return;
        }

        if (m_HitRecord.ContainsKey(targetable.Id))
        {
            if (m_HitRecord[targetable.Id] + m_SameHitInterval > m_AttributeCounter.ActiveTime)
            {
                return;
            }
        }

        DamageInfo damageInfo = DamageInfo.Create(Id, targetable.Id, 0f, m_BulletAttribute.Damage);
        GameEntry.Event.Fire(this, DamageEventArgs.Create(damageInfo));
        m_HitRecord[targetable.Id] = m_AttributeCounter.ActiveTime;

        if (m_AttributeCounter.Splite < m_BulletAttribute.Splite)
        {
            m_AttributeCounter.Splite++;

            BulletData bulletData = BulletData.Create(m_BulletData.DRBullet.Id, GameEntry.Entity.GenerateSerialId(),
                m_BulletData.Camp, CachedTransform.position, Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(typeof(BulletLogic), bulletData, (entity) =>
            {
                BulletLogic bulletLogic = entity.Logic as BulletLogic;
                if (bulletLogic == null)
                {
                    Log.Error("BulletLogic is invalid.");
                }

                bulletLogic.SetCanHitAfterCreated(0.5f);
            }));
        }
        if (m_AttributeCounter.Penetrate < m_BulletAttribute.Penetrate)
        {
            m_AttributeCounter.Penetrate++;
            return;
        }

        GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
        m_IsDestroyed = true;
    }

    public void Pause()
    {
        m_Pause = true;
    }

    public void Resume()
    {
        m_Pause = false;
    }

    /// <summary>
    /// 为BulletAttribute中的对应属性的功能的实现计数
    /// </summary>
    private class AttributeCounter
    {
        public float ActiveTime;
        public int Bounce;
        public int Penetrate;
        public int Splite;

        public AttributeCounter()
        {
            ActiveTime = 0f;
            Bounce = 0;
            Penetrate = 0;
            Splite = 0;
        }

        public void Clear()
        {
            ActiveTime = 0f;
            Bounce = 0;
            Penetrate = 0;
            Splite = 0;
        }
    }
}