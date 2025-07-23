using UnityEngine;
using UnityEngine.UI;

public class ProgressIcon : MonoBehaviour
{
    [Header("UIÝ’è")]
    [SerializeField] private Image progressIconUI;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite completeImage;

    private bool isComplete = false;

    public bool IsComplete => isComplete;

    public void ChangeIcon()
    {
        isComplete = true;
        progressIconUI.sprite = completeImage;
    }
}
