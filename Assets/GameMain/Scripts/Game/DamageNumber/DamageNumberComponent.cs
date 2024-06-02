using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class DamageNumberComponent : GameFrameworkComponent
{
    [SerializeField] private DamageNumberItem m_DamageNumberItemTemplate;
    [SerializeField] private Transform m_InstanceRoot;
    [SerializeField] private int m_InstancePoolCapacity = 16;


    private IObjectPool<DamageNumberItemObject> m_ItemObjectPool;
    private List<DamageNumberItem> m_ActiveItems;
    private Canvas m_CachedCanvas;

    private void Start()
    {
        if (m_InstanceRoot == null)
        {
            Log.Error("You must set instance root first.");
            return;
        }

        m_CachedCanvas = m_InstanceRoot.GetComponent<Canvas>();
        m_ItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<DamageNumberItemObject>("Item", m_InstancePoolCapacity);
        m_ActiveItems = new List<DamageNumberItem>();
    }

    private void Update()
    {
        for (int i = m_ActiveItems.Count - 1; i >= 0; i--)
        {
            DamageNumberItem item = m_ActiveItems[i];
            if (item.Refresh())
            {
                continue;
            }

            HideDamageNumberItem(item);
        }
    }

    public void ShowDamageNumber(Vector3 ownerPosition, string number)
    {
        var item = CreateDamageNumberItem();
        m_ActiveItems.Add(item);
        item.Init(ownerPosition, m_CachedCanvas, number);
    }

    private void HideDamageNumberItem(DamageNumberItem item)
    {
        item.Reset();
        m_ActiveItems.Remove(item);
        m_ItemObjectPool.Unspawn(item);
    }

    private DamageNumberItem CreateDamageNumberItem()
    {
        DamageNumberItem item = null;
        DamageNumberItemObject itemObj = m_ItemObjectPool.Spawn();
        if (itemObj != null)
        {
            item = (DamageNumberItem)itemObj.Target;
        }
        else
        {
            item = Instantiate(m_DamageNumberItemTemplate);
            Transform transform = item.GetComponent<Transform>();
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;
            m_ItemObjectPool.Register(DamageNumberItemObject.Create(item), true);
        }

        return item;
    }
}