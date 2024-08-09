using UnityEngine;
using UnityGameFramework.Runtime;

public class DropItemLogic : EntityLogicWithData
{
    private bool m_IsDestroyed;
    private DropItemData m_DropItemData;
    private SpriteRenderer m_SpriteRenderer;

    public bool IsDestroyed => m_IsDestroyed;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        m_DropItemData = userData as DropItemData;
        if (m_DropItemData == null)
        {
            Log.Error("DropItemData is invalid.");
        }

        GameEntry.Resource.LoadAsset(m_DropItemData.SpriteAssetPath, (asset) =>
        {
            Texture2D texture = (Texture2D)asset;
            m_SpriteRenderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        });
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);

        m_DropItemData = null;
        m_SpriteRenderer.sprite = null;
        m_IsDestroyed = false;
    }

    public void Hide()
    {
        m_IsDestroyed = true;
        GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Id));
    }

    public DropItemData GetDropItemData()
    {
        return m_DropItemData;
    }
}