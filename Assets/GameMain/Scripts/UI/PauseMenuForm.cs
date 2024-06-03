using StarForce;
using UnityGameFramework.Runtime;

public partial class PauseMenuForm : UGuiForm
{
    private ProcedureMain m_ProcedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GetBindComponents(this.gameObject);

        m_Btn_Resume.OnClick += OnBtnResumeClick;
        m_Btn_Restart.OnClick += OnBtnRestartClick;
        m_Btn_MainMenu.OnClick += OnBtnMainMenuClick;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        m_ProcedureMain = userData as ProcedureMain;
        if (m_ProcedureMain == null)
        {
            Log.Warning("ProcedureMain is invalid when open PauseMenuForm.");
            return;
        }

        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.Pasue));
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);

        m_ProcedureMain = null;
    }

    private void OnBtnResumeClick()
    {
        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.Resume));
        Close();
    }

    private void OnBtnRestartClick()
    {
        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.Restart));
        Close();
    }

    private void OnBtnMainMenuClick()
    {
        GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.MainMenu));
        Close();
    }
}