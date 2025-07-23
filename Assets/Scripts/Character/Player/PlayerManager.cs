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

    [Header("チェックポイントのレイヤー")]
    [SerializeField] private LayerMask checkPointLayer;

    private EnemyManager[] enemies;
    private float currentHealth;
    private float surpriseTimer = 0f;
    private bool onCheckPoint = false;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool OnCheckPoint => onCheckPoint;

    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    public override void GameStart()
    {
        base.GameStart();
    }

    public override void GameEnd()
    {
        base.GameEnd();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & checkPointLayer) != 0)
        {
            onCheckPoint = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & checkPointLayer) != 0)
        {
            onCheckPoint = false;
        }
    }
    public void SetEnemyManagers(EnemyManager[] enemies)
    {
        this.enemies = enemies;
    }
}
