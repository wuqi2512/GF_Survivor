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
    private RedDotComponent.Node m_EquipmentNode;
    private RedDotComponent.Node m_AchievementNode;
    private bool m_IsFocused;

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
        m_Btn_Equipment.OnClick += OnBtnEquipmentClick;
        m_Btn_Achievement.OnClick += OnBtnAchievementClick;

        m_EquipmentNode = GameEntry.RedDot.GetOrAddNode(null, RedDotConfig.EquipmentForm);
        m_AchievementNode = GameEntry.RedDot.GetOrAddNode(null, RedDotConfig.AchievementForm);
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

        m_RDot_EquipmentForm.Set(m_EquipmentNode.Value);
        m_RDot_Achievement.Set(m_AchievementNode.Value);

        m_EquipmentNode.OnValueChanged += m_RDot_EquipmentForm.Set;
        m_AchievementNode.OnValueChanged += m_RDot_Achievement.Set;
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        m_EquipmentNode.OnValueChanged -= m_RDot_EquipmentForm.Set;
        m_AchievementNode.OnValueChanged -= m_RDot_Achievement.Set;

        m_ProcedureMenu = null;

        base.OnClose(isShutdown, userData);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_IsFocused && !GameEntry.UI.HasUIForm(UIFormId.DialogForm) && Input.GetKeyDown(KeyCode.Escape))
        {
            OnBtnQuitClick();
        }
    }

    protected override void OnCover()
    {
        base.OnCover();

        m_IsFocused = false;
    }

    protected override void OnReveal()
    {
        base.OnReveal();

        m_IsFocused = true;

        m_TxtP_Coin.text = GameEntry.Player.Coin.ToString();
        m_TxtP_Diamond.text = GameEntry.Player.Diamond.ToString();

        m_RDot_EquipmentForm.Set(m_EquipmentNode.Value);
    }

    private void OnBtnQuitClick()
    {
        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Message = GameEntry.Localization.GetString("MainMenuForm.AskQuit"),
            OnClickConfirm = delegate (object userData)
            {
                GameEntry.Player.Save();
                GameEntry.Achievement.Save();
                UnityGameFramework.Runtime.GameEntry.Shutdown(UnityGameFramework.Runtime.ShutdownType.Quit);
            },
        });
    }

    private void OnBtnEquipmentClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.EquipmentForm);
    }

    private void OnBtnAchievementClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.AchievementForm);
    }
}