using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/7/24 22:02:36
	public partial class EquipPopupForm
	{

		private CommonButton m_Btn_Close;
		private TextMeshProUGUI m_TxtP_Quality;
		private TextMeshProUGUI m_TxtP_Level;
		private TextMeshProUGUI m_TxtP_ItemName;
		private TextMeshProUGUI m_TxtP_CostCoin;
		private CommonButton m_Btn_Unequip;
		private CommonButton m_Btn_Equip;
		private CommonButton m_Btn_Upgrade;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Close = autoBindTool.GetBindComponent<CommonButton>(0);
			m_TxtP_Quality = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_TxtP_Level = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
			m_TxtP_ItemName = autoBindTool.GetBindComponent<TextMeshProUGUI>(3);
			m_TxtP_CostCoin = autoBindTool.GetBindComponent<TextMeshProUGUI>(4);
			m_Btn_Unequip = autoBindTool.GetBindComponent<CommonButton>(5);
			m_Btn_Equip = autoBindTool.GetBindComponent<CommonButton>(6);
			m_Btn_Upgrade = autoBindTool.GetBindComponent<CommonButton>(7);
		}
	}
