using UnityEngine;
using UnityEngine.UI;

public class ProgressIcon : MonoBehaviour
{
    [Header("UI�ݒ�")]
    [SerializeField] private Image progressIconUI;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite completeImage;

    public void ChangeIcon()
    {
        Debug.Log("�s���Ă��");
        progressIconUI.sprite = completeImage;
    }
}
