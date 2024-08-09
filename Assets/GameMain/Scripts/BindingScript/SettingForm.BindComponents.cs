using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：2024/8/9 18:59:36
	public partial class SettingForm
	{

		private Slider m_Slider_Music;
		private Slider m_Slider_Sound;
		private Slider m_Slider_UISound;
		private TMP_Dropdown m_Drop_Language;
		private CommonButton m_Btn_Quit;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Slider_Music = autoBindTool.GetBindComponent<Slider>(0);
			m_Slider_Sound = autoBindTool.GetBindComponent<Slider>(1);
			m_Slider_UISound = autoBindTool.GetBindComponent<Slider>(2);
			m_Drop_Language = autoBindTool.GetBindComponent<TMP_Dropdown>(3);
			m_Btn_Quit = autoBindTool.GetBindComponent<CommonButton>(4);
		}
	}
