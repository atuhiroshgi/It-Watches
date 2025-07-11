using UnityEngine;

public class PlayerLocomotionManager : EntityBase
{
    [Header("初期設定")]
    [SerializeField] private PlayerGroundCheck playerGroundCheck;
    [SerializeField] private PlayerWallCheck playerWallCheck;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("移動の設定")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private LayerMask wallLayers;

    [Header("ジャンプの設定")]
    [SerializeField] private float minJumpForce = 5.0f;
    [SerializeField] private float maxJumpForce = 12.0f;
    [SerializeField] private float maxJumpPressTime = 0.5f;


    private PlayerInputManager playerInputManager;
    private PlayerCamera playerCamera;

    private float jumpPressTime = 0f;
    private bool isMoving = false;
    private bool isJumping = false;

    public void Setup()
    {
        playerWallCheck.Setup();
        playerGroundCheck.Setup();
    }

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if(!gameStart) return;
        
        HandleMovement();
        HandleJump();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        if (playerInputManager == null || playerCamera == null) return;

        float inputH = playerInputManager.HorizontalInput;
        float inputV = playerInputManager.VerticalInput;

        isMoving = (new Vector2(inputH, inputV).sqrMagnitude > 0.001f);

        Transform camTransform = playerCamera.CameraObject.transform;
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * inputV + camRight * inputH).normalized;

        // 壁にぶつかりそうなら移動を無効にする
        if (playerWallCheck != null && playerWallCheck.IsWallInFront(moveDir))
        {
            moveDir = Vector3.zero;
        }

        Vector3 velocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);
        rb.linearVelocity = velocity;

        Vector3 lookDir = new Vector3(moveDir.x, 0f, moveDir.z);
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        // ジャンプボタン押し始め
        if (playerInputManager.JumpInputDown && playerGroundCheck.GetIsGrounded() && !isJumping)
        {
            jumpPressTime = 0f;
            isJumping = true;

            // 最小ジャンプ力を即座に適用
            if (rb != null)
            {
                Vector3 currentVelocity = rb.linearVelocity;
                currentVelocity.y = 0f;
                rb.linearVelocity = currentVelocity + Vector3.up * minJumpForce;
            }
        }

        // ジャンプボタンを押している間
        if (isJumping && playerInputManager.JumpInput)
        {
            jumpPressTime += Time.deltaTime;

            // 空中ジャンプ中にさらに上昇力を追加（最大時間まで）
            if (jumpPressTime < maxJumpPressTime)
            {
                float extraRatio = jumpPressTime / maxJumpPressTime;
                float extraJumpForce = (maxJumpForce - minJumpForce) * Time.deltaTime / maxJumpPressTime;

                if (rb != null)
                {
                    rb.AddForce(Vector3.up * extraJumpForce, ForceMode.VelocityChange);
                }
            }
        }

        // ジャンプ終了条件
        if (isJumping && (playerInputManager.JumpInputUp || jumpPressTime >= maxJumpPressTime))
        {
            isJumping = false;
        }
    }

    private void HandleAnimation()
    {
        if (animator == null) return;

        animator.SetBool("isMoving", isMoving);
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        if (this.playerInputManager != null) return;
        this.playerInputManager = playerInputManager;
    }

    public void SetPlayerCamera(PlayerCamera playerCamera)
    {
        if (this.playerCamera != null) return;
        this.playerCamera = playerCamera;
    }
}
