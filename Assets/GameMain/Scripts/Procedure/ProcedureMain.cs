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
        private LevelController m_LevelController;
        private CinemachineVirtualCamera m_VirtualCamera;
        private Vector2 m_ScreenSizeInWorld;

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
            
            m_VirtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            m_ScreenSizeInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));

            m_LevelController = new LevelController(m_VirtualCamera, m_ScreenSizeInWorld);
            m_LevelController.OnEnter();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_LevelController.OnLeave();

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OpenUIFormFailure);
            GameEntry.Event.Unsubscribe(DamageEventArgs.EventId, HandleDamageInfo);
            GameEntry.Event.Unsubscribe(HideEnemyEventArgs.EventId, HideEnemyInLevel);
            GameEntry.Event.Unsubscribe(BeDamagedEventArgs.EventId, BeDamaged);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            m_LevelController.OnUpdate(elapseSeconds, realElapseSeconds);

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

            m_LevelController.HideEnemy(ne.EntityId);
        }

        private void HandleDamageInfo(object sender, BaseEventArgs e)
        {
            DamageEventArgs ne = e as DamageEventArgs;
            if (ne == null)
            {
                return;
            }

            m_LevelController.HandleDamageInfo(ne.DamageInfo);
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
    }
}
