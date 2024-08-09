using cfg;
using GameFramework;
using StarForce;
using System.Collections.Generic;
using UnityEngine;

public partial class EquipmentForm : UGuiForm
{
    public SlotItem[] EquippedItems;
    public GameObject SlotPrefab;

    private AttributeList m_AttributeList;
    private List<SlotItem> m_SlotItems;
    private Player m_Player;
    private ChaAttribute m_ChaAttribute;
    private List<Modifier> m_TempModifiers;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_AttributeList = GetComponentInChildren<AttributeList>();
        m_Player = GameEntry.Player;
        m_SlotItems = new List<SlotItem>();
        m_ChaAttribute = ReferencePool.Acquire<ChaAttribute>();
        m_TempModifiers = new List<Modifier>();

        m_Btn_Close.OnClick += Close;
        m_Btn_AttributeList.OnClick += OnClickAttributeList;
        m_AttributeList.Init(m_ChaAttribute);
        m_AttributeList.Visible = false;

        foreach (var equipItem in EquippedItems)
        {
            if (equipItem != null)
            {
                equipItem.OnClick += OnClickEquipItem;
            }
        }
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        RefreshAllSlot();
        m_Player.GetChaAttribute(m_ChaAttribute);

        foreach (var item in EquippedItems)
        {
            item.RedDotItem.Set(GameEntry.RedDot.GetOrAddNode(null, RedDotConfig.EquipmentForm, item.EquipmentType.ToString()).Value);
        }

        foreach (var item in EquippedItems)
        {
            GameEntry.RedDot.GetOrAddNode(null, RedDotConfig.EquipmentForm, item.EquipmentType.ToString()).OnValueChanged += item.RedDotItem.Set;
        }

        m_Player.OnEquip += OnEquip;
        m_Player.OnUnequip += OnUnequip;
        m_Player.OnAddEquip += OnAddEquipment;
        m_Player.OnRemoveEquip += OnRemoveEquipment;
        m_Player.OnUpgradeEquip += OnUpgradeEquipment;
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        foreach (var item in EquippedItems)
        {
            GameEntry.RedDot.GetOrAddNode(null, RedDotConfig.EquipmentForm, item.EquipmentType.ToString()).OnValueChanged -= item.RedDotItem.Set;
        }

        m_Player.OnEquip -= OnEquip;
        m_Player.OnUnequip -= OnUnequip;
        m_Player.OnAddEquip -= OnAddEquipment;
        m_Player.OnRemoveEquip -= OnRemoveEquipment;
        m_Player.OnUpgradeEquip -= OnUpgradeEquipment;
    }

    private void OnAddEquipment(EquipmentData equipmentData)
    {
        SlotItem slotItem = CreateSlotItem();
        slotItem.OnClick += OnClickSlotItem;
        m_SlotItems.Add(slotItem);
        slotItem.SetEquipmentData(equipmentData);
    }

    private void OnRemoveEquipment(EquipmentData equipmentData)
    {
        for (int i = 0; i < m_SlotItems.Count; i++)
        {
            if (m_SlotItems[i].EquipmentData == equipmentData)
            {
                Destroy(m_SlotItems[i].gameObject);
                m_SlotItems.RemoveAt(i);
                break;
            }
        }
    }

    private void OnEquip(EquipmentData oldEquip, EquipmentData newEquip)
    {
        if (oldEquip != null)
        {
            m_ChaAttribute.RemoveModifier(oldEquip.Modifiers);

            var oldSlotItem = FindSlotItem(oldEquip);
            oldSlotItem.SetEquppiedTextVisible(false);
        }

        m_ChaAttribute.AddModifier(newEquip.Modifiers);

        var slotItem = FindSlotItem(newEquip);
        slotItem.SetEquppiedTextVisible(true);

        SlotItem equippedSlotItem = FindEquippedItem(newEquip.EquipmentType);
        equippedSlotItem.SetEquipmentData(newEquip);
    }

    private void OnUnequip(EquipmentData equipmentData)
    {
        m_ChaAttribute.RemoveModifier(equipmentData.Modifiers);

        var slotItem = FindSlotItem(equipmentData);
        slotItem.SetEquppiedTextVisible(false);

        SlotItem equippedSlotItem = FindEquippedItem(equipmentData.EquipmentType);
        equippedSlotItem.Clear();
    }

    private void OnUpgradeEquipment(EquipmentData equipmentData)
    {
        equipmentData.GetModifiers(equipmentData.Level - 1, m_TempModifiers);
        m_ChaAttribute.RemoveModifier(m_TempModifiers);
        m_ChaAttribute.AddModifier(equipmentData.Modifiers);

        var slotItem = FindSlotItem(equipmentData);
        slotItem.SetLevelText(equipmentData.Level);

        var equippedSlotItem = FindEquippedItem(equipmentData.EquipmentType);
        if (equippedSlotItem.EquipmentData == equipmentData)
        {
            equippedSlotItem.SetLevelText(equipmentData.Level);
        }
    }

    public void RefreshAllSlot()
    {
        var equipments = m_Player.GetAllEquipment();

        foreach (var slot in m_SlotItems)
        {
            slot.Clear();
        }
        while (equipments.Count > m_SlotItems.Count)
        {
            var item = CreateSlotItem();
            item.OnClick += OnClickSlotItem;
            m_SlotItems.Add(item);
        }
        while (equipments.Count < m_SlotItems.Count)
        {
            var last = m_SlotItems[equipments.Count - 1];
            m_SlotItems.RemoveAt(equipments.Count - 1);
            Destroy(last.gameObject);
        }
        foreach (var slot in EquippedItems)
        {
            slot.Clear();
        }

        for (int i = 0; i < equipments.Count; i++)
        {
            EquipmentData data = equipments[i];
            SlotItem slotItem = m_SlotItems[i];
            slotItem.SetEquipmentData(data);
            if (m_Player.IsEquipped(data))
            {
                slotItem.SetEquppiedTextVisible(true);
                var equippedItem = FindEquippedItem(data.EquipmentType);
                equippedItem.SetEquipmentData(data);
            }
        }
    }

    private SlotItem CreateSlotItem()
    {
        GameObject obj = Instantiate(SlotPrefab, m_Trans_SlotContent);
        SlotItem item = obj.GetComponent<SlotItem>();
        return item;
    }

    public SlotItem FindEquippedItem(EquipmentType equipmentType)
    {
        foreach (SlotItem item in EquippedItems)
        {
            if (item.EquipmentType == equipmentType)
            {
                return item;
            }
        }

        return null;
    }

    public SlotItem FindSlotItem(EquipmentData equipmentData)
    {
        foreach (var item in m_SlotItems)
        {
            if (item.EquipmentData == equipmentData)
            {
                return item;
            }
        }

        return null;
    }

    private static void OnClickSlotItem(EquipmentData equipmentData)
    {
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentPopupForm, equipmentData);
    }

    private static void OnClickEquipItem(EquipmentData equipmentData)
    {
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentPopupForm, equipmentData);
    }

    private void OnClickAttributeList()
    {
        m_AttributeList.UpdateItem();
        m_AttributeList.Visible = !m_AttributeList.Visible;
    }
}