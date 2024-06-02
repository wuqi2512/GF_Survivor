using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GlobelRandom = GameFramework.Utility.Random;

public class LevelController
{
    public HeroLogic Player { get; private set; }
    public int KilledEnemy { get; private set; }

    private InGameForm m_View;
    private EntityLoader m_EntityLoader;
    private Transform m_PlayerTransform;
    private CinemachineVirtualCamera m_VirtualCamera;
    private Dictionary<int, Targetable> m_DicEntityEnemy;
    private Vector2 m_HalfScreenSizeInWorld;

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

        m_EntityLoader.ShowEntityDefault(typeof(HeroLogic), HeroData.Create(10000, GameEntry.Entity.GenerateSerialId()), (entity) =>
        {
            Player = entity.Logic as HeroLogic;
            m_PlayerTransform = Player.transform;
            m_VirtualCamera.Follow = m_PlayerTransform;
            GameEntry.DataNode.SetData<VarTransform>("Player", m_PlayerTransform);
        });
    }

    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
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
        GameEntry.DataNode.RemoveNode("Player");
    }

    public void SpawnEnemy()
    {
        EnemyData enemyData = EnemyData.Create(10001, GameEntry.Entity.GenerateSerialId(), m_PlayerTransform.position + RandomOffset(5f, 5f));
        m_EntityLoader.ShowEntityDefault(typeof(EnemyLogic), enemyData, (entity) =>
        {
            m_DicEntityEnemy.Add(entity.Id, (Targetable)entity.Logic);
        });
    }

    public void HideEnemy(int serialId, bool isDead)
    {
        if (!m_DicEntityEnemy.ContainsKey(serialId))
        {
            Log.Error("Can't find enemy '{0}'", serialId);
        }

        m_EntityLoader.HideEntity(serialId);
        m_DicEntityEnemy.Remove(serialId);

        if (isDead)
        {
            KilledEnemy++;
            m_View.SetKillCount(KilledEnemy);
        }
    }

    public void OnPlayerHpChanged(int lastHp, int currentHp)
    {
        m_View.SetHpBar((float)currentHp / lastHp);
    }

    public void HandleDamageInfo(DamageInfo damageInfo)
    {
        Targetable defender = m_EntityLoader.GetEntity(damageInfo.Defender).Logic as Targetable;
        if (defender == null || defender.IsDead)
        {
            return;
        }

        int damage = damageInfo.Damage;
        bool isDead  = false;
        bool isCritical = GlobelRandom.GetRandomFloat(0f, 1f) < damageInfo.CriticalRate;
        if (isCritical)
        {
            damage = (int)(damage * damageInfo.CriticalMulti);
        }
        if (damage > defender.Hp)
        {
            damage = defender.Hp;
            isDead = true;
        }

        defender.TakeDamage(damage);
        if (isDead)
        {
            GameEntry.Entity.ShowEffect(EffectData.Create(10001, GameEntry.Entity.GenerateSerialId(), defender.CachedTransform));
        }
        else
        {
            GameEntry.Entity.ShowEffect(EffectData.Create(10002, GameEntry.Entity.GenerateSerialId(), defender.CachedTransform));
        }
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