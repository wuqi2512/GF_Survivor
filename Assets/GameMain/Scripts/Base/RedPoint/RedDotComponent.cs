using cfg;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using UnityGameFramework.Runtime;

public partial class RedDotComponent : GameFrameworkComponent
{
    public delegate int DataRefreshFunc();
    public delegate void ViewRefreshFunc(int totalValue);

    private const string RootName = "<Root>";
    private Node m_Root;
    private List<Node> m_DirtyNodes;

    protected override void Awake()
    {
        base.Awake();

        m_Root = Node.Create(RootName, null);
        m_DirtyNodes = new List<Node>();
    }

    public void InitNodes()
    {
        {
            Player player = GameEntry.Player;
            Node equipmentForm = GetOrAddNode(null, RedDotConfig.EquipmentForm);
            for (int i = 0; i < 6; i++)
            {
                EquipmentType equipmentType = (EquipmentType)i;
                EquipmentData equipmentData = player.GetEquipped(equipmentType);
                int value = 0;
                if (equipmentData != null && equipmentData.CanUpgrade())
                {
                    value = 1;
                }
                GetOrAddNode(equipmentForm, equipmentType.ToString()).Value = value;
            }
            player.OnEquip += (oldEquip, newEquip) =>
            {
                if (newEquip.CanUpgrade())
                {
                    GetOrAddNode(null, RedDotConfig.EquipmentForm, newEquip.EquipmentType.ToString()).Value = 1;
                }
            };
            player.OnUnequip += (equipmentType) =>
            {
                GetOrAddNode(null, RedDotConfig.EquipmentForm, equipmentType.ToString()).Value = 0;
            };
            player.OnUpgradeEquip += (equipmentData) =>
            {
                if (!player.IsEquipped(equipmentData))
                {
                    return;
                }

                int value = 0;
                if (equipmentData.CanUpgrade())
                {
                    value = 1;
                }
                GetOrAddNode(null, RedDotConfig.EquipmentForm, equipmentData.EquipmentType.ToString()).Value = value;
            };
        }

        {
            AchievementComponent achieve = GameEntry.Achievement;
            Node achievementForm = GetOrAddNode(null, RedDotConfig.AchievementForm);
            foreach (var achievement in achieve.GetAllAchievementDatas())
            {
                if (achievement.IsCompleted && !achievement.HasReceiveAward)
                {
                    GetOrAddNode(achievementForm, achievement.AchievementId.ToString()).Value = 1;
                }
            }
            GameEntry.Event.Subscribe(AchievementUpdateEventArgs.EventId, (sender, e) =>
            {
                var ne = e as AchievementUpdateEventArgs;
                AchievementData achievementData = ne.AchievementData;
                if (achievementData.IsCompleted && !achievementData.HasReceiveAward)
                {
                    GetOrAddNode(achievementForm, achievementData.AchievementId.ToString()).Value = 1;
                }
                else if (achievementData.HasReceiveAward)
                {
                    GetOrAddNode(achievementForm, achievementData.AchievementId.ToString()).Value = 0;
                }
            });
        }
    }

    private void Update()
    {
        if (m_DirtyNodes.Count == 0)
        {
            return;
        }

        while (m_DirtyNodes.Count > 0)
        {
            Node node = m_DirtyNodes[0];
            node.RefreashValue();
            m_DirtyNodes.RemoveAt(0);
        }
    }


    public Node GetNode(Node node, params string[] splitedPath)
    {
        Node current = node ?? m_Root;
        foreach (string str in splitedPath)
        {
            current = current.GetNode(str);
            if (current == null)
            {
                return null;
            }
        }

        return current;
    }

    public Node GetOrAddNode(Node node, params string[] splitedPath)
    {
        Node current = node ?? m_Root;
        foreach (string str in splitedPath)
        {
            current = current.GetOrAddChild(str);
        }

        return current;
    }

    public void RemoveNode(Node node, params string[] splitedPath)
    {
        Node target = GetNode(node, splitedPath);
        if (target == null)
        {
            return;
        }

        target.Parent.RemoveChild(target.Name);
    }

    public void SetNodeDirty(Node node, params string[] splitedPath)
    {
        Node target = GetNode(node, splitedPath);
        if (target == null || m_DirtyNodes.Contains(target))
        {
            return;
        }

        m_DirtyNodes.Add(target);
    }
}