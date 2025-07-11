using UnityEngine;
using UnityEngine.UI;

public class HPGauge : EntityBase
{
    [Header("参照設定")]
    [SerializeField] private Image hpGaugeImageUI;

    [Header("カラーモード設定")]
    [SerializeField] private bool useGradientColor = true;

    [Header("HPカラーステージ(高→中→低)")]
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

        // 色の設定
        hpGaugeImageUI.color = useGradientColor
            ? GetGradientColor(healthRatio)
            : GetSteppedColor(healthRatio);
    }

    private Color GetGradientColor(float ratio)
    {
        if (ratio > 0.5f)
        {
            // 緑→黄
            float t = (ratio - 0.5f) * 2f;
            return Color.Lerp(midColor, highColor, t);
        }
        else
        {
            // 黄→赤
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
