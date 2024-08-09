using GameFramework.Event;
using StarForce;
using System.Collections.Generic;
using UnityEngine;

public partial class AchievementForm : UGuiForm
{
    public GameObject ItemPrefab;

    private List<AchievementItem> m_Items;
    private AchievementComponent m_Achieve;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_Achieve = GameEntry.Achievement;
        m_Items = new List<AchievementItem>();

        m_Btn_Close.OnClick += Close;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        RefreshAllItem();
        GameEntry.Event.Subscribe(AchievementUpdateEventArgs.EventId, OnAchievementUpdate);
        foreach (var item in m_Items)
        {
            var node = GameEntry.RedDot.GetNode(null, RedDotConfig.AchievementForm, item.AchievementData.AchievementId.ToString());
            if (node != null)
            {
                item.RedDot.Set(node.Value);
                node.OnValueChanged += item.RedDot.Set;
            }
        }
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        GameEntry.Event.Unsubscribe(AchievementUpdateEventArgs.EventId, OnAchievementUpdate);
        foreach (var item in m_Items)
        {
            var node = GameEntry.RedDot.GetNode(null, RedDotConfig.AchievementForm, item.AchievementData.AchievementId.ToString());
            if (node != null)
            {
                node.OnValueChanged -= item.RedDot.Set;
            }
        }
    }

    private void RefreshAllItem()
    {
        var datas = m_Achieve.GetAllAchievementDatas();
        datas.Sort((a, b) =>
        {
            if (a.HasReceiveAward)
            {
                return 1;
            }
            else if (b.HasReceiveAward)
            {
                return -1;
            }
            else if (a.IsCompleted)
            {
                return -1;
            }
            else if (b.IsCompleted)
            {
                return 1;
            }

            return 0;
        });

        while (datas.Count > m_Items.Count)
        {
            var item = CreateAchievementItem();
            item.OnClick += OnClickItem;
            m_Items.Add(item);
        }

        while (datas.Count < m_Items.Count)
        {
            m_Items.RemoveAt(m_Items.Count - 1);
        }

        for (int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i].SetAchievementData(datas[i]);
        }
    }

    private void OnAchievementUpdate(object sender, GameEventArgs e)
    {
        var ne = e as AchievementUpdateEventArgs;

        foreach (var item in m_Items)
        {
            if (item.AchievementData == ne.AchievementData)
            {
                item.SetAchievementData(ne.AchievementData);
            }
        }
    }

    public AchievementItem CreateAchievementItem()
    {
        GameObject obj = Instantiate(ItemPrefab, m_Trans_Content);
        AchievementItem item = obj.GetComponent<AchievementItem>();
        return item;
    }

    private void OnClickItem(AchievementData achievementData)
    {
        m_Achieve.ReceiveAward(achievementData);
    }
}