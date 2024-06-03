using StarForce;
using UnityEngine;

public abstract class Targetable : EntityLogicWithData, IPause
{
    protected int m_Hp;

    public abstract int MaxHp { get; }
    public bool IsDead => m_Hp <= 0;
    public int Hp => m_Hp;
    public abstract CampType Camp { get; }

    private Vector2 m_Velocity;
    protected Animator m_Animator;
    protected bool m_Pause;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_Animator = GetComponentInChildren<Animator>();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_Velocity = Vector2.zero;
        m_Pause = false;
    }

    private void FixedUpdate()
    {
        if (!m_Pause)
        {
            CachedTransform.Translate(m_Velocity * Time.fixedDeltaTime);
        }
    }

    public virtual void OnDead()
    {

    }

    public void SetVelocity(Vector2 moveVelocity, Vector2 forceVelocity)
    {
        if (!Mathf.Approximately(moveVelocity.x, 0f))
        {
            Vector3 scale = CachedTransform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveVelocity.x);
            CachedTransform.localScale = scale;
        }

        m_Velocity = moveVelocity + forceVelocity;
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

    public void Pause()
    {
        m_Pause = true;
        m_Animator.speed = 0f;
    }

    public void Resume()
    {
        m_Pause = false;
        m_Animator.speed = 1f;
    }
}