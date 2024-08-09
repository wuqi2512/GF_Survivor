//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

public static partial class Constant
{
    /// <summary>
    /// 层。
    /// </summary>
    public static class Layer
    {
        public static readonly int DefaultId;
        public static readonly int UIId;
        public static readonly int PlayerId;
        public static readonly int EnemyId;
        public static readonly int PlayerBulletId;
        public static readonly int WallId;
        public static readonly int DropItemId;

        public static readonly int EnemyMask;
        public static readonly int WallMask;

        static Layer()
        {
            DefaultId = LayerMask.NameToLayer("Default");
            UIId = LayerMask.NameToLayer("UI");
            PlayerId = LayerMask.NameToLayer("Player");
            EnemyId = LayerMask.NameToLayer("Enemy");
            PlayerBulletId = LayerMask.NameToLayer("PlayerBullet");
            WallId = LayerMask.NameToLayer("Wall");
            DropItemId = LayerMask.NameToLayer("DropItem");

            EnemyMask = 1 << EnemyId;
            WallMask = 1 << WallId;
        }
    }
}