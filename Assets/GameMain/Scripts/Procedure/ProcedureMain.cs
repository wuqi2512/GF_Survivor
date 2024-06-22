//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Cinemachine;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

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

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OpenUIFormFailure);
            GameEntry.Event.Subscribe(DamageEventArgs.EventId, HandleDamageInfo);
            GameEntry.Event.Subscribe(BeDamagedEventArgs.EventId, BeDamaged);
            GameEntry.Event.Subscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Subscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);
            GameEntry.Event.Subscribe(LevelOperationEventArgs.EventId, OnLevelOperation);

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
            GameEntry.Event.Unsubscribe(BeDamagedEventArgs.EventId, BeDamaged);
            GameEntry.Event.Unsubscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Unsubscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);
            GameEntry.Event.Unsubscribe(LevelOperationEventArgs.EventId, OnLevelOperation);

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

        #region Level Operation
        private void GotoMenu()
        {
            m_GotoMenu = true;
        }

        private void Restart()
        {
            LevelController.OnLeave();
            LevelController.OnEnter();
        }

        private void Pause()
        {
            LevelController.OnPause();
        }

        private void Resume()
        {
            LevelController.OnResume();
        }

        public void GameOver()
        {
            DialogParams dialogParams = new DialogParams();
            dialogParams.Mode = 1;
            dialogParams.Title = "GameOver";
            dialogParams.Message = Utility.Text.Format("Kill: {0}", LevelController.KilledEnemy.ToString());
            dialogParams.ConfirmText = "MainMenu";
            dialogParams.OnClickConfirm += (obj) => { GotoMenu(); };
            GameEntry.UI.OpenDialog(dialogParams);
        }

        #endregion

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
            if (ne.SerialId == LevelController.HeroLogic.Id)
            {
                LevelController.OnPlayerHpChanged();
            }
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

            if (ne.IsEnemy)
            {
                LevelController.HideEnemy(ne.SerialId, ne.IsEnemyDead);
            }
            else
            {
                LevelController.HideEntity(ne.SerialId);
            }
        }

        private void OnLevelOperation(object sender, BaseEventArgs e)
        {
            var ne = e as LevelOperationEventArgs;
            if (ne == null)
            {
                return;
            }

            switch (ne.LevelOperation)
            {
                case LevelOperation.Pasue: Pause(); break;
                case LevelOperation.Resume: Resume(); break;
                case LevelOperation.Restart: Restart(); break;
                case LevelOperation.GameOver: GameOver(); break;
                case LevelOperation.MainMenu: GotoMenu(); break;
            }
        }
    }
}
