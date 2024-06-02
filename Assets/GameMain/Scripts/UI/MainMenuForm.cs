//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using StarForce;
using UnityEngine;

public partial class MainMenuForm : UGuiForm
{
    private ProcedureMenu m_ProcedureMenu = null;

    public void OnQuitButtonClick()
    {
        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
            Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
            OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(UnityGameFramework.Runtime.ShutdownType.Quit); },
        });
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);
        m_Btn_Start.OnClick += () =>
        {
            m_ProcedureMenu.StartGame();
        };

        m_Btn_Setting.OnClick += () =>
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        };
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_ProcedureMenu = (ProcedureMenu)userData;
        if (m_ProcedureMenu == null)
        {
            UnityGameFramework.Runtime.Log.Warning("ProcedureMenu is invalid when open MenuForm.");
            return;
        }
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        m_ProcedureMenu = null;

        base.OnClose(isShutdown, userData);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitButtonClick();
        }
    }
}