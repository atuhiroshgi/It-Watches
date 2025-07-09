using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("接地判定設定")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private float checkDistance = 0.35f;
    [SerializeField] private float rayHeightOffset = 0.1f;

    private float width = 0.5f;
    private float depth = 0.5f;

    private void Awake()
    {
        AutoDetectColliderSize();
    }

    private void AutoDetectColliderSize()
    {
        var col = GetComponentInChildren<Collider>();

        if (col is CapsuleCollider capsule)
        {
            width = capsule.radius * 2f;
            depth = capsule.radius * 2f;
        }
        else if (col is BoxCollider box)
        {
            width = box.size.x * transform.localScale.x;
            depth = box.size.z * transform.localScale.z;
        }
        else if (col is SphereCollider sphere)
        {
            width = sphere.radius * 2f;
            depth = sphere.radius * 2f;
        }
    }

    private bool CheckGroundStatus()
    {
        Vector3 center = transform.position + Vector3.up * rayHeightOffset;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float halfW = width / 2f;
        float halfD = depth / 2f;

        // 四隅から Ray を飛ばす
        Vector3[] rayOrigins = new Vector3[4]
        {
            center + right * halfW + forward * halfD, // 右前
            center - right * halfW + forward * halfD, // 左前
            center + right * halfW - forward * halfD, // 右後
            center - right * halfW - forward * halfD  // 左後
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

    public bool GetIsGrounded() => CheckGroundStatus();
}
