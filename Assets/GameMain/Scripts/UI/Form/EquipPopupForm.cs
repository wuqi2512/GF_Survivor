using cfg;
using GameFramework;
using StarForce;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class EquipPopupForm : UGuiForm
{
    public AttributeItem[] Attributes;

    private EquipmentPopupParams m_Params;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_Btn_Close.OnClick += OnClickCloseBtn;
        m_Btn_Equip.OnClick += OnClickEquipBtn;
        m_Btn_Unequip.OnClick += OnClickUnequipBtn;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_Params = userData as EquipmentPopupParams;
        if (m_Params == null)
        {
            Log.Error("EquipmentPopupParams is invalid.");
            return;
        }

        if (m_Params.Mode == 0)
        {
            m_Btn_Equip.gameObject.SetActive(true);
            m_Btn_Unequip.gameObject.SetActive(false);
        }
        else if (m_Params.Mode == 1)
        {
            m_Btn_Equip.gameObject.SetActive(false);
            m_Btn_Unequip.gameObject.SetActive(true);
        }

        m_TxtP_ItemName.text = m_Params.Equipment.Name;
        SetAttributeItems();
        GameEntry.Resource.LoadAsset(AssetUtility.GetEquipmentSpriteAsset(m_Params.Equipment.AssetName), (asset) =>
        {
            Texture2D texture = (Texture2D)asset;
            m_Img_ItemSprite.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        });
    }

    private void SetAttributeItems()
    {
        ModifierData[] modifiers = m_Params.Equipment.Modifiers;
        for (int i = 0; i < Attributes.Length; i++)
        {
            AttributeItem attributeItem = Attributes[i];
            if (i < modifiers.Length)
            {
                ModifierData modifierData = modifiers[i];
                attributeItem.NameText.text = modifierData.NumericType.ToString();
                string valueText = string.Empty;
                switch (modifierData.ModifierType)
                {
                    case ModifierType.Add:
                        valueText = modifierData.Value.ToString();
                        break;
                    case ModifierType.Pct:
                        valueText = Utility.Text.Format("{0}%", modifierData.Value.ToString());
                        break;
                }
                attributeItem.ValueText.text = valueText;
                attributeItem.gameObject.SetActive(true);
            }
            else
            {
                attributeItem.gameObject.SetActive(false);
            }
        }
    }

    private void OnClickCloseBtn()
    {
        Close();
    }

    private void OnClickEquipBtn()
    {
        m_Params?.OnClickEquip();
        Close();
    }

    private void OnClickUnequipBtn()
    {
        m_Params?.OnClickUnequip();
        Close();
    }

    public class EquipmentPopupParams
    {
        // 0: EquipBtn
        // 1: UnequipBtn
        public int Mode;
        public Equipment Equipment;
        public Action OnClickEquip;
        public Action OnClickUnequip;

        public EquipmentPopupParams(int mode, Equipment equipment, Action onClickEquip, Action onClickUnequip)
        {
            Mode = mode;
            Equipment = equipment;
            OnClickEquip = onClickEquip;
            OnClickUnequip = onClickUnequip;
        }
    }
}