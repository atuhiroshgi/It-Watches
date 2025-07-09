using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private float cameraVerticalInput;
    private float cameraHorizontalInput;
    private bool jumpInput;
    private bool jumpInputDown;
    private bool jumpInputUp;

    public float VerticalInput => verticalInput;
    public float HorizontalInput => horizontalInput;
    public float CameraVerticalInput => cameraVerticalInput;
    public float CameraHorizontalInput => cameraHorizontalInput;
    public bool JumpInput => jumpInput;
    public bool JumpInputDown => jumpInputDown;
    public bool JumpInputUp => jumpInputUp;

    public void GameLoopUpdate()
    {
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleJumpInput();
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
}
