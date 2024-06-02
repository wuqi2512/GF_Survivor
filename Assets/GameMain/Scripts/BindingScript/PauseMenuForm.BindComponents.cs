using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/5/23 20:36:16
	public partial class PauseMenuForm
	{

		private CommonButton m_Btn_Resume;
		private CommonButton m_Btn_Restart;
		private CommonButton m_Btn_MainMenu;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Resume = autoBindTool.GetBindComponent<CommonButton>(0);
			m_Btn_Restart = autoBindTool.GetBindComponent<CommonButton>(1);
			m_Btn_MainMenu = autoBindTool.GetBindComponent<CommonButton>(2);
		}
	}
