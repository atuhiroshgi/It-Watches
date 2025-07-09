using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;
    [SerializeField] private Transform cameraPivotTransform;
    [SerializeField] private Transform playerTransform;

    [Header("カメラ設定")]
    [SerializeField] private float cameraSmoothSpeed = 0.2f;
    [SerializeField] private float leftAndRightRotationSpeed = 3.0f;
    [SerializeField] private float upAndDownRotationSpeed = 3.0f;
    [SerializeField] private float minimumPivot = -30f;
    [SerializeField] private float maximumPivot = 60f;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayers;
    [SerializeField] private float defaultCameraDistance = 4.0f;
    [SerializeField] private float minCameraDistance = 1.0f;
    [SerializeField] private float maxCameraDistance = 6.0f;

    [Header("デバッグ用(数値確認)")]
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;

    private PlayerInputManager playerInputManager;
    private Vector3 cameraVelocity;
    private float currentCameraDistance;

    public Camera CameraObject => cameraObject;

    public void Initialize()
    {
        currentCameraDistance = Mathf.Clamp(defaultCameraDistance, minCameraDistance, maxCameraDistance);
        // 初期角度を設定
        leftAndRightLookAngle = transform.eulerAngles.y;
        upAndDownLookAngle = transform.eulerAngles.x;
    }

    public void GameLoopLateUpdate()
    {
        HandleAllCameraActions();
    }

    public void HandleAllCameraActions()
    {
        if (playerTransform == null || playerInputManager == null) return;

        HandleFollowTarget();
        HandleRotations();
        HandleCollisions();
    }

    private void HandleFollowTarget()
    {
        // プレイヤーの位置にカメラPivotをスムーズに追従
        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position,
            playerTransform.position,
            ref cameraVelocity,
            cameraSmoothSpeed
        );
        transform.position = targetPosition;
    }

    private void HandleRotations()
    {
        float horizontal = playerInputManager.CameraHorizontalInput;
        float vertical = playerInputManager.CameraVerticalInput;

        // 回転角度の更新
        leftAndRightLookAngle += horizontal * leftAndRightRotationSpeed;
        upAndDownLookAngle -= vertical * upAndDownRotationSpeed;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        // カメラの左右回転
        transform.rotation = Quaternion.Euler(0, leftAndRightLookAngle, 0);

        // ピボットの上下回転
        cameraPivotTransform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0, 0);
    }

    private void HandleCollisions()
    {
        // カメラの理想位置（ピボットから後方currentCameraDistance）
        Vector3 desiredCameraPos = cameraPivotTransform.position - cameraPivotTransform.forward * currentCameraDistance;

        // コリジョン判定
        RaycastHit hit;
        float targetDistance = currentCameraDistance;
        if (Physics.SphereCast(
            cameraPivotTransform.position,
            cameraCollisionRadius,
            -cameraPivotTransform.forward,
            out hit,
            currentCameraDistance,
            collideWithLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance - cameraCollisionRadius, minCameraDistance, currentCameraDistance);
        }

        // カメラをスムーズに移動
        Vector3 finalCameraPos = cameraPivotTransform.position - cameraPivotTransform.forward * targetDistance;
        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, finalCameraPos, 0.5f);

        // カメラが常にPivotを見る
        cameraObject.transform.LookAt(cameraPivotTransform.position);
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        if (playerInputManager != null) return;
        this.playerInputManager = playerInputManager;
    }
}
