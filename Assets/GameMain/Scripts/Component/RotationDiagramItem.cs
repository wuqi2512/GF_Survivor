using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotationDiagramItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int PosId;
    private Transform m_Trans;
    private RectTransform m_RectTrans;
    private Image m_Image;
    private Action<float> m_OnDragEvent;
    private float m_DragMovemnt;

    private static readonly float s_AnimDuration = 0.5f;

    public void Init()
    {
        m_Trans = GetComponent<Transform>();
        m_RectTrans = GetComponent<RectTransform>();
        m_Image = GetComponent<Image>();
    }

    public void SetPosData(RotationDiagram.ItemPosData itemPosData)
    {
        StartCoroutine(m_RectTrans.MoveAnim(Vector2.right * itemPosData.PosX, s_AnimDuration));
        StartCoroutine(m_RectTrans.ScaleAnim(itemPosData.Scale, s_AnimDuration));
        StartCoroutine(CorSetOrder(itemPosData.Order));
    }

    private IEnumerator CorSetOrder(int order)
    {
        yield return new WaitForSeconds(s_AnimDuration / 2);
        m_Trans.SetSiblingIndex(order);
    }

    public void SetSprite(Sprite sprite)
    {
        m_Image.sprite = sprite;
    }

    public void AddListener(Action<float> action)
    {
        m_OnDragEvent += action;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_DragMovemnt += eventData.delta.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_OnDragEvent != null)
        {
            m_OnDragEvent(m_DragMovemnt);
        }
        m_DragMovemnt = 0f;
    }
}