using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/6/21 15:22:54
	public partial class EquipmentForm
	{

		private RectTransform m_Trans_SlotContent;
		private CommonButton m_Btn_Close;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Trans_SlotContent = autoBindTool.GetBindComponent<RectTransform>(0);
			m_Btn_Close = autoBindTool.GetBindComponent<CommonButton>(1);
		}
	}
