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

    public float GetVerticalInput() => verticalInput;
    public float GetHorizontalInput() => horizontalInput;
    public float GetCameraVerticalInput() => cameraVerticalInput;
    public float GetCameraHorizontalInput() => cameraHorizontalInput;
    public bool GetJumpInput() => jumpInput;
    public bool GetJumpInputDown() => jumpInputDown;
    public bool GetJumpInputUp() => jumpInputUp;

}
