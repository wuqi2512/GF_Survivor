using cfg;
using GameFramework;
using StarForce;
using UnityGameFramework.Runtime;

public partial class EquipPopupForm : UGuiForm
{
    public AttributeItem[] Attributes;
    public SlotItem SlotItem;

    private EquipmentData m_EquipmentData;
    private Equipment m_Equipment;
    private Player m_Player;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_Btn_Close.OnClick += OnBtnCloseClick;
        m_Btn_Equip.OnClick += OnBtnEquipClick;
        m_Btn_Unequip.OnClick += OnBtnUnequipClick;
        m_Btn_Upgrade.OnClick += OnBtnUpgradeClick;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_EquipmentData = userData as EquipmentData;
        if (m_EquipmentData == null)
        {
            Log.Error("EquipmentData is invalid.");
            return;
        }
        m_Equipment = m_EquipmentData.Equipment;
        m_Player = GameEntry.Player;

        if (!m_Player.IsEquipped(m_EquipmentData))
        {
            m_Btn_Equip.gameObject.SetActive(true);
            m_Btn_Unequip.gameObject.SetActive(false);
        }
        else
        {
            m_Btn_Equip.gameObject.SetActive(false);
            m_Btn_Unequip.gameObject.SetActive(true);
        }

        m_TxtP_ItemName.text = GameEntry.Localization.GetString(m_Equipment.Name);
        m_TxtP_Level.text = Utility.Text.Format("Lv.{0}", m_EquipmentData.Level.ToString());
        m_TxtP_Quality.text = m_Equipment.Quality.ToString();
        SetAttributeItems();
        SetCostAndButtons();
        SlotItem.SetEquipmentData(m_EquipmentData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        m_EquipmentData = null;
        m_Equipment = null;
        m_Player = null;
    }

    private void SetAttributeItems()
    {
        cfg.equipment.AttributeData[] modifiers = m_Equipment.BaseAttributes;
        for (int i = 0; i < Attributes.Length; i++)
        {
            AttributeItem attributeItem = Attributes[i];
            if (i < modifiers.Length)
            {
                var modifierData = modifiers[i];
                attributeItem.NameText.text = modifierData.Numeric.ToString();
                string valueText = string.Empty;
                switch (modifierData.Modifier)
                {
                    case ModifierType.Add:
                        valueText = modifierData.Values[m_EquipmentData.Level].ToString();
                        break;
                    case ModifierType.Pct:
                        valueText = Utility.Text.Format("{0}%", modifierData.Values[m_EquipmentData.Level].ToString());
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

    private void OnBtnCloseClick()
    {
        Close();
    }

    private void OnBtnEquipClick()
    {
        m_Player.Equip(m_EquipmentData);
        Close();
    }

    private void OnBtnUnequipClick()
    {
        m_Player.Unequip(m_EquipmentData.EquipmentType);
        Close();
    }

    private void OnBtnUpgradeClick()
    {
        m_Player.UpgradeEquipment(m_EquipmentData);
        m_TxtP_Level.text = Utility.Text.Format("Lv.{0}", m_EquipmentData.Level.ToString());
        SetAttributeItems();
        SetCostAndButtons();
        SlotItem.SetEquipmentData(m_EquipmentData);
    }

    private void SetCostAndButtons()
    {
        if (m_EquipmentData.Level < Constant.Game.EquipmentMaxLevel)
        {
            m_Btn_Upgrade.gameObject.SetActive(true);
            m_TxtP_CostCoin.text = Utility.Text.Format("{0}/{1}", Constant.Game.EquipmentUpgradeCostCoin[m_EquipmentData.Level], m_Player.Coin);
        }
        else
        {
            m_Btn_Upgrade.gameObject.SetActive(false);
            m_TxtP_CostCoin.text = Utility.Text.Format("0/{0}", m_Player.Coin); ;
        }
    }
}