using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/7/20 14:30:58
	public partial class EquipmentForm
	{

		private RectTransform m_Trans_SlotContent;
		private CommonButton m_Btn_Close;
		private CommonButton m_Btn_AttributeList;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Trans_SlotContent = autoBindTool.GetBindComponent<RectTransform>(0);
			m_Btn_Close = autoBindTool.GetBindComponent<CommonButton>(1);
			m_Btn_AttributeList = autoBindTool.GetBindComponent<CommonButton>(2);
		}
	}
