using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IPointerClickHandler
{
    public int EquipmentId;
    public Image Image;
    public event Action<int> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick(EquipmentId);
    }
}