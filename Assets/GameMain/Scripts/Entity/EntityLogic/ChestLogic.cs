using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

public class ChestLogic : EntityLogicWithData, IPause
{
    private bool m_IsOpened;
    private Animator m_Animator;
    private float m_Timer;
    private bool m_Pause;
    private ChestData m_ChestData;

    private static readonly RangeInt ItemCountRange = new RangeInt(1, 4);
    private static readonly float ItemRangeMin = 0.5f;
    private static readonly float ItemRangeMax = 1f;

    private const float DestroyAfterOpened = 5f;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_Animator = GetComponentInChildren<Animator>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_ChestData = userData as ChestData;
        if (m_ChestData == null)
        {
            Log.Error("ChestData is invalid.");
        }

        m_Animator.Play(Constant.Anim.ChestClosed);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause)
        {
            return;
        }

        if (m_IsOpened)
        {
            m_Timer += elapseSeconds;
            if (m_Timer >= DestroyAfterOpened)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(typeof(EffectAnimator),
                    EffectData.Create((int)EffectType.DeadExplosionAnim, GameEntry.Entity.GenerateSerialId(), CachedTransform.position)));
            }
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_IsOpened = false;
        m_Timer = 0.0f;
        m_Pause = false;
    }

    private void CreateDropItem()
    {
        int count = Utility.Random.GetRandom(ItemCountRange.start, ItemCountRange.end);

        for (int i = 1; i <= count; i++)
        {
            DropPool.DropItem item = m_ChestData.DropPool.RandomItem();
            Vector3 pos = CachedTransform.position + (Vector3)Utility.Random.GetRandomVector2(ItemRangeMin, ItemRangeMax);
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                typeof(DropItemLogic),
                DropItemData.Create(GameEntry.Entity.GenerateSerialId(), item.ItemType, item.Value, pos), null));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_IsOpened)
        {
            return;
        }

        m_IsOpened = true;
        m_Animator.Play(Constant.Anim.ChestOpen);
        CreateDropItem();
    }
}