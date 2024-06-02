using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

static internal class UIMenuOptionsExtend
{
    [MenuItem("GameObject/UI/WithoutRaycastTarget/Image - WithoutRay", false, 10)]
    static void CreatImage(MenuCommand menuCommand)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        GameObject go = Selection.activeGameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        go.GetComponent<Image>().raycastTarget = false;
    }

    [MenuItem("GameObject/UI/WithoutRaycastTarget/RawImage - WithoutRay", false, 10)]
    static void CreateRawImage(MenuCommand menuCommand)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Raw Image");
        GameObject go = Selection.activeGameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        go.GetComponent<RawImage>().raycastTarget = false;
    }

    [MenuItem("GameObject/UI/WithoutRaycastTarget/Text TMP - WithoutRay", false, 10)]
    static void CreateTMPText(MenuCommand menuCommand)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Text - TextMeshPro");
        GameObject go = Selection.activeGameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        go.GetComponent<TextMeshProUGUI>().raycastTarget = false;
    }

    [MenuItem("GameObject/UI/WithoutRaycastTarget/Text - WithoutRay", false, 10)]
    static void CreateLegacyText(MenuCommand menuCommand)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Legacy/Text");
        GameObject go = Selection.activeGameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        go.GetComponent<Text>().raycastTarget = false;
    }
}