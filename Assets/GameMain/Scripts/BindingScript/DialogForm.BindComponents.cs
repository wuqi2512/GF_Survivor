using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/5/24 9:07:41
	public partial class DialogForm
	{

		private TextMeshProUGUI m_TxtP_Title;
		private TextMeshProUGUI m_TxtP_Message;
		private CommonButton m_Btn_Confirm1;
		private CommonButton m_Btn_Confirm2;
		private CommonButton m_Btn_Cancel2;
		private CommonButton m_Btn_Confirm3;
		private CommonButton m_Btn_Cancel3;
		private CommonButton m_Btn_Other3;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_TxtP_Title = autoBindTool.GetBindComponent<TextMeshProUGUI>(0);
			m_TxtP_Message = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_Btn_Confirm1 = autoBindTool.GetBindComponent<CommonButton>(2);
			m_Btn_Confirm2 = autoBindTool.GetBindComponent<CommonButton>(3);
			m_Btn_Cancel2 = autoBindTool.GetBindComponent<CommonButton>(4);
			m_Btn_Confirm3 = autoBindTool.GetBindComponent<CommonButton>(5);
			m_Btn_Cancel3 = autoBindTool.GetBindComponent<CommonButton>(6);
			m_Btn_Other3 = autoBindTool.GetBindComponent<CommonButton>(7);
		}
	}
