using GameFramework;
using StarForce;
using UnityGameFramework.Runtime;

public partial class InGameForm : UGuiForm
{
    public AttributeList AttributeList;
    public HeroLogic HeroLogic;
    private ProcedureMain m_ProcedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        this.GetBindComponents(this.gameObject);

        AttributeList = GetComponentInChildren<AttributeList>();
        m_Btn_Pause.OnClick += OnBtnPauseClick;
        m_Btn_Equipment.OnClick += OnBtnEquipmentClick;
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
        AttributeList.Init();
    }

    public void SetHpBar(float ratio)
    {
        m_Slider_HpBar.value = ratio;
        if (HeroLogic != null)
        {
            m_TxtP_Hp.text = Utility.Text.Format("{0}/{1}", HeroLogic.Hp, HeroLogic.MaxHp);
            AttributeList.UpdateItem();
        }
    }

    public void SetKillCount(int count)
    {
        m_TxtP_KillCount.text = count.ToString();
    }

    private void OnBtnPauseClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.PauseMenuForm, m_ProcedureMain);
    }

    private void OnBtnEquipmentClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentForm);
    }

    public void SetHeroLogic(HeroLogic heroLogic)
    {
        HeroLogic = heroLogic;
        AttributeList.ChaAttribute = HeroLogic.GetChaAttribute();
        AttributeList.UpdateItem();
    }
}