using UnityEngine;
using Cysharp.Threading.Tasks;

public class ProgressPanel : EntityBase
{
    [Header("UI設定")]
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -300);
    [SerializeField] private Vector2 visiblePosition = new Vector2(0, 0);
    [SerializeField] private float slideDuration = 0.5f;

    [Header("動作設定")]
    [SerializeField] private float idleTimeThreshold = 2f;

    [Header("参照")]
    [SerializeField] private ProgressIcon[] progressIcons;

    private PlayerLocomotionManager playerLocomotionManager;
    private bool isHandling = false;
    private int iconsIndex = 0;

    public void Initialize()
    {
        panelTransform.anchoredPosition = hiddenPosition;
        iconsIndex = 0;
    }

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if (!gameStart) return;

        if (playerLocomotionManager != null && !isHandling)
        {
            HandleIdleDetectionAsync().Forget();
        }
    }

    private async UniTaskVoid HandleIdleDetectionAsync()
    {
        isHandling = true;
        float idleTimer = 0f;

        // プレイヤーが一定時間止まっていたら表示
        while (playerLocomotionManager != null && !playerLocomotionManager.IsMoving)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeThreshold)
                break;

            await UniTask.Yield();
        }

        if (playerLocomotionManager == null || playerLocomotionManager.IsMoving)
        {
            isHandling = false;
            return;
        }

        // スライドイン
        await SlidePanelAsync(visiblePosition);

        // 動き出すまで待つ
        while (playerLocomotionManager != null && !playerLocomotionManager.IsMoving)
        {
            await UniTask.Yield();
        }

        // スライドアウト
        await SlidePanelAsync(hiddenPosition);
        isHandling = false;
    }

    private async UniTask SlidePanelAsync(Vector2 targetPosition)
    {
        Vector2 start = panelTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            panelTransform.anchoredPosition = Vector2.Lerp(start, targetPosition, elapsed / slideDuration);
            await UniTask.Yield();
        }

        panelTransform.anchoredPosition = targetPosition;
    }

    public void AdvanceProgressIcon()
    {
        if (progressIcons == null || progressIcons.Length == 0) return;

        if(iconsIndex < progressIcons.Length)
        {
            progressIcons[iconsIndex].ChangeIcon();
            iconsIndex++;
        }
    }

    public void SetPlayerLocomotionManager(PlayerLocomotionManager manager)
    {
        this.playerLocomotionManager = manager;
    }
}
