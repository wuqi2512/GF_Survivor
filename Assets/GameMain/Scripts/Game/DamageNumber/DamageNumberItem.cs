using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumberItem : MonoBehaviour
{
    private const float AnimSeconds = 0.5f;
    private const float MoveSpeed = 1.0f;

    [SerializeField] private TextMeshProUGUI m_Text;

    private RectTransform m_CachedTransform;
    private bool m_AnimFinished;

    public void Init(Vector3 ownerPosition, Canvas parentCanvas, string number)
    {
        gameObject.SetActive(true);
        m_Text.text = number;
        m_AnimFinished = false;

        Vector3 worldPosition = ownerPosition;
        Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)parentCanvas.transform, screenPosition,
            null, out position))
        {
            m_CachedTransform.localPosition = position;
        }

        worldPosition += Vector3.up * AnimSeconds * MoveSpeed;
        screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);
        Vector2 toPosition = position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)parentCanvas.transform,
            screenPosition, null, out toPosition);

        StartCoroutine(DamageNumberCor(toPosition, MoveSpeed));
    }

    public bool Refresh()
    {
        return !m_AnimFinished;
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        m_Text.text = "-1";
        m_AnimFinished = true;
    }

    private void Awake()
    {
        m_CachedTransform = GetComponent<RectTransform>();
    }

    private IEnumerator DamageNumberCor(Vector2 toPosition, float animSeconds)
    {
        yield return m_CachedTransform.MoveAnim(toPosition, animSeconds);
        m_AnimFinished = true;
    }
}