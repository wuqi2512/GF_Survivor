//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class AIUtility
    {
        private static Dictionary<CampPair, RelationType> s_CampPairToRelation = new Dictionary<CampPair, RelationType>();
        private static Dictionary<KeyValuePair<CampType, RelationType>, CampType[]> s_CampAndRelationToCamps = new Dictionary<KeyValuePair<CampType, RelationType>, CampType[]>();

        static AIUtility()
        {
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral), RelationType.Neutral);
        }

        /// <summary>
        /// 获取两个阵营之间的关系。
        /// </summary>
        /// <param name="first">阵营一。</param>
        /// <param name="second">阵营二。</param>
        /// <returns>阵营间关系。</returns>
        public static RelationType GetRelation(CampType first, CampType second)
        {
            if (first > second)
            {
                CampType temp = first;
                first = second;
                second = temp;
            }

            RelationType relationType;
            if (s_CampPairToRelation.TryGetValue(new CampPair(first, second), out relationType))
            {
                return relationType;
            }

            Log.Warning("Unknown relation between '{0}' and '{1}'.", first.ToString(), second.ToString());
            return RelationType.Unknown;
        }

        /// <summary>
        /// 获取和指定具有特定关系的所有阵营。
        /// </summary>
        /// <param name="camp">指定阵营。</param>
        /// <param name="relation">关系。</param>
        /// <returns>满足条件的阵营数组。</returns>
        public static CampType[] GetCamps(CampType camp, RelationType relation)
        {
            KeyValuePair<CampType, RelationType> key = new KeyValuePair<CampType, RelationType>(camp, relation);
            CampType[] result = null;
            if (s_CampAndRelationToCamps.TryGetValue(key, out result))
            {
                return result;
            }

            // TODO: GC Alloc
            List<CampType> camps = new List<CampType>();
            Array campTypes = Enum.GetValues(typeof(CampType));
            for (int i = 0; i < campTypes.Length; i++)
            {
                CampType campType = (CampType)campTypes.GetValue(i);
                if (GetRelation(camp, campType) == relation)
                {
                    camps.Add(campType);
                }
            }

            // TODO: GC Alloc
            result = camps.ToArray();
            s_CampAndRelationToCamps[key] = result;

            return result;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(EntityLogicWithData fromEntity, EntityLogicWithData toEntity)
        {
            Transform fromTransform = fromEntity.CachedTransform;
            Transform toTransform = toEntity.CachedTransform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        [StructLayout(LayoutKind.Auto)]
        private struct CampPair
        {
            private readonly CampType m_First;
            private readonly CampType m_Second;

            public CampPair(CampType first, CampType second)
            {
                m_First = first;
                m_Second = second;
            }

            public CampType First
            {
                get
                {
                    return m_First;
                }
            }

            public CampType Second
            {
                get
                {
                    return m_Second;
                }
            }
        }
    }
}
