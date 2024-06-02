//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using UnityEngine;

[Serializable]
public abstract class EntityData : IReference
{
    [SerializeField]
    protected int m_SerialId = 0;

    [SerializeField]
    protected Vector3 m_Position = Vector3.zero;

    [SerializeField]
    protected Quaternion m_Rotation = Quaternion.identity;


    public EntityData()
    {

    }


    public int SerialId => m_SerialId;

    public abstract int EntityId { get; }

    public Vector3 Position
    {
        get
        {
            return m_Position;
        }
        set
        {
            m_Position = value;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return m_Rotation;
        }
        set
        {
            m_Rotation = value;
        }
    }

    public virtual void Clear()
    {
        m_SerialId = 0;
        m_Position = Vector3.zero;
        m_Rotation = Quaternion.identity;
    }
}