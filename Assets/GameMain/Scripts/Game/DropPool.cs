using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

public class DropPool
{
    public struct DropItem
    {
        public DropItemType ItemType;
        public int Value;

        public DropItem(DropItemType itemType, int value)
        {
            ItemType = itemType;
            Value = value;
        }
    }

    private List<int> m_Weights;
    private List<DropItem> m_Items;
    private int m_TotalWeight;

    public DropPool()
    {
        m_Weights = new List<int>();
        m_Items = new List<DropItem>();
    }

    public void Add(DropItem item, int weight)
    {
        m_Items.Add(item);
        m_TotalWeight += weight;
        m_Weights.Add(m_TotalWeight);
    }

    public DropItem RandomItem()
    {
        int value = Utility.Random.GetRandom(0, m_TotalWeight);
        int index;
        for (index = 0; index < m_Weights.Count; index++)
        {
            if (m_Weights[index] > value)
            {
                break;
            }
        }
        
        return m_Items[index];
    }
}