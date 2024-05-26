using UnityEngine;

public class EnemyStateDie : EnemyState
{
    private EnemyCreator _enemyCreator;

    public void Initialize(EnemyStateMachine stateMachine, EnemyCreator enemyCreator)
    {
        BaseInitialize(stateMachine);

        _enemyCreator = enemyCreator;
    }

    public override void Enter()
    {
        base.Enter();

        _navMeshAgent.isStopped = true;

        Invoke(nameof(Respawn), 3f);
    }

    private void Respawn()
    {
        _enemyCreator.CreateEnemy(GetComponent<Enemy>().EnemyType);
        Destroy(gameObject);
    }
}
