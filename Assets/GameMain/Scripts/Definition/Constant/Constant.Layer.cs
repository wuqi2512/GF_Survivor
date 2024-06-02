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
        public static readonly int DefaultLayerId;
        public static readonly int UILayerId;

        public static readonly int Player;
        public static readonly int Enemy;
        public static readonly int PlayerBullet;
        public static readonly int Wall;

        static Layer()
        {
            DefaultLayerId = LayerMask.NameToLayer("Default");
            UILayerId = LayerMask.NameToLayer("UI");

            Player = LayerMask.NameToLayer("Player");
            Enemy = LayerMask.NameToLayer("Enemy");
            PlayerBullet = LayerMask.NameToLayer("PlayerBullet");
            Wall = LayerMask.NameToLayer("Wall");
        }
    }
}