using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    [Header("ã§í ê›íË")]
    public Transform player;
    public Animator animator;
    public float sightRange = 10f;
    public float fieldOfView = 90f;
    public LayerMask obstructionMask;

    [Header("èÑâÒê›íË")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 1f;

    [Header("í«ê’ê›íË")]
    public float chaseSpeed = 3.5f;
    public float loseSightAfterSeconds = 3f;

    private IEnemyState currentState;

    public void Initialize()
    {
        currentState = new EnemyPatrolState();
        currentState.Enter(this);
    }

    public void GameLoopUpdate()
    {
        currentState.Update(this);
    }

    public void SwitchState(IEnemyState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < fieldOfView / 2f)
        {
            if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, sightRange, ~0))
            {
                return hit.transform == player;
            }
        }

        return false;
    }
}
