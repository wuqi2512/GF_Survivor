using StarForce;
using UnityGameFramework.Runtime;

public partial class PauseMenuForm : UGuiForm
{
    private ProcedureMain m_ProcedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GetBindComponents(this.gameObject);

        m_Btn_Resume.OnClick += () =>
        {
            GameEntry.UI.CloseUIForm(this);
        };

        m_Btn_Restart.OnClick += () =>
        {
            m_ProcedureMain.Restart();
            GameEntry.UI.CloseUIForm(this);
        };

        m_Btn_MainMenu.OnClick += () =>
        {
            m_ProcedureMain.GotoMenu();
            GameEntry.UI.CloseUIForm(this);
        };
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_ProcedureMain = (ProcedureMain)userData;
        if (m_ProcedureMain == null)
        {
            Log.Warning("ProcedureMain is invalid when open PauseMenuForm.");
            return;
        }
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        m_ProcedureMain = null;
    }
}