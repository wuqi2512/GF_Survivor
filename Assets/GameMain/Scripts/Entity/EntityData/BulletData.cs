using GameFramework;
using GameFramework.DataTable;
using StarForce;
using System;
using UnityEngine;

[Serializable]
public class BulletData : EntityData
{
    [SerializeField] protected DRBullet m_DRBullet;
    [SerializeField] protected CampType m_CampType;
    [SerializeField] protected BulletAttribute m_TotalBulletAttri;

    public override int EntityId => m_DRBullet.EntityId;
    public DRBullet DRBullet => m_DRBullet;
    public CampType Camp => m_CampType;
    public BulletAttribute BulletAttri => m_TotalBulletAttri;

    public static BulletData Create(int bulletId, int serialId, CampType campType, Vector3 position, Quaternion rotation)
    {
        BulletData bulletData = ReferencePool.Acquire<BulletData>();

        IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet>();
        bulletData.m_DRBullet = dtBullet.GetDataRow(bulletId);
        bulletData.m_CampType = campType;

        bulletData.m_SerialId = serialId;
        bulletData.m_Position = position;
        bulletData.m_Rotation = rotation;

        bulletData.m_TotalBulletAttri = new BulletAttribute(10, 5f, 5f, 1, 10, 10, 10);

        return bulletData;
    }

    public override void Clear()
    {
        base.Clear();

        m_DRBullet = null;
        m_CampType = CampType.Unknown;
    }
}