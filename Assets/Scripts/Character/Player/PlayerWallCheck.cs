using UnityEngine;

public class PlayerWallCheck : EntityBase
{
    [Header("壁検出の設定")]
    [SerializeField] private LayerMask wallLayerMask = ~0;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private float rayDistance = 0.6f;
    [SerializeField] private float wallAngleThreshold = 60f;
    [SerializeField] private float rayHeight = 0.5f;

    private float width = 0.5f;
    private float depth = 0.5f;

    public void Setup()
    {
        AutoDetectColliderSize();
    }

    private void AutoDetectColliderSize()
    {
        width = col.radius * 2f;
        depth = col.radius * 2f;
    }

    public bool IsWallInFront(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.001f) return false;

        moveDirection.y = 0f;
        moveDirection.Normalize();

        // Capsuleの上下端の位置
        Vector3 point1 = transform.position + Vector3.up * (col.radius);
        Vector3 point2 = transform.position + Vector3.up * (col.height - col.radius);

        // Capsuleのキャスト
        if (Physics.CapsuleCast(point1, point2, col.radius * 0.95f, moveDirection, out RaycastHit hit, rayDistance, wallLayerMask))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle > wallAngleThreshold)
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDirection * rayDistance, Color.red);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDirection * rayDistance, Color.yellow);
            }
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDirection * rayDistance, Color.green);
        }

        return false;
    }


}
