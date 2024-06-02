using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/5/23 20:56:16
	public partial class TileMapForm
	{

		private TextMeshProUGUI m_TxtP_Score;
		private CommonButton m_Btn_Pause;
		private RectTransform m_Rect_Panel;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_TxtP_Score = autoBindTool.GetBindComponent<TextMeshProUGUI>(0);
			m_Btn_Pause = autoBindTool.GetBindComponent<CommonButton>(1);
			m_Rect_Panel = autoBindTool.GetBindComponent<RectTransform>(2);
		}
	}
