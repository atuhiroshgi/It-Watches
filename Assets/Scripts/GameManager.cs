using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("各クラスの参照")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PlayerLocomotionManager playerLocomotionManager;
    [SerializeField] private PlayerAttackManager playerAttackManager;
    [SerializeField] private EnemyManager[] enemyManagers;

    [Header("UI関連のクラスの参照")]
    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private StartSignalManager startSignalManager;
    [SerializeField] private SkillGauge skillGauge;

    private bool gameStartFlag = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InjectDependencies();
        CallCustomAwake();
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
    }

    private void CallCustomAwake()
    {
        playerLocomotionManager.Setup();
        crosshairManager.Setup();
        startSignalManager.Setup();

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

        foreach (EnemyManager enemy in enemyManagers)
        {
            enemy.GameLoopUpdate();
        }
    }

    private void LateUpdateGameLoop()
    {
        playerCamera.GameLoopLateUpdate();
    }

    private void ForDebug()
    {

    }
}
