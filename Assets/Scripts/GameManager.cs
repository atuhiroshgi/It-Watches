using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("各クラスの参照")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PlayerLocomotionManager playerLocomotionManager;
    [SerializeField] private PlayerAttackManager playerAttackManager;
    [SerializeField] private CheckPointGenerator checkPointGenerator;
    [SerializeField] private CheckPoint fixedCheckPoint;
    [SerializeField] private EnemyManager[] enemyManagers;

    [Header("UI関連のクラスの参照")]
    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private StartSignalManager startSignalManager;
    [SerializeField] private SkillGauge skillGauge;
    [SerializeField] private ProgressPanel progressPanel;
    [SerializeField] private CheckPointPrompt checkPointPrompt;
    [SerializeField] private FinishBanner finishBanner;

    private readonly List<CheckPoint> dynamicCheckPoints = new();
    private bool gameStartFlag = false;
    private bool gameStopFlag = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        checkPointGenerator.OnCheckPointSpawned += RegisterCheckPoint;

        InjectDependencies();
        CallCustomAwake();
    }

    private void OnDestroy()
    {
        checkPointGenerator.OnCheckPointSpawned -= RegisterCheckPoint;
    }

    private void Start()
    {
        GameInitialize();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        ForDebug();
        
        if (startSignalManager.IsFinished && !gameStartFlag)
        {
            GameStart();
            gameStartFlag = true;
        }

        if (progressPanel.IsClear && !gameStopFlag)
        {
            GameEnd();
            gameStopFlag = true;
        }

        UpdateGameLoop();
    }

    private void LateUpdate()
    {
        LateUpdateGameLoop();
    }

    private void InjectDependencies()
    {
        playerManager.SetEnemyManagers(enemyManagers);
        
        playerCamera.SetPlayerInputManager(playerInputManager);
        
        playerLocomotionManager.SetPlayerInputManager(playerInputManager);
        playerLocomotionManager.SetPlayerCamera(playerCamera);
        
        playerAttackManager.SetPlayerInputManager(playerInputManager);
        playerAttackManager.SetPlayerCamera(playerCamera);
        playerAttackManager.SetCrosshairManager(crosshairManager);
        playerAttackManager.SetSkillGauge(skillGauge);
        
        hpGauge.SetPlayerManager(playerManager);
        
        progressPanel.SetPlayerManager(playerManager);
        progressPanel.SetPlayerLocomotionManager(playerLocomotionManager);
        progressPanel.SetFinishBanner(finishBanner);

        fixedCheckPoint.SetProgressPanel(progressPanel);
        fixedCheckPoint.SetCheckPointPrompt(checkPointPrompt);
    }

    private void CallCustomAwake()
    {
        playerLocomotionManager.Setup();
        crosshairManager.Setup();
        startSignalManager.Setup();
        checkPointGenerator.Setup();
        finishBanner.Setup();

        foreach (EnemyManager enemy in enemyManagers)
        {
            enemy.SetPlayerTransform(playerLocomotionManager.transform);
            enemy.Setup();
        }
    }

    private void GameInitialize()
    {
        playerManager.Initialize();
        playerCamera.Initialize();
        hpGauge.Initialize();
        progressPanel.Initialize();
    }

    private void GameStart()
    {
        playerManager.GameStart();
        playerInputManager.GameStart();
        playerAttackManager.GameStart();
        playerLocomotionManager.GameStart();
        timerManager.GameStart();
        crosshairManager.GameStart();
        hpGauge.GameStart();
        skillGauge.GameStart();
        progressPanel.GameStart();
    }

    private void GameEnd()
    {
        playerManager.GameEnd();
        playerInputManager.GameEnd();
        playerAttackManager.GameEnd();
        playerLocomotionManager.GameEnd();
        timerManager.GameEnd();
        crosshairManager.GameEnd();
        hpGauge.GameEnd();
        skillGauge.GameEnd();
        progressPanel.GameEnd();
    }

    private void FixedUpdateGameLoop()
    {

    }

    private void UpdateGameLoop()
    {
        playerManager.GameLoopUpdate();
        playerInputManager.GameLoopUpdate();
        playerLocomotionManager.GameLoopUpdate();
        playerAttackManager.GameLoopUpdate();
        hpGauge.GameLoopUpdate();
        timerManager.GameLoopUpdate();
        skillGauge.GameLoopUpdate();
        progressPanel.GameLoopUpdate();
        fixedCheckPoint.GameLoopUpdate();

        foreach (EnemyManager enemy in enemyManagers)
        {
            enemy.GameLoopUpdate();
        }

        foreach(CheckPoint checkPoint in dynamicCheckPoints)
        {
            checkPoint.GameLoopUpdate();
        }
    }

    private void LateUpdateGameLoop()
    {
        playerCamera.GameLoopLateUpdate();
    }

    private void RegisterCheckPoint(CheckPoint checkPoint)
    {
        if(checkPoint == null) return;

        dynamicCheckPoints.Add(checkPoint);
        checkPoint.SetProgressPanel(progressPanel);
        checkPoint.SetCheckPointPrompt(checkPointPrompt);
    }

    private void ForDebug()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            progressPanel.ClearDebug();
        }
    }
}
