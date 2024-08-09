using TMPro;
using UnityEngine;

public class RedDotItem : MonoBehaviour
{
    public TextMeshProUGUI Number;
    public CanvasGroup CanvasGroup;

    public void SetVisible(bool visible)
    {
        CanvasGroup.alpha = visible ? 1f : 0f;
    }

    public void SetNumber(int value)
    {
        Number.text = value.ToString();
    }

    public void Set(int value)
    {
        if (value <= 0f)
        {
            CanvasGroup.alpha = 0f;
            return;
        }

        CanvasGroup.alpha = 1f;
        SetNumber(value);
    }
}