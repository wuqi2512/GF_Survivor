using System.Collections;
using UnityEngine;

public static class UIExtension
{
    public static void AdaptToSafeArea(this RectTransform rectTrans)
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        rectTrans.anchorMin = anchorMin;

        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        rectTrans.anchorMax = anchorMax;
    }

    public static void SetAnchor(this RectTransform rectTrans, AnchorPreset preset, int offsetX = 0, int offsetY = 0)
    {
        rectTrans.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (preset)
        {
            case (AnchorPreset.TopLeft):
                {
                    rectTrans.anchorMin = new Vector2(0, 1);
                    rectTrans.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPreset.TopCenter):
                {
                    rectTrans.anchorMin = new Vector2(0.5f, 1);
                    rectTrans.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPreset.TopRight):
                {
                    rectTrans.anchorMin = new Vector2(1, 1);
                    rectTrans.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPreset.MiddleLeft):
                {
                    rectTrans.anchorMin = new Vector2(0, 0.5f);
                    rectTrans.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPreset.MiddleCenter):
                {
                    rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPreset.MiddleRight):
                {
                    rectTrans.anchorMin = new Vector2(1, 0.5f);
                    rectTrans.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPreset.BottomLeft):
                {
                    rectTrans.anchorMin = new Vector2(0, 0);
                    rectTrans.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPreset.BottonCenter):
                {
                    rectTrans.anchorMin = new Vector2(0.5f, 0);
                    rectTrans.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPreset.BottomRight):
                {
                    rectTrans.anchorMin = new Vector2(1, 0);
                    rectTrans.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPreset.HorStretchTop):
                {
                    rectTrans.anchorMin = new Vector2(0, 1);
                    rectTrans.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPreset.HorStretchMiddle):
                {
                    rectTrans.anchorMin = new Vector2(0, 0.5f);
                    rectTrans.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPreset.HorStretchBottom):
                {
                    rectTrans.anchorMin = new Vector2(0, 0);
                    rectTrans.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPreset.VertStretchLeft):
                {
                    rectTrans.anchorMin = new Vector2(0, 0);
                    rectTrans.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPreset.VertStretchCenter):
                {
                    rectTrans.anchorMin = new Vector2(0.5f, 0);
                    rectTrans.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPreset.VertStretchRight):
                {
                    rectTrans.anchorMin = new Vector2(1, 0);
                    rectTrans.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPreset.StretchAll):
                {
                    rectTrans.anchorMin = new Vector2(0, 0);
                    rectTrans.anchorMax = new Vector2(1, 1);
                    rectTrans.sizeDelta = Vector2.zero;
                    break;
                }
        }
    }

    public static void SetPivot(this RectTransform rectTrans, PivotPreset preset)
    {

        switch (preset)
        {
            case (PivotPreset.TopLeft):
                {
                    rectTrans.pivot = new Vector2(0, 1);
                    break;
                }
            case (PivotPreset.TopCenter):
                {
                    rectTrans.pivot = new Vector2(0.5f, 1);
                    break;
                }
            case (PivotPreset.TopRight):
                {
                    rectTrans.pivot = new Vector2(1, 1);
                    break;
                }

            case (PivotPreset.MiddleLeft):
                {
                    rectTrans.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case (PivotPreset.MiddleCenter):
                {
                    rectTrans.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (PivotPreset.MiddleRight):
                {
                    rectTrans.pivot = new Vector2(1, 0.5f);
                    break;
                }

            case (PivotPreset.BottomLeft):
                {
                    rectTrans.pivot = new Vector2(0, 0);
                    break;
                }
            case (PivotPreset.BottomCenter):
                {
                    rectTrans.pivot = new Vector2(0.5f, 0);
                    break;
                }
            case (PivotPreset.BottomRight):
                {
                    rectTrans.pivot = new Vector2(1, 0);
                    break;
                }
        }
    }

    public static IEnumerator MoveAnim(this RectTransform rectTransform, Vector2 toPos, float duration)
    {
        float timer = 0f;
        Vector2 originalPos = rectTransform.anchoredPosition;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(originalPos, toPos, timer / duration);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.anchoredPosition = toPos;
    }

    public static IEnumerator ScaleAnim(this RectTransform rectTransform, float toScale, float duration)
    {
        float timer = 0f;
        float scale = rectTransform.localScale.x;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newValue = Mathf.Lerp(scale, toScale, timer / duration);
            rectTransform.localScale = new Vector3(newValue, newValue, 1f);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.localScale = new Vector3(toScale, toScale, 1f);
    }

    public static IEnumerator ScaleForwardThenBackAnim(this RectTransform rectTransform, float tosScale, float duration)
    {
        float timer = 0f;
        float halfDuration = duration / 2;
        float originalScale = rectTransform.localScale.x;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float newValue = Mathf.Lerp(originalScale, tosScale, timer / halfDuration);
            rectTransform.localScale = new Vector3(newValue, newValue, 1f);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.localScale = new Vector3(tosScale, tosScale, 1f);

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float newValue = Mathf.Lerp(tosScale, originalScale, timer / halfDuration);
            rectTransform.localScale = new Vector3(newValue, newValue, 1f);
            yield return new WaitForEndOfFrame();
        }
        rectTransform.localScale = new Vector3(originalScale, originalScale, 1f);
    }

    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
    {
        float time = 0f;
        float originAlpha = canvasGroup.alpha;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(originAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = alpha;
    }
}