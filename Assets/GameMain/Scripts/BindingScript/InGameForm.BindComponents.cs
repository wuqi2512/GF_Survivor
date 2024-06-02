using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/6/2 12:54:15
	public partial class InGameForm
	{

		private Slider m_Slider_HpBar;
		private CommonButton m_Btn_Pause;
		private TextMeshProUGUI m_TxtP_KillCount;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Slider_HpBar = autoBindTool.GetBindComponent<Slider>(0);
			m_Btn_Pause = autoBindTool.GetBindComponent<CommonButton>(1);
			m_TxtP_KillCount = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
		}
	}
