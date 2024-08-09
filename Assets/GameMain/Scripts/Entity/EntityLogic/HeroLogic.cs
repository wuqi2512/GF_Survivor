using cfg;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class HeroLogic : Targetable
{
    public BulletType BulletType = BulletType.Bullet;
    private float m_ShootTimer;
    private HeroData m_HeroData;

    public override float Hp
    {
        get
        {
            return m_HeroData.ChaAttribute[NumericType.Hp].Value;
        }
        protected set
        {
            m_HeroData.ChaAttribute[NumericType.Hp].SetBase(value);
        }
    }
    public override float MaxHp => m_HeroData.ChaAttribute[NumericType.MaxHp].Value;
    public override CampType Camp => CampType.Player;
    public HeroData HeroData => m_HeroData;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_HeroData = userData as HeroData;
        if (m_HeroData == null)
        {
            Log.Error("HeroData is invalid.");
        }
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (m_Pause)
        {
            return;
        }

        Vector2 moveInput = GetInput();
        SetMoveVelocity(moveInput * m_HeroData.ChaAttribute[NumericType.MoveSpeed].Value);

        m_ShootTimer += elapseSeconds;
        if (m_ShootTimer > 1 / m_HeroData.ChaAttribute[NumericType.AttackSpeed].Value && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = mousePos - pos;

            float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            BulletData bulletData = BulletData.Create((int)BulletType, GameEntry.Entity.GenerateSerialId(),
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

    private static Vector2 GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }

    public ChaAttribute GetChaAttribute()
    {
        return m_HeroData.ChaAttribute;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constant.Layer.DropItemId)
        {
            DropItemLogic logic = collision.gameObject.GetComponent<DropItemLogic>();
            if (logic == null || logic.IsDestroyed)
            {
                return;
            }

            GameEntry.Player.GainDropItem(logic.GetDropItemData());
            logic.Hide();
        }
    }
}