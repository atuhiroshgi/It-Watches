using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputManager : EntityBase
{
    private float verticalInput;
    private float horizontalInput;
    private float cameraVerticalInput;
    private float cameraHorizontalInput;
    private bool jumpInput;
    private bool jumpInputDown;
    private bool jumpInputUp;
    private bool attackInput;

    public float VerticalInput => verticalInput;
    public float HorizontalInput => horizontalInput;
    public float CameraVerticalInput => cameraVerticalInput;
    public float CameraHorizontalInput => cameraHorizontalInput;
    public bool JumpInput => jumpInput;
    public bool JumpInputDown => jumpInputDown;
    public bool JumpInputUp => jumpInputUp;
    public bool AttackInput => attackInput;

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if (!gameStart) return;
        
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleJumpInput();
        HandleAttackInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = Input.GetAxis("Mouse Y");
        cameraHorizontalInput = Input.GetAxis("Mouse X");
    }

    private void HandleJumpInput()
    {
        jumpInput = Input.GetKey(KeyCode.Space);
        jumpInputDown = Input.GetKeyDown(KeyCode.Space);
        jumpInputUp = Input.GetKeyUp(KeyCode.Space);
    }

    private void HandleAttackInput()
    {
        attackInput = Input.GetMouseButton(0);
    }
}
