using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    [Header("壁検出の設定")]
    [SerializeField] private float rayDistance = 0.6f;
    [SerializeField] private float wallAngleThreshold = 60f;
    [SerializeField] private float rayHeight = 0.5f;
    [SerializeField] private LayerMask wallLayerMask = ~0;

    private float width = 0.5f;
    private float depth = 0.5f;

    private void Awake()
    {
        AutoDetectColliderSize();
    }

    private void AutoDetectColliderSize()
    {
        var box = GetComponentInChildren<Collider>();

        if (box is CapsuleCollider capsule)
        {
            width = capsule.radius * 2f;
            depth = capsule.radius * 2f;
        }
        else if (box is BoxCollider boxCollider)
        {
            width = boxCollider.size.x * transform.localScale.x;
            depth = boxCollider.size.z * transform.localScale.z;
        }
        else if (box is SphereCollider sphere)
        {
            width = sphere.radius * 2f;
            depth = sphere.radius * 2f;
        }
        // 他のコライダーも必要ならここで追加
    }

    public bool IsWallInFront(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.001f) return false;

        moveDirection.y = 0f;
        moveDirection.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, moveDirection);
        Vector3 center = transform.position + Vector3.up * rayHeight;

        float halfW = width / 2f;
        float halfD = depth / 2f;

        Vector3[] rayOrigins = new Vector3[4]
        {
            center + right * halfW + moveDirection * halfD, // 右前
            center - right * halfW + moveDirection * halfD, // 左前
            center + right * halfW - moveDirection * halfD, // 右後
            center - right * halfW - moveDirection * halfD  // 左後
        };

        foreach (Vector3 origin in rayOrigins)
        {
            if (Physics.Raycast(origin, moveDirection, out RaycastHit hit, rayDistance, wallLayerMask))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                if (angle > wallAngleThreshold)
                {
                    Debug.DrawRay(origin, moveDirection * rayDistance, Color.red);
                    return true;
                }
                else
                {
                    Debug.DrawRay(origin, moveDirection * rayDistance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(origin, moveDirection * rayDistance, Color.green);
            }
        }

        return false;
    }
}
