using UnityEngine;

public class MovePreorder
{
    private float m_Duration;
    private Vector3 m_Velocity;
    private float m_ElapsedTime;

    public MovePreorder(float duration, Vector3 velocity)
    {
        this.m_Duration = duration;
        this.m_Velocity = velocity;
    }

    public float Duration => m_Duration;
    public float ElapsedTime => m_ElapsedTime;
    public bool IsFinish => m_ElapsedTime >= m_Duration;

    public Vector3 GetVelocity(float deltaTime)
    {
        m_ElapsedTime += deltaTime;
        if (m_ElapsedTime >= m_Duration)
        {
            return Vector3.zero;
        }

        return m_Velocity;
    }
}