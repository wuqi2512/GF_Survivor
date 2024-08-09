using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/8/8 20:37:07
	public partial class MainMenuForm
	{

		private TextMeshProUGUI m_TxtP_Coin;
		private TextMeshProUGUI m_TxtP_Diamond;
		private CommonButton m_Btn_Setting;
		private CommonButton m_Btn_Equipment;
		private RedDotItem m_RDot_EquipmentForm;
		private CommonButton m_Btn_Achievement;
		private RedDotItem m_RDot_Achievement;
		private CommonButton m_Btn_Start;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_TxtP_Coin = autoBindTool.GetBindComponent<TextMeshProUGUI>(0);
			m_TxtP_Diamond = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_Btn_Setting = autoBindTool.GetBindComponent<CommonButton>(2);
			m_Btn_Equipment = autoBindTool.GetBindComponent<CommonButton>(3);
			m_RDot_EquipmentForm = autoBindTool.GetBindComponent<RedDotItem>(4);
			m_Btn_Achievement = autoBindTool.GetBindComponent<CommonButton>(5);
			m_RDot_Achievement = autoBindTool.GetBindComponent<RedDotItem>(6);
			m_Btn_Start = autoBindTool.GetBindComponent<CommonButton>(7);
		}
	}
