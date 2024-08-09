using cfg;
using GameFramework;
using UnityEngine;

public class AttributeList : MonoBehaviour
{
    public Transform Content;
    public GameObject ItemPrefab;

    private bool m_Visible = true;
    private AttributeItem[] m_Items;
    private ChaAttribute m_ChaAttribute;

    public bool Visible
    {
        get
        {
            return m_Visible;
        }
        set
        {
            if (m_Visible != value)
            {
                m_Visible = value;
                Content.gameObject.SetActive(m_Visible);
            }
        }
    }

    public void Init(ChaAttribute chaAttribute)
    {
        m_ChaAttribute = chaAttribute;
        m_Items = new AttributeItem[m_ChaAttribute.Count];
        for (int i = 0; i < m_Items.Length; i++)
        {
            GameObject obj = Instantiate(ItemPrefab, Content.transform);
            m_Items[i] = obj.GetComponent<AttributeItem>();
        }
    }

    public void UpdateItem()
    {
        for (int i = 0; i < m_Items.Length; i++)
        {
            NumericType numericType = (NumericType)(i + 1);
            m_Items[i].NameText.text = numericType.ToString();
            Numeric numeric = m_ChaAttribute[numericType];
            string valueText = Utility.Text.Format("{0}=({1}+{2})*{3}%", numeric.Value, numeric.Base, numeric.Add, numeric.Pct + 100f);
            m_Items[i].ValueText.text = valueText;
        }
    }
}