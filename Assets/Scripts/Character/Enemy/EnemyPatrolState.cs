using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private int patrolIndex = 0;
    private float waitTimer = 0f;

    public void Enter(EnemyAIManager enemy)
    {
        enemy.animator.SetBool("isWalking", true);
    }

    public void Update(EnemyAIManager enemy)
    {
        if (enemy.CanSeePlayer())
        {
            enemy.SwitchState(new EnemyChaseState());
            return;
        }

        if (enemy.patrolPoints.Length == 0) return;

        Transform target = enemy.patrolPoints[patrolIndex];
        Vector3 moveDir = (target.position - enemy.transform.position).normalized;
        moveDir.y = 0;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.position, enemy.patrolSpeed * Time.deltaTime);
        enemy.transform.rotation = Quaternion.LookRotation(moveDir);

        if (Vector3.Distance(enemy.transform.position, target.position) < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= enemy.waitTimeAtPoint)
            {
                waitTimer = 0f;
                patrolIndex = (patrolIndex + 1) % enemy.patrolPoints.Length;
            }
        }
    }

    public void Exit(EnemyAIManager enemy)
    {
        enemy.animator.SetBool("isWalking", false);
    }
}
