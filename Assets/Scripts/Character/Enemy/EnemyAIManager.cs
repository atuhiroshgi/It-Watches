using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    [Header("共通設定")]
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float fieldOfView = 90f;

    [Header("巡回設定")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waitTimeAtPoint = 1f;

    [Header("注視設定")]
    [SerializeField] private float lookAtPlayerDuration = 5f;

    private Animator animator;
    private Transform playerTransform;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private bool isLookingAtPlayer = false;
    private float lookAtTimer = 0f;

    public void GameLoopUpdate()
    {
        if (CanSeePlayer())
        {
            // プレイヤー視認中は注視状態へ
            isLookingAtPlayer = true;
            lookAtTimer = 0f;
            LookAtPlayer();
            animator?.SetBool("isWalking", false);
        }
        else if (isLookingAtPlayer)
        {
            // 視界から外れた後も一定時間注視続行
            lookAtTimer += Time.deltaTime;
            if (lookAtTimer >= lookAtPlayerDuration)
            {
                isLookingAtPlayer = false;
            }
            else
            {
                LookAtPlayer();
                animator?.SetBool("isWalking", false);
            }
        }
        else
        {
            // 巡回処理
            HandlePatrol();
        }
    }

    private void HandlePatrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;

        // 向く
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);

        // 到着判定と待機処理
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = 0f;
                animator?.SetBool("isWalking", false);
            }

            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                isWaiting = false;
            }
        }
        else
        {
            animator?.SetBool("isWalking", true);
        }
    }

    private void LookAtPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < fieldOfView / 2f)
        {
            Vector3 origin = transform.position + Vector3.up;
            Vector3 target = playerTransform.position + Vector3.up;

            if (Physics.Raycast(origin, (target - origin).normalized, out RaycastHit hit, sightRange, ~obstructionMask))
            {
                return hit.transform == playerTransform;
            }
        }

        return false;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void SetEnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }
}
