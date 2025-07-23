using UnityEngine;
using UnityEngine.UI;

public class CheckPointPrompt : MonoBehaviour
{
    [Header("UIÝ’è")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private Image gauge;

    public void Setup()
    {
        gauge.fillAmount = 0f;
        HidePrompt();
    }

    public void ShowPrompt()
    {
        promptUI.SetActive(true);
    }

    public void HidePrompt()
    {
        promptUI.SetActive(false);
    }

    public void UpdateGauge(float fillPercent)
    {
        if(fillPercent >= 100)
        {
            HidePrompt();
        }

        gauge.fillAmount = fillPercent;
    }
}
