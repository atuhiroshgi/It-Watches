using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("�A�j���[�V�����̐ݒ�")]
    [SerializeField] private float surpriseCooldown = 5f;

    [Header("�G�������ɗ^����_���[�W�̐ݒ�")]
    [SerializeField] private float damagePerSecond = 5f;
    [SerializeField] private float maxHealth = 100f;

    private EnemyManager[] enemies;
    private Animator animator;
    private float currentHealth;
    private float surpriseTimer = 0f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public void Setup()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    public void GameLoopUpdate()
    {
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
                surpriseTimer = surpriseCooldown; // �N�[���_�E���J�n
            }

            ApplyDamage(damagePerSecond * Time.deltaTime);
        }

        // �N�[���^�C�}�[����
        if (surpriseTimer > 0f)
        {
            surpriseTimer -= Time.deltaTime;
        }
    }


    private void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"���΂��̗̑�: {currentHealth}");

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
