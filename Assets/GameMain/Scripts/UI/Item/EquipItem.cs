using cfg;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour, IPointerClickHandler
{
    public EquipmentType EquipmentType;
    public int EquipmentId = -1;
    public Image Image;
    public event Action<int> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EquipmentId == -1)
        {
            return;
        }

        OnClick(EquipmentId);
    }
}