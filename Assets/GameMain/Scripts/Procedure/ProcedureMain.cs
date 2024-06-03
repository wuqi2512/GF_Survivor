//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Cinemachine;

namespace StarForce
{
    public class ProcedureMain : ProcedureBase
    {
        private bool m_GotoMenu = false;
        public LevelController LevelController { get; private set; }
        private CinemachineVirtualCamera m_VirtualCamera;
        private Vector2 m_ScreenSizeInWorld;
        private int m_InGameFormSerialId;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        public void Restart()
        {
            LevelController.OnLeave();
            LevelController.OnEnter();
        }

        public void Pause()
        {
            LevelController.OnPause();
        }

        public void Resume()
        {
            LevelController.OnResume();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OpenUIFormFailure);
            GameEntry.Event.Subscribe(DamageEventArgs.EventId, HandleDamageInfo);
            GameEntry.Event.Subscribe(HideEnemyEventArgs.EventId, HideEnemyInLevel);
            GameEntry.Event.Subscribe(BeDamagedEventArgs.EventId, BeDamaged);
            GameEntry.Event.Subscribe(LevelStateChangeEventArgs.EventId, OnLevelStateChange);
            GameEntry.Event.Subscribe(PlayerHpChangedEventArgs.EventId, OnPlayerHpChanged);
            GameEntry.Event.Subscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Subscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);

            m_VirtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            m_ScreenSizeInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));

            GameEntry.UI.OpenUIForm(UIFormId.BlackEdgeForm);
            m_InGameFormSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.InGameForm, this);
            LevelController = new LevelController(m_VirtualCamera, m_ScreenSizeInWorld);
            LevelController.OnEnter();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            LevelController.OnLeave();

            GameEntry.UI.CloseAllLoadedUIForms();

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OpenUIFormFailure);
            GameEntry.Event.Unsubscribe(DamageEventArgs.EventId, HandleDamageInfo);
            GameEntry.Event.Unsubscribe(HideEnemyEventArgs.EventId, HideEnemyInLevel);
            GameEntry.Event.Unsubscribe(BeDamagedEventArgs.EventId, BeDamaged);
            GameEntry.Event.Unsubscribe(LevelStateChangeEventArgs.EventId, OnLevelStateChange);
            GameEntry.Event.Unsubscribe(PlayerHpChangedEventArgs.EventId, OnPlayerHpChanged);
            GameEntry.Event.Unsubscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Unsubscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            LevelController.OnUpdate(elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEntry.UI.OpenUIForm(UIFormId.PauseMenuForm, this);
            }

            if (m_GotoMenu)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OpenUIFormSuccess(object sender, BaseEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            if (ne.UIForm.SerialId == m_InGameFormSerialId)
            {
                LevelController.SetView(ne.UIForm.Logic as InGameForm);
            }
        }

        private void OpenUIFormFailure(object sender, BaseEventArgs e)
        {
            OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error(ne.ErrorMessage);
        }

        private void HideEnemyInLevel(object sender, BaseEventArgs e)
        {
            HideEnemyEventArgs ne = (HideEnemyEventArgs)e;
            if (ne == null)
            {
                return;
            }

            LevelController.HideEnemy(ne.SerialId, ne.IsDead);
        }

        private void HandleDamageInfo(object sender, BaseEventArgs e)
        {
            DamageEventArgs ne = e as DamageEventArgs;
            if (ne == null)
            {
                return;
            }

            LevelController.HandleDamageInfo(ne.DamageInfo);
        }

        private void BeDamaged(object sender, BaseEventArgs e)
        {
            var ne = e as BeDamagedEventArgs;
            if (ne == null)
            {
                return;
            }

            GameEntry.DamageNumber.ShowDamageNumber(ne.Position, ne.Damage.ToString());
        }

        private void OnLevelStateChange(object sender, BaseEventArgs e)
        {
            var ne = e as LevelStateChangeEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.LastState == LevelState.Pause)
            {

            }
            else if (ne.CurrentState == LevelState.Pause)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PauseMenuForm, this);
            }
        }

        private void OnPlayerHpChanged(object sender, BaseEventArgs e)
        {
            var ne = e as PlayerHpChangedEventArgs;
            if (ne == null)
            {
                return;
            }

            LevelController.OnPlayerHpChanged(ne.LastHp, ne.CurrentHp);
        }

        private void OnShowEntityInLevel(object sender, BaseEventArgs e)
        {
            var ne = e as ShowEntityInLevelEventArgs;
            if (ne == null)
            {
                return;
            }

            LevelController.ShowEntity(ne.LogicType, ne.EntityData, ne.ShowSuccess);
        }

        private void OnHideEntityInLevel(object sender, BaseEventArgs e)
        {
            var ne = e as HideEntityInLevelEventArgs;
            if (ne == null)
            {
                return;
            }

            LevelController.HideEntity(ne.SerialId);
        }
    }
}
