using UnityEngine;
using UnityGameFramework.Runtime;

public class EffectLogic : EntityLogicWithData
{
    private EffectData m_EffectData;
    private float m_Timer;

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
        // TODO: 把表中的动画名自动hash
        CachedAnimator.Play(m_EffectData.DREffect.AnimName);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        m_Timer += elapseSeconds;
        if (m_Timer >= m_EffectData.DREffect.ActiveSeconds)
        {
            GameEntry.Entity.HideEntity(this);
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
    }
}