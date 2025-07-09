using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private float lostSightTimer = 0f;

    public void Enter(EnemyAIManager enemy)
    {
        enemy.animator.SetBool("isChasing", true);
    }

    public void Update(EnemyAIManager enemy)
    {
        if (enemy.CanSeePlayer())
        {
            lostSightTimer = 0f;
        }
        else
        {
            lostSightTimer += Time.deltaTime;
            if (lostSightTimer >= enemy.loseSightAfterSeconds)
            {
                enemy.SwitchState(new EnemyPatrolState());
                return;
            }
        }

        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        dir.y = 0f;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.player.position, enemy.chaseSpeed * Time.deltaTime);
        if (dir != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void Exit(EnemyAIManager enemy)
    {
        enemy.animator.SetBool("isChasing", false);
    }
}
