using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDiagram : MonoBehaviour
{
    private RectTransform m_RectTrans;
    private List<RotationDiagramItem> m_Items;
    private List<ItemPosData> m_ItemPosDatas;
    public Sprite[] sprites;
    public float MaxScale;
    public float MinScale;

    public void Start()
    {
        m_RectTrans = GetComponent<RectTransform>();
        m_ItemPosDatas = new List<ItemPosData>();
        m_Items = new List<RotationDiagramItem>();
        CreateInstances(this.transform, sprites.Length, m_Items);
        InitItems();
        CalculateItemPosData(m_ItemPosDatas, m_Items.Count);
        SetPosData();
    }

    private void InitItems()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            RotationDiagramItem item = m_Items[i];
            item.Init();
            item.PosId = i;
            item.SetSprite(sprites[i]);
            item.AddListener(OnDrageItem);
        }
    }

    private void SetPosData()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i].SetPosData(m_ItemPosDatas[m_Items[i].PosId]);
        }
    }

    private void CalculateItemPosData(List<ItemPosData> list, int count)
    {
        float ratio = 0f;
        float deltaRatio = 1f / count;
        float length = m_RectTrans.rect.width * 2;
        float scaleLength = (MaxScale - MinScale) * 2;
        for (int i = 0; i < count; i++)
        {
            ItemPosData itemPosData = new ItemPosData();
            itemPosData.Id = i;
            itemPosData.PosX = CalculatePosX(ratio, length);
            itemPosData.Scale = CalculateScale(ratio, scaleLength, MaxScale);
            itemPosData.Order = CalculateOrder(i, count);
            list.Add(itemPosData);

            ratio += deltaRatio;
        }
    }

    private float CalculatePosX(float ratio, float length)
    {
        if (0f <= ratio && ratio <= 0.25f)
        {
            return ratio * length;
        }
        else if (0.25f < ratio && ratio <= 0.75f)
        {
            return (0.50f - ratio) * length;
        }
        else
        {
            return (ratio - 1.00f) * length;
        }
    }

    private float CalculateScale(float ratio, float length, float maxScale)
    {
        if (0 <= ratio && ratio <= 0.50f)
        {
            return maxScale - ratio * length;
        }
        else
        {
            return maxScale - (1f - ratio) * length;
        }
    }

    private int CalculateOrder(int index, int totalCount)
    {
        if (index * 2 <= totalCount && index != 0)
        {
            return totalCount - index * 2;
        }
        else
        {
            return totalCount - ((totalCount - index) * 2 + 1);
        }
    }

    private void OnDrageItem(float dragMovement)
    {
        int symbol = (int)Mathf.Sign(dragMovement);
        int count = m_Items.Count;
        for (int i = 0; i < m_Items.Count; i++)
        {
            int id = m_Items[i].PosId + symbol;
            if (id < 0)
            {
                id += count;
            }
            m_Items[i].PosId = id % count;
        }

        SetPosData();
    }

    private static RotationDiagramItem CreateTemplate()
    {
        GameObject template = new GameObject();
        template.name = "RotationDiagramItem";
        template.AddComponent<RectTransform>();
        template.AddComponent<CanvasRenderer>();
        template.AddComponent<Image>();
        return template.AddComponent<RotationDiagramItem>();
    }

    private static void CreateInstances(Transform parent, int count, List<RotationDiagramItem> list)
    {
        RotationDiagramItem template = CreateTemplate();
        for (int i = 0; i < count; i++)
        {
            RotationDiagramItem item = Instantiate<RotationDiagramItem>(template, parent);
            list.Add(item);
        }
        GameObject.Destroy(template);
    }

    [Serializable]
    public class ItemPosData
    {
        public int Id;
        public float PosX;
        public float Scale;
        public int Order;
    }
}