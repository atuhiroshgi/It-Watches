using UnityEngine;
using UnityEngine.UI;

public class ProgressIcon : MonoBehaviour
{
    [Header("UIê›íË")]
    [SerializeField] private Image progressIconUI;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite completeImage;

    public void ChangeIcon()
    {
        Debug.Log("çsÇØÇƒÇÒÇ≈");
        progressIconUI.sprite = completeImage;
    }
}
