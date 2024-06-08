using UnityEngine;

public partial class BulletData
{
    public class BulletBehaviour
    {
        private BulletOnCreate m_OnCreate;
        private BulletMoveTween m_MoveTween;
        private BulletOnHit m_OnHit;
        private BulletOnDestroy m_OnDestroy;

        public BulletBehaviour(BulletOnCreate onCreate, BulletMoveTween moveTween, BulletOnHit onHit, BulletOnDestroy onDestroy)
        {
            m_OnCreate = onCreate;
            m_MoveTween = moveTween;
            m_OnHit = onHit;
            m_OnDestroy = onDestroy;
        }

        public void OnCreate(BulletLogic bulletLogic)
        {
            if (m_OnCreate != null)
            {
                m_OnCreate.Invoke(bulletLogic);
            }
        }

        public Vector3 MoveTween(BulletLogic bulletLogic, float elapseSeconds)
        {
            if (m_MoveTween != null)
            {
                return m_MoveTween(bulletLogic, elapseSeconds);
            }

            return Vector3.zero;
        }

        public void OnHit(BulletLogic bulletLogic, Collider2D other)
        {
            if (m_OnHit != null)
            {
                m_OnHit(bulletLogic, other);
            }
        }

        public void OnDestroy(BulletLogic bulletLogic)
        {
            if (m_OnDestroy != null)
            {
                m_OnDestroy(bulletLogic);
            }
        }
    }
}