using cfg;
using GameFramework;
using UnityEngine;

public class AttributeList : MonoBehaviour
{
    public AttributeItem[] Items;
    public GameObject ItemPrefab;
    public ChaAttribute ChaAttribute;

    public void Init()
    {
        Items = new AttributeItem[4];
        for (int i = 0; i < Items.Length; i++)
        {
            GameObject obj = Instantiate(ItemPrefab, gameObject.transform);
            Items[i] = obj.GetComponent<AttributeItem>();
        }
    }

    public void UpdateItem()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            NumericType numericType = (NumericType)(i + 1);
            Items[i].NameText.text = numericType.ToString();
            Numeric numeric = ChaAttribute[numericType];
            string valueText = Utility.Text.Format("{0}=({1}+{2})*{3}%", numeric.Value, numeric.Base, numeric.Add, numeric.Pct + 100f);
            Items[i].ValueText.text = valueText;
        }
    }
}