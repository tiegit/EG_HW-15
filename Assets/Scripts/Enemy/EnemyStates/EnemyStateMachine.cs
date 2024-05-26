using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rig _rig;

    [SerializeField, Space(10)] private EnemyStatePatrol _enemyStatePatrol;
    [SerializeField] private EnemyStateFollow _enemyStateFollow;
    [SerializeField] private EnemyStateHitted _enemyStateHitted;
    [SerializeField] private EnemyStateDie _enemyStateDie;

    public EnemyState CurrentEnemyState { get; private set; }

    public void Initialize(EnemyCreator enemyCreator, PatrolManager patrolManager, Transform playerCenter, float viewingDistance, float viewingAngle, LayerMask layerMask)
    {
        _enemyStatePatrol.Initialize(this, _animator, patrolManager, playerCenter, viewingDistance, viewingAngle, layerMask);

        _enemyStateFollow.Initialize(this, _animator, playerCenter, viewingDistance, viewingAngle, layerMask);

        _enemyStateHitted.Initialize(this, _animator, _rig);

        _enemyStateDie.Initialize(this, enemyCreator);
    }

    private void Start() => SetState(_enemyStatePatrol);

    private void Update()
    {
        if (CurrentEnemyState)
        {
            CurrentEnemyState.Process();
        }
    }

    public void StartPatrolState() => SetState(_enemyStatePatrol);

    public void StartFollowState() => SetState(_enemyStateFollow);

    public void StartHittedState() => SetState(_enemyStateHitted);

    public void StartDieState() => SetState(_enemyStateDie);

    private void SetState(EnemyState enemyState)
    {
        //if (_currentEnemyState && _currentEnemyState != enemyState)// эта строчка не правильная, выходить из состояний нужно всегда, иначе отписка событий происходить не будет

        if (CurrentEnemyState is EnemyStateDie)
        {
            return;
        }

        if (CurrentEnemyState)
        {
            CurrentEnemyState.Exit();
        }

        CurrentEnemyState = enemyState;
        CurrentEnemyState.Enter();

        //Debug.Log(CurrentEnemyState);
    }
}
