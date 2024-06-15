using Cinemachine;
using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GlobelRandom = GameFramework.Utility.Random;

public class LevelController
{
    public HeroLogic Player { get; private set; }
    public int KilledEnemy { get; private set; }
    public bool GameOver { get; private set; }

    private InGameForm m_View;
    private EntityLoader m_EntityLoader;
    private Transform m_PlayerTransform;
    private CinemachineVirtualCamera m_VirtualCamera;
    private Dictionary<int, Targetable> m_DicEntityEnemy;
    private Vector2 m_HalfScreenSizeInWorld;
    private bool m_Pause;

    private float m_SpawnEnemyInterval = 1f;
    private float m_SpawnEnemyTimer;


    public LevelController(CinemachineVirtualCamera virtualCamera, Vector2 screenSizeInWorld)
    {
        m_DicEntityEnemy = new Dictionary<int, Targetable>();
        m_VirtualCamera = virtualCamera;
        m_HalfScreenSizeInWorld = screenSizeInWorld / 2;
    }

    public void OnEnter()
    {
        m_EntityLoader = EntityLoader.Create(this);


        ShowEntity(typeof(HeroLogic), HeroData.Create(10000, GameEntry.Entity.GenerateSerialId()), (entity) =>
        {
            Player = entity.Logic as HeroLogic;
            m_PlayerTransform = Player.transform;
            m_VirtualCamera.Follow = m_PlayerTransform;
            GameEntry.DataNode.SetData<VarTransform>("Player", m_PlayerTransform);
        });
    }

    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (m_Pause)
        {
            return;
        }

        // spawn enemy
        m_SpawnEnemyTimer += elapseSeconds;
        if (m_SpawnEnemyTimer >= m_SpawnEnemyInterval)
        {
            if (m_DicEntityEnemy.Count < 20)
            {
                SpawnEnemy();
            }
            m_SpawnEnemyTimer -= m_SpawnEnemyInterval;
        }
    }

    public void OnLeave()
    {
        m_EntityLoader.HideAllEntity();
        ReferencePool.Release(m_EntityLoader);
        m_DicEntityEnemy.Clear();
        m_Pause = false;
        KilledEnemy = 0;
        m_SpawnEnemyTimer = 0f;
        GameEntry.DataNode.RemoveNode("Player");
        GameOver = false;
    }

    public void OnPause()
    {
        m_Pause = true;

        foreach (Entity entity in m_EntityLoader.GetAllEntities())
        {
            IPause obj = entity.Logic as IPause;
            if (obj != null)
            {
                obj.Pause();
            }
        }
    }

    public void OnResume()
    {
        m_Pause = false;

        foreach (Entity entity in m_EntityLoader.GetAllEntities())
        {
            IPause obj = entity.Logic as IPause;
            if (obj != null)
            {
                obj.Resume();
            }
        }
    }

    public void SpawnEnemy()
    {
        EnemyData enemyData = EnemyData.Create(10001, GameEntry.Entity.GenerateSerialId(), m_PlayerTransform.position + RandomOffset(5f, 5f));
        ShowEntity(typeof(EnemyLogic), enemyData, (entity) =>
        {
            m_DicEntityEnemy.Add(entity.Id, (Targetable)entity.Logic);
        });
    }

    public void HideEnemy(int serialId, bool isDead)
    {
        if (!m_DicEntityEnemy.ContainsKey(serialId))
        {
            Log.Error("Can't find enemy '{0}'", serialId);
            return;
        }

        m_EntityLoader.HideEntity(serialId);
        m_DicEntityEnemy.Remove(serialId);

        if (isDead)
        {
            KilledEnemy++;
            m_View.SetKillCount(KilledEnemy);
        }
    }

    public void OnPlayerHpChanged()
    {
        float ratio = Player.Hp / Player.MaxHp;
        m_View.SetHpBar(ratio);

        if (ratio == 0f)
        {
            GameEntry.Event.Fire(this, LevelOperationEventArgs.Create(LevelOperation.GameOver));
        }
    }

    public void HandleDamageInfo(DamageInfo damageInfo)
    {
        if (damageInfo == null)
        {
            Log.Error("DamageInfo is invalid.");
            return;
        }

        Entity entity = m_EntityLoader.GetEntity(damageInfo.Defender);
        if (entity == null)
        {
            return;
        }

        Targetable defender = entity.Logic as Targetable;
        if (defender == null || defender.IsDead)
        {
            return;
        }

        float damage = damageInfo.Damage;
        bool isCritical = GlobelRandom.GetRandomFloat(0f, 1f) < damageInfo.CriticalRate;
        if (isCritical)
        {
            damage = damage * damageInfo.CriticalMulti;
        }
        if (damage >= defender.Hp)
        {
            damage = defender.Hp;
        }

        defender.TakeDamage(damage);
    }

    public void ShowEntity(Type logicType, EntityData entityData, Action<Entity> onSuccess)
    {
        m_EntityLoader.ShowEntity(logicType, entityData, (entity) =>
        {
            if (m_Pause)
            {
                IPause obj = entity.Logic as IPause;
                if (obj != null)
                {
                    obj.Pause();
                }
            }

            onSuccess?.Invoke(entity);
        });
    }

    public void HideEntity(int serialId)
    {
        m_EntityLoader.HideEntity(serialId);
    }

    private Vector3 RandomOffset(float maxX, float maxY)
    {
        float x = GlobelRandom.GetRandomFloat(0f, maxX);
        float y = GlobelRandom.GetRandomFloat(0f, maxY);
        Vector2 pos = new Vector2(x, y) + m_HalfScreenSizeInWorld + new Vector2(2.5f, 5f);
        if (GlobelRandom.GetRandomBool())
        {
            pos.x *= -1;
        }
        if (GlobelRandom.GetRandomBool())
        {
            pos.y *= -1;
        }

        return pos;
    }

    public void SetView(InGameForm form)
    {
        m_View = form;
    }
}