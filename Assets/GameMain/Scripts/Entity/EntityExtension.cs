//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;
using StarForce;

public static class EntityExtension
{
    // 关于 EntityId 的约定：
    // 0 为无效
    // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
    // 负值用于本地生成的临时实体（如特效、FakeObject等）
    private static int s_SerialId = 0;

    public static EntityLogicWithData GetGameEntity(this EntityComponent entityComponent, int entityId)
    {
        UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
        if (entity == null)
        {
            return null;
        }

        return (EntityLogicWithData)entity.Logic;
    }

    public static void HideEntity(this EntityComponent entityComponent, EntityLogicWithData entity)
    {
        entityComponent.HideEntity(entity.Entity);
    }

    public static void AttachEntity(this EntityComponent entityComponent, EntityLogicWithData entity, int ownerId, string parentTransformPath = null, object userData = null)
    {
        entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
    }

    public static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
    {
        if (data == null)
        {
            Log.Warning("Data is invalid.");
            return;
        }

        IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
        DREntity drEntity = dtEntity.GetDataRow(data.EntityId);
        if (drEntity == null)
        {
            Log.Warning("Can not load entity id '{0}' from data table.", data.EntityId.ToString());
            return;
        }

        entityComponent.ShowEntity(data.SerialId, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
    }

    public static void ShowBullet(this EntityComponent entityComponent, EntityData data)
    {
        entityComponent.ShowEntity(typeof(BulletLogic), "Bullet", Constant.AssetPriority.BulletAsset, data);
    }

    public static void ShowEffect(this EntityComponent entityComponent, EntityData data)
    {
        entityComponent.ShowEntity(typeof(EffectLogic), "Effect", Constant.AssetPriority.EffectAsset, data);
    }

    public static void ShowDefault(this EntityComponent entityComponent, Type logicType, EntityData data)
    {
        entityComponent.ShowEntity(logicType, "Default", Constant.AssetPriority.DefaultAsset, data);
    }

    public static int GenerateSerialId(this EntityComponent entityComponent)
    {
        return --s_SerialId;
    }
}
