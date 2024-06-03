using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EnemyLogic : Targetable
{
    private int MeleeDamage = 5;
    private float MeleeInterval = 0.5f;
    private float MoveSpeed = 2f;
    private EnemyData m_EnemyData;

    private float m_MeleeTimer;
    private Transform m_Target;

    public override int MaxHp => m_EnemyData.MexHp;
    public override CampType Camp => CampType.Enemy;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_EnemyData = userData as EnemyData;
        if (m_EnemyData == null)
        {
            Log.Error("EnemyData is invalid.");
        }

        m_Hp = m_EnemyData.MexHp;
        m_Target = GameEntry.DataNode.GetData<VarTransform>("Player");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        m_MeleeTimer += elapseSeconds;

        Vector3 direction = (m_Target.position - CachedTransform.position).normalized;
        SetVelocity(direction * MoveSpeed, Vector2.zero);
    }

    public override void OnDead()
    {
        base.OnDead();

        GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id, true, true));
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (m_MeleeTimer < MeleeInterval)
        {
            return;
        }

        Targetable targetable = collider.GetComponentInParent<Targetable>();
        if (targetable == null)
        {
            return;
        }

        RelationType relation = AIUtility.GetRelation(targetable.Camp, this.Camp);
        if (relation != RelationType.Hostile)
        {
            return;
        }

        m_MeleeTimer = 0f;
        DamageInfo damageInfo = DamageInfo.Create(Id, targetable.Id, 0f, MeleeDamage);
        GameEntry.Event.Fire(this, DamageEventArgs.Create(damageInfo));

        Vector2 hitDir = (Vector2)(collider.transform.position - CachedTransform.position);
        float degree = Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg;
        EffectData effectData = EffectData.Create(10003, GameEntry.Entity.GenerateSerialId(), CachedTransform.position, Quaternion.Euler(0f, 0f, degree));
        GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(typeof(EffectAnimator), effectData));
    }
}