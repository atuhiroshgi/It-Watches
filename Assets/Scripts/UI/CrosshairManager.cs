using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : EntityBase
{
    [Header("è∆èÄÇÃê›íË")]
    [SerializeField] private Image crosshairImageUI;
    [SerializeField] private Sprite defaultCrosshairSprite;
    [SerializeField] private Sprite targetingCrosshairSprite;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color targetingColor;

    public void Setup()
    {
        crosshairImageUI.enabled = false;
    }

    public override void GameStart()
    {
        crosshairImageUI.enabled = true;
    }

    public void SetActiveState(bool canHit)
    {
        if (crosshairImageUI == null) return;

        crosshairImageUI.sprite = canHit ? targetingCrosshairSprite : defaultCrosshairSprite;
        crosshairImageUI.color = canHit ? targetingColor : defaultColor;
    }
}
