using UnityEngine;
using UnityEngine.UI;

public class ModuleForm : MonoBehaviour
{
    private Camera m_Camera;
    private RenderTexture m_RenderTexture;
    private RawImage m_RawImage;
    public GameObject m_Module;
    private Vector3 m_Offset = new Vector3(0f, 0f, -2f);

    private void Start()
    {
        m_Camera = CreateCamera();
        m_Camera.cullingMask = LayerMask.NameToLayer("Module");
        m_Camera.clearFlags = CameraClearFlags.SolidColor;
        m_Camera.transform.position = m_Module.transform.position + m_Offset;

        m_RawImage = CreateRawImage(this.transform);
        RectTransform rectTransform = m_RawImage.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);

        m_RenderTexture = CreateRenderTexture();
        m_Module.layer = LayerMask.NameToLayer("Module");

        m_Camera.targetTexture = m_RenderTexture;
        m_RawImage.texture = m_RenderTexture;
    }

    private RawImage CreateRawImage(Transform parent)
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(parent);
        obj.name = "ModuleRawImage";
        obj.AddComponent<RectTransform>();
        obj.AddComponent<CanvasRenderer>();
        return obj.AddComponent<RawImage>();
    }

    private Camera CreateCamera()
    {
        GameObject obj = new GameObject();
        obj.name = "ModuleCamera";
        return obj.AddComponent<Camera>();
    }

    private RenderTexture CreateRenderTexture()
    {
        return RenderTexture.GetTemporary((int)m_RawImage.rectTransform.rect.width, (int)m_RawImage.rectTransform.rect.height, 1, RenderTextureFormat.ARGB32);
    }
}