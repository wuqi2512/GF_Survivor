using cfg;
using StarForce;
using System.Collections.Generic;
using UnityEngine;

public partial class EquipmentForm : UGuiForm
{
    public EquipItem[] EquipItems;
    public GameObject SlotPrefab;

    private List<SlotItem> m_Slots;
    private Player m_Player;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_Btn_Close.OnClick += Close;
        m_Slots = new List<SlotItem>();

        foreach (var equipItem in EquipItems)
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

        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.Pasue));
        m_Player = GameEntry.Controller.Player;
        m_Player.SetEquipmentForm(this);
        RefreshAllSlot(m_Player.GetAllEquipment());
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        m_Player.SetEquipmentForm(null);
        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.Resume));
    }

    public void RefreshAllSlot(List<int> equipmentIds)
    {
        DestroyAllSlot();

        foreach (int id in equipmentIds)
        {
            AddEquipment(id);
        }
    }

    public void DestroyAllSlot()
    {
        for (int i = m_Slots.Count - 1; i >= 0; i--)
        {
            Destroy(m_Slots[i].gameObject);
            m_Slots.RemoveAt(i);
        }
    }

    public void AddEquipment(int equipmentId)
    {
        Equipment equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);
        GameObject obj = Instantiate(SlotPrefab, m_Trans_SlotContent);
        SlotItem item = obj.GetComponent<SlotItem>();
        item.EquipmentId = equipmentId;
        item.OnClick += OnClickSlotItem;
        GameEntry.Resource.LoadAsset(AssetUtility.GetEquipmentSpriteAsset(equipment.AssetName), (asset) =>
        {
            Texture2D texture = (Texture2D)asset;
            item.Image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        });
        m_Slots.Add(item);
    }

    public void RemoveEquipment(int equipmentId)
    {
        for (int i = 0; i < m_Slots.Count; i++)
        {
            if (m_Slots[i].EquipmentId == equipmentId)
            {
                SlotItem item = m_Slots[i];
                m_Slots.RemoveAt(i);
                GameObject.Destroy(item.gameObject);
                return;
            }
        }
    }

    public void Equip(int equipmentId)
    {
        Equipment equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);
        EquipItem equipItem = null;
        foreach (EquipItem item in EquipItems)
        {
            if (item.EquipmentType == equipment.EquipmentType)
            {
                equipItem = item;
                break;
            }
        }

        equipItem.EquipmentId = equipmentId;
        GameEntry.Resource.LoadAsset(AssetUtility.GetEquipmentSpriteAsset(equipment.AssetName), (asset) =>
        {
            Texture2D texture = (Texture2D)asset;
            equipItem.Image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        });
    }

    public void Unequip(EquipmentType equipmentType)
    {
        EquipItem equipItem = null;
        foreach (EquipItem item in EquipItems)
        {
            if (item.EquipmentType == equipmentType)
            {
                equipItem = item;
                break;
            }
        }

        equipItem.Image.sprite = null;
        equipItem.EquipmentId = -1;
    }

    private void OnClickSlotItem(int equipmentId)
    {
        Equipment equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentPopupForm, new EquipPopupForm.EquipmentPopupParams(0, equipment, () => { m_Player.Equip(equipmentId); }, null));
    }

    private void OnClickEquipItem(int equipmentId)
    {
        Equipment equipment = GameEntry.Luban.Tables.TbEquipment.GetOrDefault(equipmentId);
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentPopupForm, new EquipPopupForm.EquipmentPopupParams(1, equipment, null, () => { m_Player.Unequip(equipment.EquipmentType); }));
    }
}