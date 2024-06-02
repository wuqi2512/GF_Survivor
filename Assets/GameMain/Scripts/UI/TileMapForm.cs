using StarForce;
using System.Collections.Generic;
using UnityEngine;

public partial class TileMapForm : UGuiForm
{
    private const float MergeScale = 1.2f;
    private const float ScaleDuration = 0.2f;
    private const float MoveDuration = 0.2f;
    private const float AddDuration = 0.15f;
    private const float TileSpace = 12f;

    public GameObject BoxPrefab;

    private Vector2 m_BoxSize;
    private TileItem[,] m_Boxs;
    private Dictionary<int, Color32> m_Colors;
    private TileMapController m_Controller;

    private int height = 5;
    private int width = 5;


    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GetBindComponents(this.gameObject);
        float panelSize = m_Rect_Panel.rect.width;
        float boxHeight = (panelSize - TileSpace * (height + 1)) / height;
        float boxWidth = (panelSize - TileSpace * (width + 1)) / width;
        m_BoxSize = new Vector2(boxWidth, boxHeight);
        m_Boxs = new TileItem[height, width];
        InitColors();

        m_Btn_Pause.OnClick += () =>
        {
            GameEntry.UI.OpenUIForm(UIFormId.PauseMenuForm);
        };
        m_TxtP_Score.text = "0";
    }

    public void SetController(TileMapController controller)
    {
        m_Controller = controller;
    }

    private void InitColors()
    {
        m_Colors = new Dictionary<int, Color32>();
        Color color;
        ColorUtility.TryParseHtmlString("#EEE4DA", out color);
        m_Colors.Add(2, color);
        ColorUtility.TryParseHtmlString("#EDE0C8", out color);
        m_Colors.Add(4, color);
        ColorUtility.TryParseHtmlString("#F2B179", out color);
        m_Colors.Add(8, color);
        ColorUtility.TryParseHtmlString("#F59563", out color);
        m_Colors.Add(16, color);
        ColorUtility.TryParseHtmlString("#F67C5F", out color);
        m_Colors.Add(32, color);
        ColorUtility.TryParseHtmlString("#F65E3B", out color);
        m_Colors.Add(64, color);
        ColorUtility.TryParseHtmlString("#EDCF72", out color);
        m_Colors.Add(128, color);
        ColorUtility.TryParseHtmlString("#EDCC61", out color);
        m_Colors.Add(256, color);
        ColorUtility.TryParseHtmlString("#EDC850", out color);
        m_Colors.Add(512, color);
        ColorUtility.TryParseHtmlString("#EDC53F", out color);
        m_Colors.Add(1024, color);
        ColorUtility.TryParseHtmlString("#EDC22E", out color);
        m_Colors.Add(2048, color);
    }

    public void AddBox(Vector2Int pos, int value)
    {
        TileItem box = null;
        if (TryGetBox(pos, out box))
        {
            throw new System.Exception("Alread has box");
        }

        box = InstantiateItem();
        m_Boxs[pos.y, pos.x] = box;
        box.SetPos(GridPosToUIPos(pos));
        SetValue(pos, value);
        box.RectTrans.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine(UnityExtension.WaitToAction(MoveDuration, () => { box.AddAnimCor(AddDuration); }));
    }

    private TileItem InstantiateItem()
    {
        TileItem item = Instantiate(BoxPrefab, m_Rect_Panel).GetComponent<TileItem>();
        item.SetSize(m_BoxSize);
        return item;
    }

    public void SetValue(Vector2Int pos, int value)
    {
        TileItem box = null;
        if (TryGetBox(pos, out box))
        {
            box.SetValue(value);
            if (value == 0)
            {
                box.SetColor(Color.clear);
            }
            else
            {
                box.SetColor(m_Colors[value]);
            }
            return;
        }

        throw new System.Exception("Box is invalid.");
    }

    public bool TryGetBox(Vector2Int pos, out TileItem box)
    {
        box = m_Boxs[pos.y, pos.x];
        return box != null;
    }

    public TileItem GetBox(Vector2Int pos)
    {
        return m_Boxs[pos.y, pos.x];
    }

    public void MoveTile(Vector2Int pos, Vector2Int toPos, int value)
    {
        TileItem box = GetBox(pos);
        TileItem nextBox = GetBox(toPos);
        if (box == null)
        {
            throw new System.Exception("Box is invalid.");
        }

        m_Boxs[pos.y, pos.x] = null;
        if (nextBox == null)
        {
            m_Boxs[toPos.y, toPos.x] = box;
            box.MoveAnimCor(GridPosToUIPos(toPos), MoveDuration);
        }
        else
        {
            box.MoveAnimCor(GridPosToUIPos(toPos), MoveDuration);
            StartCoroutine(UnityExtension.WaitToAction(MoveDuration, () =>
            {
                Destroy(box.gameObject);
                nextBox.MergeScaleAnimCor(MergeScale, ScaleDuration);
                SetValue(toPos, value);
            }));
        }
    }

    public void UpdateScore(int score)
    {
        StartCoroutine(UnityExtension.WaitToAction(MoveDuration, () => { m_TxtP_Score.text = score.ToString(); }));
    }

    public void RestartGame()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                TileItem item = m_Boxs[i, j];
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        m_TxtP_Score.text = "0";
        // m_Controller.Restart();
    }

    private Vector2 GridPosToUIPos(Vector2Int gridPos)
    {
        return new Vector2(TileSpace * (gridPos.x + 1) + m_BoxSize.x * (gridPos.x + 0.5f),
            -(TileSpace * (gridPos.y + 1) + m_BoxSize.y * (gridPos.y + 0.5f)));
    }
}