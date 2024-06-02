using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BulletLogic : EntityLogicWithData
{
    [SerializeField] private BulletData m_BulletData;
    private AttributeCounter m_AttributeCounter;
    private Rigidbody2D m_RigidBody;

    public CampType Camp => m_BulletData.Camp;
    private BulletAttribute m_BulletAttribute => m_BulletData.BulletAttri;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_AttributeCounter = new AttributeCounter();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_BulletData = userData as BulletData;
        if (m_BulletData == null)
        {
            Log.Error("BulletData is invalid.");
        }

        m_AttributeCounter.Clear();
        float scale = m_BulletData.BulletAttri.Scale;
        CachedTransform.localScale = new Vector3(scale, scale, 0f);
        switch (m_BulletData.Camp)
        {
            case CampType.Player: gameObject.SetLayerRecursively(Constant.Layer.PlayerBullet); break;
        }

        m_RigidBody.velocity = CachedTransform.right * m_BulletAttribute.MoveSpeed;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        m_AttributeCounter.ActiveTime += Time.deltaTime;
        if (m_AttributeCounter.ActiveTime > m_BulletAttribute.ActiveTime)
        {
            GameEntry.Entity.HideEntity(this);
            return;
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
                GameEntry.Entity.HideEntity(this);
            }
            return;
        }

        RelationType relation = AIUtility.GetRelation(targetable.Camp, this.Camp);
        if (relation != RelationType.Hostile)
        {
            return;
        }

        DamageInfo damageInfo = DamageInfo.Create(Id, targetable.Id, 0f, m_BulletAttribute.Damage);
        GameEntry.Event.Fire(this, DamageEventArgs.Create(damageInfo));

        if (m_AttributeCounter.Splite < m_BulletAttribute.Splite)
        {
            m_AttributeCounter.Splite++;
            GameEntry.Entity.ShowBullet(BulletData.Create(m_BulletData.DRBullet.Id, GameEntry.Entity.GenerateSerialId(),
                m_BulletData.Camp, CachedTransform.position, Quaternion.Euler(0f, 0f, Random.Range(0, 360))));
        }
        if (m_AttributeCounter.Penetrate < m_BulletAttribute.Penetrate)
        {
            m_AttributeCounter.Penetrate++;
            return;
        }
        GameEntry.Entity.HideEntity(this);
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