using UnityEngine;
using UnityEngine.UI;

public class ProgressIcon : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Image progressIconUI;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite completeImage;

    public void ChangeIcon()
    {
        Debug.Log("行けてんで");
        progressIconUI.sprite = completeImage;
    }
}
