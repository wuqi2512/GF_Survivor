using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class HeroLogic : Targetable
{
    private int BulletId = 10001;
    private float MoveSpeed = 3f;
    private float m_ShootInterval = 0.5f;

    private float m_ShootTimer;
    private HeroData m_HeroData;

    public override int MaxHp => m_HeroData.MaxHp;
    public override CampType Camp => CampType.Player;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_HeroData = userData as HeroData;
        if (m_HeroData == null)
        {
            Log.Error("HeroData is invalid.");
        }

        m_Hp = m_HeroData.MaxHp;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause)
        {
            return;
        }

        Vector2 moveInput = GetInput();
        SetVelocity(moveInput * MoveSpeed, Vector2.zero);

        m_ShootTimer += elapseSeconds;
        if (m_ShootTimer > m_ShootInterval && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = mousePos - pos;

            float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            BulletData bulletData = BulletData.Create(BulletId, GameEntry.Entity.GenerateSerialId(),
                CampType.Player, CachedTransform.position, Quaternion.Euler(0f, 0f, degree));
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(typeof(BulletLogic), bulletData));
            m_ShootTimer = 0f;
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_ShootTimer = 0f;
    }

    public override void TakeDamage(int damage)
    {
        GameEntry.Event.Fire(this, PlayerHpChangedEventArgs.Create(m_Hp, m_Hp - damage));

        base.TakeDamage(damage);
    }
    private static Vector2 GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }
}