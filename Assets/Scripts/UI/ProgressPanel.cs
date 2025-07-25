using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class ProgressPanel : EntityBase
{
    [Header("ゲームデータの参照")]
    [SerializeField] private GameStateData gameStateData;

    [Header("UI設定")]
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -300);
    [SerializeField] private Vector2 visiblePosition = new Vector2(0, 0);
    [SerializeField] private float slideDuration = 0.5f;

    [Header("動作設定")]
    [SerializeField] private float idleTimeThreshold = 2f;

    [Header("参照")]
    [SerializeField] private ProgressIcon[] progressIcons;

    private PlayerManager playerManager;
    private PlayerLocomotionManager playerLocomotionManager;
    private FinishBanner finishBanner;
    private TimerManager timerManager;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private int iconsIndex = 0;
    private bool isHandling = false;
    private bool isPanelVisible = false;
    private bool isClear = false;

    public bool IsClear => isClear;

    public void Initialize()
    {
        panelTransform.anchoredPosition = hiddenPosition;
        iconsIndex = 0;
    }

    public override void GameStart()
    {
        base.GameStart();
        cts = new CancellationTokenSource();
    }

    public override void GameEnd()
    {
        base.GameEnd();
        cts.Cancel();
        cts.Dispose();
    }

    public void GameLoopUpdate()
    {
        if (!gameStart) return;

        if (CheckClear() && !isClear)
        {
            isClear = true;
            gameStateData.clearTime = timerManager.RemainingTime;
            finishBanner?.ShowFinishBannerAsync().Forget();
        }

        if(playerManager != null)
        {
            if(playerManager.OnCheckPoint && !isPanelVisible)
            {
                SlidePanelAsync(visiblePosition, cts.Token).Forget();
                isPanelVisible = true;
            }
            else if(!playerManager.OnCheckPoint && isPanelVisible)
            {
                SlidePanelAsync(hiddenPosition, cts.Token).Forget();
                isPanelVisible = false;
            }
        }

        if (playerLocomotionManager != null && !isHandling)
        {
            HandleIdleDetectionAsync().Forget();
        }
    }

    private async UniTaskVoid HandleIdleDetectionAsync()
    {
        if (!gameStart || cts?.IsCancellationRequested == true) return;

        isHandling = true;
        float idleTimer = 0f;

        try
        {
            // プレイヤーが止まっていたら表示
            while (playerLocomotionManager != null && !playerLocomotionManager.IsMoving)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTimeThreshold)
                    break;

                await UniTask.Yield(cts.Token); // キャンセル可能にする
            }

            if (playerLocomotionManager == null || playerLocomotionManager.IsMoving)
            {
                isHandling = false;
                return;
            }

            await SlidePanelAsync(visiblePosition, cts.Token);

            while (playerLocomotionManager != null && !playerLocomotionManager.IsMoving)
            {
                await UniTask.Yield(cts.Token);
            }

            await SlidePanelAsync(hiddenPosition, cts.Token);
        }
        catch (OperationCanceledException)
        {
            // キャンセル時は無視して終了
        }

        isHandling = false;
    }


    private async UniTask SlidePanelAsync(Vector2 targetPosition, CancellationToken token)
    {
        if (!gameStart || panelTransform == null) return;

        Vector2 start = panelTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            panelTransform.anchoredPosition = Vector2.Lerp(start, targetPosition, elapsed / slideDuration);
            await UniTask.Yield(token); // トークンで中断可能
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

    private bool CheckClear()
    {
        foreach(ProgressIcon icon in progressIcons)
        {
            if (!icon.IsComplete)
            {
                return false;
            }

        }

        return true;
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public void SetPlayerLocomotionManager(PlayerLocomotionManager manager)
    {
        this.playerLocomotionManager = manager;
    }

    public void SetFinishBanner(FinishBanner finishBanner)
    {
        this.finishBanner = finishBanner;
    }

    public void SetTimerManager(TimerManager timerManager)
    {
        this.timerManager = timerManager;
    }

    public void ClearDebug()
    {
        foreach (var icon in progressIcons)
        {
            icon.ChangeIcon();
        }
    }
}
