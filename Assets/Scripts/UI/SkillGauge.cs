using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillGauge : EntityBase
{
    [Header("UI‚ÌŽQÆ")]
    [SerializeField] private Image skillGaugeUI;
    [SerializeField] private TextMeshProUGUI skillCountTextUI;

    [Header("Ý’è")]
    [SerializeField] private float fillSpeed = 0.2f;
    [SerializeField] private int maxCount = 5;

    private float currentFillAmount = 0f;
    private int currentCount = 0;

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if (!gameStart) return;

        if (currentCount >= maxCount) return;

        currentFillAmount += fillSpeed * Time.deltaTime;
        skillGaugeUI.fillAmount = currentFillAmount;

        if (currentFillAmount >= 1f)
        {
            currentFillAmount = 0f;
            currentCount++;
            skillCountTextUI.text = $"{currentCount}";
        }
    }

    public bool DecreaseSkillCount(int amount)
    {
        if(currentCount >= amount)
        {
            currentCount -= amount;
            skillCountTextUI.text = $"{currentCount}";
            return true;
        }

        return false;
    }
}
