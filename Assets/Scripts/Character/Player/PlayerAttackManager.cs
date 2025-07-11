using UnityEngine;

public class PlayerAttackManager : EntityBase
{
    [Header("参照設定")]
    [SerializeField] private Animator animator;

    [Header("攻撃の設定")]
    [SerializeField] private LayerMask attackHitMask;
    [SerializeField] private float attackCoolTime = 0.7f;
    [SerializeField] private float attackDuration = 0.4f;
    [SerializeField] private float attackRange = 3f;

    [Header("照準の調整(画面の高さに対する割合)")]
    [SerializeField, Range(0f, 0.5f)] private float crosshairOffsetRatio = 0.1f;

    [Header("クロスヘア ヒステリシス(OFF判定までの連続フレーム数)")]
    [SerializeField, Range(1, 10)] private int hysteresisFrames = 5;

    private PlayerInputManager playerInputManager;
    private PlayerCamera playerCamera;
    private CrosshairManager crosshairManager;
    private EnemyManager currentTargetEnemy;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttack = false;

    // ヒステリシス用カウンターと安定状態
    private int offCounter = 0;
    private bool stableCanHit = false;

    public bool IsAttack => isAttack;

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if(!gameStart) return;

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

        UpdateCrosshairAndTarget();

        if (isAttack && currentTargetEnemy != null)
        {
            currentTargetEnemy.TakeDamage();
        }
    }

    private void UpdateCrosshairAndTarget()
    {
        float verticalOffset = Screen.height * crosshairOffsetRatio;
        Vector3 screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f + verticalOffset);
        Ray ray = playerCamera.CameraObject.ScreenPointToRay(screenPoint);

        bool currentCanHit = false;
        EnemyManager newTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, attackHitMask))
        {
            EnemyManager enemy = hit.collider.GetComponent<EnemyManager>() ?? hit.collider.GetComponentInParent<EnemyManager>();
            if (enemy != null && enemy.CanSeePlayer())
            {
                newTarget = enemy;
                currentCanHit = true;
            }
        }

        // ヒステリシス処理
        if (currentCanHit)
        {
            offCounter = 0;
            stableCanHit = true;
        }
        else
        {
            offCounter++;
            if (offCounter >= hysteresisFrames)
            {
                stableCanHit = false;
                currentTargetEnemy = null;
            }
        }

        if (stableCanHit)
        {
            currentTargetEnemy = newTarget;
        }

        crosshairManager?.SetActiveState(stableCanHit);
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        this.playerInputManager = playerInputManager;
    }

    public void SetPlayerCamera(PlayerCamera playerCamera)
    {
        this.playerCamera = playerCamera;
    }

    public void SetCrosshairManager(CrosshairManager crosshairManager)
    {
        this.crosshairManager = crosshairManager;
    }
}
