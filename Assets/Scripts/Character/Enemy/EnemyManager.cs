using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyManager : MonoBehaviour
{
    [Header("�p�����[�^�ݒ�")]
    [SerializeField] private float downDuration = 10f;
    [SerializeField] private float damageCooldown = 0.5f;

    private EnemyAIManager enemyAIManager;
    private Animator animator;
    private Transform playerTransform;
    private int damageCount = 0;
    private float lastDamageTime = -Mathf.Infinity;
    private bool isDown = false;

    public bool IsDown => isDown;

    public void Setup()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAIManager = GetComponent<EnemyAIManager>();

        enemyAIManager.SetEnemyAnimator(animator);
        enemyAIManager.SetPlayerTransform(playerTransform);
        enemyAIManager.SetIsDownFunc(() => IsDown);
    }

    public void GameLoopUpdate()
    {
        enemyAIManager.GameLoopUpdate();
    }

    public bool CanSeePlayer()
    {
        return enemyAIManager != null && enemyAIManager.CanSeePlayer();
    }

    public void TakeDamage()
    {
        if (isDown) return;

        if (Time.time < lastDamageTime + damageCooldown)
        {
            // �܂��_���[�W�N�[���^�C�����Ȃ̂Ŗ���
            return;
        }

        lastDamageTime = Time.time;
        damageCount++;

        if (damageCount == 1)
        {
            animator.SetTrigger("Damage");
        }
        else if (damageCount >= 2)
        {
            isDown = true;
            animator.SetBool("isDown", true);

            _ = DownRecoveryAsync();
        }
    }

    private async UniTaskVoid DownRecoveryAsync()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(downDuration));
        animator.SetBool("isDown", false);
        isDown = false;
        damageCount = 0;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
}
