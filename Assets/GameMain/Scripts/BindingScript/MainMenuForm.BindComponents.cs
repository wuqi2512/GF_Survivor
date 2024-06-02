using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/5/23 21:02:16
	public partial class MainMenuForm
	{

		private CommonButton m_Btn_Start;
		private CommonButton m_Btn_Setting;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Start = autoBindTool.GetBindComponent<CommonButton>(0);
			m_Btn_Setting = autoBindTool.GetBindComponent<CommonButton>(1);
		}
	}
