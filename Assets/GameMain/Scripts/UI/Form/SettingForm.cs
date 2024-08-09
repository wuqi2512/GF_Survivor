using GameFramework;
using GameFramework.Localization;
using StarForce;
using System.Collections.Generic;
using System.Linq;
using UnityGameFramework.Runtime;

public partial class SettingForm : UGuiForm
{
    private List<Language> m_Languages;
    private Language m_SelectedLanguage = Language.Unspecified;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);

        m_Languages = new List<Language>()
        {
            Language.ChineseSimplified,
            Language.English,
        };

        m_Slider_Music.onValueChanged.AddListener(OnMusicVolumeChanged);
        m_Slider_Sound.onValueChanged.AddListener(OnSoundVolumeChanged);
        m_Slider_UISound.onValueChanged.AddListener(OnUISoundVolumeChanged);

        m_Drop_Language.AddOptions(m_Languages.Select(l => GameEntry.Localization.GetString(Utility.Text.Format("Language.{0}", l.ToString()))).ToList());
        m_Drop_Language.onValueChanged.AddListener(OnLanguageChanged);

        m_Btn_Quit.OnClick += Close;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_Slider_Music.value = GameEntry.Sound.GetVolume("Music");
        m_Slider_Sound.value = GameEntry.Sound.GetVolume("Sound");
        m_Slider_UISound.value = GameEntry.Sound.GetVolume("UISound");

        m_SelectedLanguage = GameEntry.Localization.Language;
        m_Drop_Language.value = m_Languages.IndexOf(m_SelectedLanguage);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("Music", volume);
    }

    private void OnSoundVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("Sound", volume);
    }

    private void OnUISoundVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("UISound", volume);
    }

    private void OnLanguageChanged(int index)
    {
        Language language = m_Languages[index];

        if (m_SelectedLanguage == language)
        {
            return;
        }

        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Message = GameEntry.Localization.GetString("SettingForm.NeedRestart"),
            OnClickConfirm = (obj) =>
            {

                GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
                GameEntry.Setting.Save();

                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
            },
            OnClickCancel = (obj) =>
            {
                m_Drop_Language.value = m_Languages.IndexOf(m_SelectedLanguage);
            }
        });
    }
}
