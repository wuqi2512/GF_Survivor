using StarForce;
using UnityEngine;
using UnityEngine.UI;

public class BlackEdgeForm : UGuiForm
{
    private enum BlackEdgeType
    {
        None,
        Height,
        Width,
    }

    [SerializeField] private RectTransform RootCanvas;
    [SerializeField] private RectTransform[] m_BlackEdges;
    private RectTransform m_RectTrans;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        // TODO
        RootCanvas = CachedTransform.parent.parent.GetComponent<RectTransform>();
        m_RectTrans = GetComponent<RectTransform>();
        m_BlackEdges = new RectTransform[2];
        m_BlackEdges[0] = CreateBlackEdge(m_RectTrans);
        m_BlackEdges[1] = CreateBlackEdge(m_RectTrans);

        BlackEdageAdapt();
    }

    private BlackEdgeType GetBlackEdgeType()
    {
        BlackEdgeType edgeType = BlackEdgeType.None;
        ScreenOrientation orientation = Screen.orientation;
        if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
        {
            edgeType = BlackEdgeType.Height;
        }
        else if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown)
        {
            edgeType = BlackEdgeType.Width;
        }

        return edgeType;
    }

    private void BlackEdageAdapt()
    {
        BlackEdgeType edgeType = GetBlackEdgeType();
        if (edgeType == BlackEdgeType.Height)
        {
            float blockHeight = RootCanvas.rect.width - m_RectTrans.rect.width;

            var temp = m_BlackEdges[0];
            temp.SetAnchor(AnchorPreset.VertStretchRight);
            temp.SetPivot(PivotPreset.MiddleLeft);
            temp.sizeDelta = new Vector2(blockHeight, 0f);
            temp.anchoredPosition = new Vector2(0f, 0f);

            temp = m_BlackEdges[1];
            temp.SetAnchor(AnchorPreset.VertStretchLeft);
            temp.SetPivot(PivotPreset.MiddleRight);
            temp.sizeDelta = new Vector2(blockHeight, 0f);
            temp.anchoredPosition = new Vector2(0f, 0f);
        }
        else if (edgeType == BlackEdgeType.Width)
        {
            float blockHeight = RootCanvas.rect.height - m_RectTrans.rect.height;

            var temp = m_BlackEdges[0];
            temp.SetAnchor(AnchorPreset.HorStretchTop);
            temp.SetPivot(PivotPreset.BottomCenter);
            temp.sizeDelta = new Vector2(0f, blockHeight);
            temp.anchoredPosition = new Vector2(0f, 0f);

            temp = m_BlackEdges[1];
            temp.SetAnchor(AnchorPreset.HorStretchBottom);
            temp.SetPivot(PivotPreset.TopCenter);
            temp.sizeDelta = new Vector2(0f, blockHeight);
            temp.anchoredPosition = new Vector2(0f, 0f);
        }
    }

    private RectTransform CreateBlackEdge(RectTransform parent)
    {
        GameObject obj = new GameObject();
        RectTransform rectTrans = obj.AddComponent<RectTransform>();
        obj.AddComponent<CanvasRenderer>();
        obj.AddComponent<Image>().color = Color.black;
        Transform trans = obj.transform;
        trans.transform.SetParent(parent);
        trans.localPosition = new Vector2(0f, 0f);
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector2(1f, 1f);

        return rectTrans;
    }
}