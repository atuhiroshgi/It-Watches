using UnityEngine;

public class EnemyAIManager : EntityBase
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

    [Header("視線設定")]
    [SerializeField] private float eyeHeight = 1.0f;
    [SerializeField] private float playerAimHeight = 0.9f;

    private Animator animator;
    private Transform playerTransform;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private float lookAtTimer = 0f;
    private bool isWaiting = false;
    private bool isLookingAtPlayer = false;

    private System.Func<bool> isDownFunc;

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if(isDownFunc != null && isDownFunc())
        {
            animator?.SetBool("isWalking", false);
            return;
        }

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
        if (isDownFunc != null && isDownFunc()) return false;
        if (playerTransform == null) return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 target = playerTransform.position + Vector3.up * playerAimHeight;
        Vector3 direction = (target - origin).normalized;

        float angle = Vector3.Angle(transform.forward, direction);
        if (angle < fieldOfView / 2f)
        {
            Debug.DrawRay(origin, direction * sightRange, Color.green);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, sightRange, ~obstructionMask))
            {
                if (hit.transform == playerTransform || hit.transform.root == playerTransform)
                {
                    return true;
                }
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

    public void SetIsDownFunc(System.Func<bool> func)
    {
        isDownFunc = func;
    }
}
