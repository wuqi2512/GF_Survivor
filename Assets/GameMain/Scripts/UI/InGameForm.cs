using GameFramework;
using StarForce;
using UnityGameFramework.Runtime;

public partial class InGameForm : UGuiForm
{
    private ProcedureMain m_ProcedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);

        m_Btn_Pause.OnClick += OnBtnPauseClick;
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

        SetHpBar(1f);
        SetKillCount(0);
    }

    public void SetHpBar(float ratio)
    {
        m_Slider_HpBar.value = ratio;
    }

    public void SetKillCount(int count)
    {
        m_TxtP_KillCount.text = count.ToString();
    }

    private void OnBtnPauseClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.PauseMenuForm, m_ProcedureMain);
    }
}