using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyManager : MonoBehaviour
{
    [Header("ÉpÉâÉÅÅ[É^ê›íË")]
    [SerializeField] private float downDuration = 10f;

    private EnemyAIManager enemyAIManager;
    private Animator animator;
    private Transform playerTransform;
    private int damageCount = 0;
    private bool isDown = false;

    public void Setup()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAIManager = GetComponent<EnemyAIManager>();

        enemyAIManager.SetEnemyAnimator(animator);
        enemyAIManager.SetPlayerTransform(playerTransform);
    }

    public void GameLoopUpdate()
    {
        enemyAIManager.GameLoopUpdate();
    }

    public bool CanSeePlayer()
    {
        return enemyAIManager != null && enemyAIManager.CanSeePlayer();
    }

    public async void TakeDamage()
    {
        if (isDown) return;

        damageCount++;

        if (damageCount == 1)
        {
            animator.SetTrigger("Damage");
        }
        else if (damageCount >= 2)
        {
            isDown = true;
            animator.SetBool("isDown", true);

            await UniTask.Delay(System.TimeSpan.FromSeconds(downDuration));

            animator.SetBool("isDown", false);
            isDown = false;
            damageCount = 0;
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
}
