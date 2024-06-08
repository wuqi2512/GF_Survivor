using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class BulletLogic : EntityLogicWithData, IPause
{
    public BulletData BulletData { get; private set; }
    private bool m_IsDestroyed;
    private bool m_Pause;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        BulletData = userData as BulletData;
        if (BulletData == null)
        {
            Log.Error("BulletData is invalid.");
        }

        float scale = BulletData.Scale;
        CachedTransform.localScale = new Vector3(scale, scale, 1f);
        switch (BulletData.Camp)
        {
            case CampType.Player: gameObject.SetLayerRecursively(Constant.Layer.PlayerBulletId); break;
        }

        BulletData.Behaviour.OnCreate(this);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause || m_IsDestroyed)
        {
            return;
        }

        if (BulletData.UpdateActiveTime(elapseSeconds))
        {
            DestroySelf();
            return;
        }

        CachedTransform.Translate(BulletData.Behaviour.MoveTween(this, elapseSeconds));
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        BulletData.Behaviour.OnDestroy(this);

        BulletData = null;
        m_IsDestroyed = false;
        m_Pause = false;
    }

    public void DestroySelf()
    {
        GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
        m_IsDestroyed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Pause || m_IsDestroyed)
        {
            return;
        }
        
        BulletData.Behaviour.OnHit(this, other);
    }

    public void Pause()
    {
        m_Pause = true;
    }

    public void Resume()
    {
        m_Pause = false;
    }
}