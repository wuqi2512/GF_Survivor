using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileItem : MonoBehaviour
{
    public RectTransform RectTrans;
    public TextMeshProUGUI Text;
    public Image Image;

    private void Awake()
    {
        RectTrans = GetComponent<RectTransform>();
    }

    public void SetValue(int value)
    {
        Text.text = value.ToString();
    }

    public void SetPos(Vector2 pos)
    {
        RectTrans.anchoredPosition = pos;
    }

    public void SetColor(Color color)
    {
        Image.color = color;
    }

    public void SetSize(Vector2 size)
    {
        RectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        RectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    public void MergeScaleAnimCor(float toScale, float duration)
    {
        StartCoroutine(RectTrans.ScaleForwardThenBackAnim(toScale, duration));
    }

    public void MoveAnimCor(Vector2 toPos, float duration)
    {
        StartCoroutine(RectTrans.MoveAnim(toPos, duration));
    }

    public void AddAnimCor(float duration)
    {
        StartCoroutine(RectTrans.ScaleAnim(1f, duration));
    }
}
