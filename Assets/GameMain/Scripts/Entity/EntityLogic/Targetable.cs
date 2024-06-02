using StarForce;
using UnityEngine;

public abstract class Targetable : EntityLogicWithData
{
    protected int m_Hp;

    public abstract int MaxHp { get; }
    public bool IsDead => m_Hp <= 0;
    public int Hp => m_Hp;
    public abstract CampType Camp { get; }

    public Rigidbody2D CachedRigidbody { get; protected set; }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        CachedRigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void OnDead()
    {
        CachedRigidbody.velocity = Vector3.zero;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        Rotate();
    }

    protected virtual void Rotate()
    {
        Vector3 scale = CachedTransform.localScale;
        scale.x = Mathf.Sign(CachedRigidbody.velocity.x) * Mathf.Abs(scale.x);
        CachedTransform.localScale = scale;
    }

    public virtual void TakeDamage(int damage)
    {
        m_Hp -= damage;

        BeDamagedEventArgs args = BeDamagedEventArgs.Create(damage, this.Id, IsDead, base.Entity, CachedTransform.position);
        GameEntry.Event.Fire(this, args);

        if (IsDead)
        {
            m_Hp = 0;
            OnDead();
        }
    }
}