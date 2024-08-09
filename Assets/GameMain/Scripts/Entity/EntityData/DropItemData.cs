using cfg;
using GameFramework;
using StarForce;
using UnityEngine;

public class DropItemData : EntityData
{
    private int m_EntityId;
    private DropItemType m_DropItemType;
    private int m_Value;
    private string m_SpriteAssetPath;

    public override int EntityId => m_EntityId;
    public DropItemType DropItemType => m_DropItemType;
    public int Value => m_Value;
    public string SpriteAssetPath => m_SpriteAssetPath;

    public static DropItemData Create(int serialId, DropItemType dropItemType, int value, Vector3 position)
    {
        DropItemData dropItemData = ReferencePool.Acquire<DropItemData>();
        dropItemData.m_EntityId = (int)EntityType.DropItem;
        dropItemData.m_SerialId = serialId;
        dropItemData.m_DropItemType = dropItemType;
        dropItemData.m_Value = value;
        dropItemData.m_Position = position;

        switch (dropItemType)
        {
            case DropItemType.Coin: dropItemData.m_SpriteAssetPath = Constant.Game.CoinSpriteAssetPath; break;
            case DropItemType.Diamond: dropItemData.m_SpriteAssetPath = Constant.Game.DiamondSpriteAssetPath; break;
            case DropItemType.Equipment:
                Equipment equipment = GameEntry.Luban.Tables.TbEquipment.Get(value);
                dropItemData.m_SpriteAssetPath = AssetUtility.GetEquipmentSpriteAsset(equipment.AssetName);
                break;
        }

        return dropItemData;
    }

    public override void Clear()
    {
        base.Clear();

        m_EntityId = 0;
        m_DropItemType = DropItemType.None;
        m_Value = 0;
        m_SpriteAssetPath = null;
    }
}

public enum DropItemType
{
    None,
    Coin,
    Diamond,
    Equipment,
}