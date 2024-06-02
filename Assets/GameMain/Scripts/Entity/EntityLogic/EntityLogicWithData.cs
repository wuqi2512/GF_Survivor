//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public abstract class EntityLogicWithData : EntityLogic
{
    [SerializeField]
    private EntityData m_EntityData = null;

    public int Id
    {
        get
        {
            return Entity.Id;
        }
    }

    public Animator CachedAnimator
    {
        get;
        private set;
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        CachedAnimator = GetComponent<Animator>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_EntityData = userData as EntityData;
        if (m_EntityData == null)
        {
            Log.Error("Entity data is invalid.");
            return;
        }

        Name = Utility.Text.Format("[Entity {0}]", Id);
        CachedTransform.localPosition = m_EntityData.Position;
        CachedTransform.localRotation = m_EntityData.Rotation;
        CachedTransform.localScale = Vector3.one;
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        ReferencePool.Release(m_EntityData);
    }
}