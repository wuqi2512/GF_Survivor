using GameFramework;
using System.Collections.Generic;

public class BlackBoard : IReference
{
    private Dictionary<string, Variable> m_Datas;

    public BlackBoard()
    {
        m_Datas = new Dictionary<string, Variable>();
    }

    public void SetData(string key, Variable value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new GameFrameworkException(Utility.Text.Format("Key '{0}' is invalid.", key));
        }
        if (value == null)
        {
            throw new GameFrameworkException("Value is null");
        }

        m_Datas[key] = value;
    }

    public T GetData<T>(string key) where T : Variable
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new GameFrameworkException(Utility.Text.Format("Key '{0}' is invalid.", key));
        }

        Variable value;
        if (m_Datas.TryGetValue(key, out value))
        {
            return (T)value;
        }

        throw new GameFrameworkException(Utility.Text.Format("Key '{0}' is not exist.", key));
    }

    public void RemoveData(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new GameFrameworkException(Utility.Text.Format("Key '{0}' is invalid.", key));
        }

        m_Datas.Remove(key);
    }

    public static BlackBoard Create()
    {
        return ReferencePool.Acquire<BlackBoard>();
    }

    public void Clear()
    {
        m_Datas.Clear();
    }
}