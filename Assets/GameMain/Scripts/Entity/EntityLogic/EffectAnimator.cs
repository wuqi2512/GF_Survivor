using UnityEngine;
using UnityGameFramework.Runtime;

public class EffectAnimator : EntityLogicWithData, IPause
{
    private EffectData m_EffectData;
    private Animator m_Animator;
    private float m_Timer;
    private bool m_Pause;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_Animator = GetComponent<Animator>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_EffectData = userData as EffectData;
        if (m_EffectData == null)
        {
            Log.Error("EffectData is invalid.");
            return;
        }

        CachedTransform.localScale = m_EffectData.Scale;
        m_Animator.Play(m_EffectData.DREffect.AnimName);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause)
        {
            return;
        }

        m_Timer += elapseSeconds;
        if (m_Timer >= m_EffectData.DREffect.ActiveSeconds)
        {
            GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
            return;
        }

        if (m_EffectData.Follow != null)
        {
            CachedTransform.position = m_EffectData.Follow.position + m_EffectData.Offset;
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_EffectData = null;
        CachedTransform.localScale = Vector3.one;
        m_Timer = 0;
        m_Pause = false;
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