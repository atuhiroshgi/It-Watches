using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;
    [SerializeField] private Transform cameraPivotTransform;
    [SerializeField] private Transform playerTransform;

    [Header("�J�����ݒ�")]
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

    [Header("�f�o�b�O�p(���l�m�F)")]
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;

    private PlayerInputManager playerInputManager;
    private Vector3 cameraVelocity;
    private float currentCameraDistance;

    public Camera CameraObject => cameraObject;

    public void Initialize()
    {
        currentCameraDistance = Mathf.Clamp(defaultCameraDistance, minCameraDistance, maxCameraDistance);
        // �����p�x��ݒ�
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
        // �v���C���[�̈ʒu�ɃJ����Pivot���X���[�Y�ɒǏ]
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

        // ��]�p�x�̍X�V
        leftAndRightLookAngle += horizontal * leftAndRightRotationSpeed;
        upAndDownLookAngle -= vertical * upAndDownRotationSpeed;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        // �J�����̍��E��]
        transform.rotation = Quaternion.Euler(0, leftAndRightLookAngle, 0);

        // �s�{�b�g�̏㉺��]
        cameraPivotTransform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0, 0);
    }

    private void HandleCollisions()
    {
        // �J�����̗��z�ʒu�i�s�{�b�g������currentCameraDistance�j
        Vector3 desiredCameraPos = cameraPivotTransform.position - cameraPivotTransform.forward * currentCameraDistance;

        // �R���W��������
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

        // �J�������X���[�Y�Ɉړ�
        Vector3 finalCameraPos = cameraPivotTransform.position - cameraPivotTransform.forward * targetDistance;
        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, finalCameraPos, 0.5f);

        // �J���������Pivot������
        cameraObject.transform.LookAt(cameraPivotTransform.position);
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        if (playerInputManager != null) return;
        this.playerInputManager = playerInputManager;
    }
}
