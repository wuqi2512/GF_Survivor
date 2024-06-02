using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int Segment = 100;
    private float Percentage = 0.30f;
    private Color32 DisableColor = new Color32(60, 60, 60, 255);

    private RectTransform m_RectTrans;
    private Vector2 m_Ratio;
    private float m_Radius;
    private Vector3 m_Offset;
    private Vector2 m_UVOffset;
    private float m_DeltaRadian;
    private List<Vector2> m_Vertexes;

    protected override void Start()
    {
        m_RectTrans = GetComponent<RectTransform>();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();

        float width = m_RectTrans.rect.width;
        float height = m_RectTrans.rect.height;
        Vector4 uv = DataUtility.GetOuterUV(overrideSprite);
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;

        m_Ratio = new Vector2(uvWidth / width, uvHeight / height);
        m_Radius = Mathf.Min(width, height) * 0.5f;
        m_Offset = new Vector3((0.5f - m_RectTrans.pivot.x) * width, (0.5f - m_RectTrans.pivot.y) * height, 0f);
        m_UVOffset = new Vector2(uvWidth, uvHeight) * 0.5f;
        m_DeltaRadian = 2 * Mathf.PI / Segment;
        m_Vertexes = new List<Vector2>();
        CalculateVertex(Segment, m_Radius, m_Vertexes);

        UIVertex origin = new UIVertex();
        origin.position = m_Offset;
        origin.uv0 = m_UVOffset;
        origin.color = color;
        toFill.AddVert(origin);

        UIVertex disableOrigin = new UIVertex();
        disableOrigin.position = m_Offset;
        disableOrigin.uv0 = m_UVOffset;
        disableOrigin.color = DisableColor;
        toFill.AddVert(disableOrigin);

        float deltaRadian = 2 * Mathf.PI / Segment;
        int temp = (int)(Segment * Percentage);

        AddSector(0f, temp, origin, this.color, toFill);
        AddSector(temp * deltaRadian, Segment - temp, disableOrigin, DisableColor, toFill);
    }

    private void AddSector(float startRadian, int count, UIVertex origin, Color32 color, VertexHelper vh)
    {
        Vector2 ratio = m_Ratio;
        float radius = m_Radius;
        Vector3 offset = m_Offset;
        Vector2 uvOffset = m_UVOffset;
        float deltaRadian = m_DeltaRadian;

        vh.AddVert(origin);
        int originId = vh.currentVertCount - 1;
        float curRadian = startRadian;
        for (int i = 0; i < count + 1; i++)
        {
            float x = Mathf.Sin(curRadian) * radius;
            float y = Mathf.Cos(curRadian) * radius;

            UIVertex tempVertex = new UIVertex();
            tempVertex.position = new Vector3(x, y, 0f) + offset;
            tempVertex.uv0 = new Vector4(x * ratio.x, y * ratio.y, 0f, 0f) + (Vector4)uvOffset;
            tempVertex.color = color;
            vh.AddVert(tempVertex);

            curRadian += deltaRadian;
        }

        int id = originId + 1;
        for (int i = 0; i < count; i++)
        {
            vh.AddTriangle(id, id + 1, originId);
            id++;
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTrans, screenPoint, eventCamera, out localPoint);
        return CrossingNumber(localPoint, m_Vertexes) % 2 == 1;
    }

    private static void CalculateVertex(int segment, float radius, List<Vector2> list)
    {
        list.Clear();

        float deltaRadian = 2 * Mathf.PI / segment;
        float curRadian = 0f;
        for (int i = 0; i < segment; i++)
        {
            float x = Mathf.Sin(curRadian) * radius;
            float y = Mathf.Cos(curRadian) * radius;
            list.Add(new Vector2(x, y));

            curRadian += deltaRadian;
        }
    }

    private static int CrossingNumber(Vector2 point, List<Vector2> polygonVertexes)
    {
        int count = 0;
        int vertexCount = polygonVertexes.Count;
        for (int i = 0; i < vertexCount; i++)
        {
            Vector2 vertex0 = polygonVertexes[i];
            Vector2 vertex1 = polygonVertexes[(i + 1) % vertexCount];
            if (!IsInRange(point.y, vertex0.y, vertex1.y))
            {
                continue;
            }

            float k = (vertex0.y - vertex1.y) / (vertex0.x - vertex1.x);
            float x = vertex0.x + (point.y - vertex0.y) / k;
            if (x > point.x)
            {
                count++;
            }
        }

        return count;
    }

    private static bool IsInRange(float x, float a, float b)
    {
        return x >= Mathf.Min(a, b) && x <= Mathf.Max(a, b);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerClick CircleImage");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        color = Color.white;
    }
}