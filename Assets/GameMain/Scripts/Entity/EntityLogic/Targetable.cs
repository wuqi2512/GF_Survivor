using StarForce;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : EntityLogicWithData, IPause
{
    public abstract float MaxHp { get; }
    public bool IsDead => Hp <= 0f;
    public abstract float Hp { get; protected set; }
    public abstract CampType Camp { get; }

    private Vector3 m_Velocity;
    private Vector3 m_MoveVelocity;
    private List<MovePreorder> m_ForceMove;
    protected Animator m_Animator;
    protected bool m_Pause;
    protected bool m_IsDestroy;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_Animator = GetComponentInChildren<Animator>();
        m_ForceMove = new List<MovePreorder>();
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_Velocity = Vector2.zero;
        m_MoveVelocity = Vector2.zero;
        m_ForceMove.Clear();
        m_Pause = false;
        m_IsDestroy = false;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (m_Pause || m_IsDestroy)
        {
            return;
        }
        
        m_Velocity = m_MoveVelocity;
        for (int i = m_ForceMove.Count - 1; i >= 0; i--)
        {
            MovePreorder movePreorder = m_ForceMove[i];
            m_Velocity += movePreorder.GetVelocity(elapseSeconds);
            if (movePreorder.IsFinish)
            {
                m_ForceMove.RemoveAt(i);
            }
        }
        CachedTransform.Translate(m_Velocity * Time.fixedDeltaTime);
    }

    public virtual void OnDead()
    {
        EffectData effectData = EffectData.Create((int)EffectType.DeadExplosionAnim, GameEntry.Entity.GenerateSerialId(), this.CachedTransform);
        GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(typeof(EffectAnimator), effectData, null));
    }

    public void SetMoveVelocity(Vector3 moveVelocity)
    {
        if (!Mathf.Approximately(moveVelocity.x, 0f))
        {
            Vector3 scale = CachedTransform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveVelocity.x);
            CachedTransform.localScale = scale;
        }

        m_MoveVelocity = moveVelocity;
    }

    public void AddForceMove(MovePreorder forceMove)
    {
        m_ForceMove.Add(forceMove);
    }

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;

        BeDamagedEventArgs args = BeDamagedEventArgs.Create(damage, this.Id, IsDead, base.Entity, CachedTransform.position);
        GameEntry.Event.Fire(this, args);

        if (IsDead)
        {
            Hp = 0f;
            OnDead();
            m_IsDestroy = true;
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