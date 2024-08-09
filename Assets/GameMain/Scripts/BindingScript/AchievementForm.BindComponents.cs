using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/8/8 21:28:11
	public partial class AchievementForm
	{

		private CommonButton m_Btn_Close;
		private RectTransform m_Trans_Content;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Close = autoBindTool.GetBindComponent<CommonButton>(0);
			m_Trans_Content = autoBindTool.GetBindComponent<RectTransform>(1);
		}
	}
