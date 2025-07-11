using UnityEngine;

public class PlayerGroundCheck : EntityBase
{
    [Header("�ڒn����ݒ�")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private float checkDistance = 0.35f;
    [SerializeField] private float rayHeightOffset = 0.1f;

    private float width = 0.5f;
    private float depth = 0.5f;

    public bool GetIsGrounded() => CheckGroundStatus();

    public void Setup()
    {
        AutoDetectColliderSize();
    }

    private void AutoDetectColliderSize()
    {
        width = col.radius * 2f;
        depth = col.radius * 2f;
    }

    private bool CheckGroundStatus()
    {
        Vector3 center = transform.position + Vector3.up * rayHeightOffset;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float halfW = width / 2f;
        float halfD = depth / 2f;

        // �l������ Ray ���΂�
        Vector3[] rayOrigins = new Vector3[4]
        {
            center + right * halfW + forward * halfD, // �E�O
            center - right * halfW + forward * halfD, // ���O
            center + right * halfW - forward * halfD, // �E��
            center - right * halfW - forward * halfD  // ����
        };

        foreach (var origin in rayOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, checkDistance, groundLayers))
            {
                Debug.DrawRay(origin, Vector3.down * checkDistance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(origin, Vector3.down * checkDistance, Color.red);
            }
        }

        return false;
    }

}
