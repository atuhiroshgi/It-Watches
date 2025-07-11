using UnityEngine;

public class PlayerManager : EntityBase
{
    [Header("参照設定")]
    [SerializeField] private Animator animator;

    [Header("アニメーションの設定")]
    [SerializeField] private float surpriseCooldown = 5f;

    [Header("敵が味方に与えるダメージの設定")]
    [SerializeField] private float damagePerSecond = 5f;
    [SerializeField] private float maxHealth = 100f;

    private EnemyManager[] enemies;
    private float currentHealth;
    private float surpriseTimer = 0f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    public override void GameStart()
    {
        base.GameStart();
    }

    public void GameLoopUpdate()
    {
        if(!gameStart) return;

        bool beingSeen = false;

        foreach (var enemy in enemies)
        {
            if (enemy.CanSeePlayer())
            {
                beingSeen = true;
                break;
            }
        }

        if (beingSeen)
        {
            if (surpriseTimer <= 0f)
            {
                animator?.SetTrigger("Surprise");
                surpriseTimer = surpriseCooldown; // クールダウン開始
            }

            ApplyDamage(damagePerSecond * Time.deltaTime);
        }

        // クールタイマー減少
        if (surpriseTimer > 0f)
        {
            surpriseTimer -= Time.deltaTime;
        }
    }


    private void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        
    }

    public void SetEnemyManagers(EnemyManager[] enemies)
    {
        this.enemies = enemies;
    }
}
