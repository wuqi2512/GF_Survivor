//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private const float FadeTime = 0.3f;
    private const float OnHoverAlpha = 0.7f;
    private const float OnClickAlpha = 0.6f;

    private CanvasGroup m_CanvasGroup;

    public event Action OnHover;
    public event Action OnClick;

    private void Awake()
    {
        m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        m_CanvasGroup.alpha = 1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(m_CanvasGroup.FadeToAlpha(OnHoverAlpha, FadeTime));
        OnHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        m_CanvasGroup.alpha = OnClickAlpha;
        OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        m_CanvasGroup.alpha = OnHoverAlpha;
    }
}