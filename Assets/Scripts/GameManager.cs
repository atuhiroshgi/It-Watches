using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("各クラスの参照")]
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PlayerLocomotionManager playerLocomotionManager;
    [SerializeField] private EnemyAIManager enemyAIManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InjectDependencies();
        CallCustomAwake();
    }

    private void Start()
    {
        StartGamePlaySystems();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        ForDebug();
        UpdateGameLoop();
    }

    private void LateUpdate()
    {
        LateUpdateGameLoop();
    }

    private void InjectDependencies()
    {
        playerCamera.SetPlayerInputManager(playerInputManager);
        playerLocomotionManager.SetPlayerInputManager(playerInputManager);
        playerLocomotionManager.SetPlayerCamera(playerCamera);
    }

    private void CallCustomAwake()
    {
        playerLocomotionManager.Setup();
    }

    private void StartGamePlaySystems()
    {
        playerLocomotionManager.Initialize();
        playerCamera.Initialize();
        enemyAIManager.Initialize();
    }

    private void FixedUpdateGameLoop()
    {

    }

    private void UpdateGameLoop()
    {
        playerInputManager.GameLoopUpdate();
        playerLocomotionManager.GameLoopUpdate();
        enemyAIManager.GameLoopUpdate();
    }

    private void LateUpdateGameLoop()
    {
        playerCamera.GameLoopLateUpdate();
    }

    private void ForDebug()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}
