using cfg;
using GameFramework;
using GameFramework.Resource;
using StarForce;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class SlotItem : MonoBehaviour, IPointerClickHandler
{
    public Sprite[] Frames;
    public EquipmentType EquipmentType;

    public Image Frame;
    public Image Icon;
    public TextMeshProUGUI LeftUpText;
    public TextMeshProUGUI LeftBottomText;
    public RedDotItem RedDotItem;

    public EquipmentData EquipmentData;
    public event Action<EquipmentData> OnClick;
    private LoadAssetCallbacks m_LoadAssetCallback;

    private void Awake()
    {
        m_LoadAssetCallback = new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFailure);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EquipmentData == null)
        {
            return;
        }

        OnClick(EquipmentData);
    }

    public void SetEquipmentData(EquipmentData data)
    {
        EquipmentData = data;
        LeftBottomText.text = Utility.Text.Format("Lv.{0}", EquipmentData.Level.ToString());
        Frame.sprite = Frames[(int)EquipmentData.Equipment.Quality];
        GameEntry.Resource.LoadAsset(AssetUtility.GetEquipmentSpriteAsset(EquipmentData.Equipment.AssetName), m_LoadAssetCallback);
    }

    public void Clear()
    {
        EquipmentData = null;
        LeftBottomText.text = string.Empty;
        Icon.sprite = null;
        LeftUpText.gameObject.SetActive(false);
        RedDotItem.SetVisible(false);
    }

    public void SetEquppiedTextVisible(bool isEquipped)
    {
        if (!isEquipped)
        {
            LeftUpText.text = "E";
        }
        LeftUpText.gameObject.SetActive(isEquipped);
    }

    public void SetLevelText(int level)
    {
        LeftBottomText.text = Utility.Text.Format("Lv.{0}", level.ToString());
    }

    private void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
    {
        Texture2D texture = (Texture2D)asset;
        Icon.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void LoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        Log.Error("Load asset failure with '{0}'.", errorMessage);
    }
}