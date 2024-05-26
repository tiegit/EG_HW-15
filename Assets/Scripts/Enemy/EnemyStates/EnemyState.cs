using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyState : MonoBehaviour
{
    protected const string Crouch = "Crouch";
    protected const string Walk = "Walk";
    protected const string Hit = "Hit";

    protected EnemyStateMachine _stateMachine;

    protected NavMeshAgent _navMeshAgent;
    protected EnemyHealth _enemyHealth;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();

    }

    protected void BaseInitialize(EnemyStateMachine stateMachine) => _stateMachine = stateMachine;

    public virtual void Enter()
    {
        _enemyHealth.EnemyHitted += OnEnemyHitted;
        _enemyHealth.EnemyDie += OnEnemyDie;
    }

    public virtual void Process()
    {
    }

    public virtual void Exit()
    {
        _enemyHealth.EnemyHitted -= OnEnemyHitted;
        _enemyHealth.EnemyDie -= OnEnemyDie;
    }

    private void OnEnemyHitted() => _stateMachine.StartHittedState();

    private void OnEnemyDie(EnemyBodyPart part, Vector3 vector)
    {
        _stateMachine.StartDieState();
    }

    private void OnDestroy()
    {
        _enemyHealth.EnemyHitted -= OnEnemyHitted;
        _enemyHealth.EnemyDie -= OnEnemyDie;
    }
}
