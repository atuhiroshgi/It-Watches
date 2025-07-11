using UnityEngine;
using UnityEngine.UI;

public class HPGauge : EntityBase
{
    [Header("�Q�Ɛݒ�")]
    [SerializeField] private Image hpGaugeImageUI;

    [Header("�J���[���[�h�ݒ�")]
    [SerializeField] private bool useGradientColor = true;

    [Header("HP�J���[�X�e�[�W(����������)")]
    [SerializeField] private Color highColor = Color.green;
    [SerializeField] private Color midColor = Color.yellow;
    [SerializeField] private Color lowColor = Color.red;

    private PlayerManager playerManager;

    public void Initialize()
    {
        hpGaugeImageUI.color = highColor;
    }

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if (playerManager == null || hpGaugeImageUI == null || !gameStart) return;

        float healthRatio = playerManager.CurrentHealth / playerManager.MaxHealth;
        hpGaugeImageUI.fillAmount = healthRatio;

        // �F�̐ݒ�
        hpGaugeImageUI.color = useGradientColor
            ? GetGradientColor(healthRatio)
            : GetSteppedColor(healthRatio);
    }

    private Color GetGradientColor(float ratio)
    {
        if (ratio > 0.5f)
        {
            // �΁���
            float t = (ratio - 0.5f) * 2f;
            return Color.Lerp(midColor, highColor, t);
        }
        else
        {
            // ������
            float t = ratio * 2f;
            return Color.Lerp(lowColor, midColor, t);
        }
    }

    private Color GetSteppedColor(float ratio)
    {
        if (ratio > 0.6f)
        {
            return highColor;
        }
        else if (ratio > 0.3f)
        {
            return midColor;
        }
        else
        {
            return lowColor;
        }
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
}
