using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyAIManager enemy);
    void Update(EnemyAIManager enemy);
    void Exit(EnemyAIManager enemy);
}
