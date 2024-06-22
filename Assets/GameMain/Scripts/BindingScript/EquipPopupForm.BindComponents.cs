using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/6/21 15:11:17
	public partial class EquipPopupForm
	{

		private CommonButton m_Btn_Close;
		private Image m_Img_ItemSprite;
		private TextMeshProUGUI m_TxtP_ItemName;
		private CommonButton m_Btn_Equip;
		private CommonButton m_Btn_Unequip;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Btn_Close = autoBindTool.GetBindComponent<CommonButton>(0);
			m_Img_ItemSprite = autoBindTool.GetBindComponent<Image>(1);
			m_TxtP_ItemName = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
			m_Btn_Equip = autoBindTool.GetBindComponent<CommonButton>(3);
			m_Btn_Unequip = autoBindTool.GetBindComponent<CommonButton>(4);
		}
	}
