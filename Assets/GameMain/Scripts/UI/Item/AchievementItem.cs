using GameFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class AchievementItem : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescriptionText;
    public Slider Progress;
    public TextMeshProUGUI ProgressText;
    public TextMeshProUGUI AwardNumText;
    public GameObject CheckIcon;
    public RedDotItem RedDot;

    public event Action<AchievementData> OnClick;
    public AchievementData AchievementData;

    public void SetAchievementData(AchievementData achievementData)
    {
        this.AchievementData = achievementData;
        NameText.text = GameEntry.Localization.GetString(achievementData.Achievement.NameKey);
        DescriptionText.text = Utility.Text.Format(GameEntry.Localization.GetString(achievementData.Achievement.DescriptionKey), achievementData.MaxValue.ToString());
        Progress.value = (float)achievementData.Value / achievementData.MaxValue;
        ProgressText.text = Utility.Text.Format("{0}/{1}", achievementData.Value, achievementData.MaxValue);
        AwardNumText.text = achievementData.Achievement.AwardDiamond.ToString();
        CheckIcon.gameObject.SetActive(achievementData.HasReceiveAward);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AchievementData == null)
        {
            return;
        }

        Log.Warning("Click");
        OnClick(AchievementData);
    }
}