using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/5/24 9:34:54
	public partial class SettingForm
	{

		private CommonButton m_Btn_Confirm;
		private CommonButton m_Btn_Cancel;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Confirm = autoBindTool.GetBindComponent<CommonButton>(0);
			m_Btn_Cancel = autoBindTool.GetBindComponent<CommonButton>(1);
		}
	}
