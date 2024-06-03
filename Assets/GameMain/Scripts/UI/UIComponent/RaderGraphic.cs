using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaderGraphic : MaskableGraphic
{
    [SerializeField] private List<float> m_VerticesLength = new List<float>();
    [SerializeField] private float m_Length;
    [SerializeField][Range(0, 360)] private float m_Rotation;

    [SerializeField]Texture m_Texture;

    public override Texture mainTexture
    {
        get
        {
            return m_Texture == null ? s_WhiteTexture : m_Texture;
        }
    }

    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value) return;
            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        int sides = m_VerticesLength.Count;
        float deltaDegree = 360f / sides;
        m_VerticesLength.Add(m_VerticesLength[0]);

        UIVertex uiVertex = new UIVertex();
        for (int i = 0; i < m_VerticesLength.Count; i++)
        {
            uiVertex.position = CalculatePos(m_Length * m_VerticesLength[i], i * deltaDegree + m_Rotation);
            uiVertex.color = base.color;
            vh.AddVert(uiVertex);
        }

        for (int i = 1; i < sides; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }

        m_VerticesLength.RemoveAt(m_VerticesLength.Count - 1);
    }

    private static Vector2 CalculatePos(float length, float degree)
    {
        float rad = Mathf.Deg2Rad * degree;
        return new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * length;
    }
}
