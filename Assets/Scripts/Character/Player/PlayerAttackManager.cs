using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private LayerMask attackHitMask;
    [SerializeField] private float attackCoolTime = 0.7f;
    [SerializeField] private float attackDuration = 0.4f;
    [SerializeField] private float attackRange = 3f;

    private PlayerInputManager playerInputManager;
    private PlayerCamera playerCamera;
    private EnemyManager currentTargetEnemy;
    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttack = false;

    public bool IsAttack => isAttack;

    public void Setup()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void GameLoopUpdate()
    {
        if (playerInputManager.AttackInput && Time.time >= lastAttackTime + attackCoolTime)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
            isAttack = true;
        }

        if (isAttack && Time.time >= lastAttackTime + attackDuration)
        {
            isAttack = false;
            currentTargetEnemy = null;
        }

        if (isAttack)
        {
            CheckAttackHit();
        }
        else
        {
            currentTargetEnemy = null;
        }
    }

    private void CheckAttackHit()
    {
        Ray ray = playerCamera.CameraObject.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, attackHitMask))
        {
            EnemyManager enemy = hit.collider.GetComponent<EnemyManager>() ?? hit.collider.GetComponentInParent<EnemyManager>();
            if (enemy != null && enemy.CanSeePlayer())
            {
                if (enemy != currentTargetEnemy)
                {
                    currentTargetEnemy = enemy;
                    enemy.TakeDamage();
                }
            }
            else
            {
                currentTargetEnemy = null;
            }
        }
        else
        {
            currentTargetEnemy = null;
        }
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        this.playerInputManager = playerInputManager;
    }

    public void SetPlayerCamera(PlayerCamera playerCamera)
    {
        this.playerCamera = playerCamera;
    }
}
