using GameFramework;

public class Numeric : IReference
{
    private float m_Base;
    private float m_Add;
    private float m_Pct;
    private float m_Value;

    public void UpdateFinal()
    {
        m_Value = (m_Base + m_Add) * (100f + m_Pct) / 100f;
    }

    public float Base
    {
        get { return m_Base; }
        set
        {
            m_Base = value;
            UpdateFinal();
        }
    }

    public float Add
    {
        get { return m_Add; }
        set
        {
            m_Add = value;
            UpdateFinal();
        }
    }

    public float Pct
    {
        get { return m_Pct; }
        set
        {
            m_Pct = value;
            UpdateFinal();
        }
    }

    public float Value => m_Value;

    public static Numeric Create()
    {
        Numeric numeric = ReferencePool.Acquire<Numeric>();

        return numeric;
    }

    public void Clear()
    {
        m_Base = 0;
        m_Add = 0;
        m_Pct = 0;
        m_Value = 0;
    }
}