using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/6/20 17:01:21
	public partial class InGameForm
	{

		private Slider m_Slider_HpBar;
		private TextMeshProUGUI m_TxtP_Hp;
		private CommonButton m_Btn_Pause;
		private TextMeshProUGUI m_TxtP_KillCount;
		private CommonButton m_Btn_Equipment;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Slider_HpBar = autoBindTool.GetBindComponent<Slider>(0);
			m_TxtP_Hp = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_Btn_Pause = autoBindTool.GetBindComponent<CommonButton>(2);
			m_TxtP_KillCount = autoBindTool.GetBindComponent<TextMeshProUGUI>(3);
			m_Btn_Equipment = autoBindTool.GetBindComponent<CommonButton>(4);
		}
	}
