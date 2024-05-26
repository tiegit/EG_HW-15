using System.Collections;
using UnityEngine;

public class EnemyStateAttackEnemy : EnemyState
{
    private EnemyShooting _enemyShooting;
    private Transform _aim;

    private Animator _animator;
    private float _viewingDistance;
    private float _viewingAngle;
    private LayerMask _wallMask;

    private Transform _otherEnemyCenter;

    private Coroutine _behaviourCoroutine;
    private float _lostOtherEnemyTimer;


    public void Initialize(EnemyStateMachine stateMachine, Animator animator, float viewingDistance, float viewingAngle, LayerMask wallMask)
    {
        BaseInitialize(stateMachine);

        _animator = animator;
        _viewingDistance = viewingDistance;
        _viewingAngle = viewingAngle;
        _wallMask = wallMask;

        _enemyShooting = GetComponentInChildren<EnemyShooting>();
        _aim = GetComponentInChildren<Aim>().GetComponent<Transform>();
    }

    public void SetTarget(Transform otherEnemyCenter)
    {
        _otherEnemyCenter = otherEnemyCenter;
    }

    public override void Enter()
    {
        base.Enter();

        _lostOtherEnemyTimer = 0;
        _behaviourCoroutine = StartCoroutine(Behaviour());
    }

    public override void Process()
    {
        if (_otherEnemyCenter == null)
        {
            _stateMachine.StartPatrolState();

            return;
        }

        _lostOtherEnemyTimer += Time.deltaTime;

        bool seeOtherEnemy = SearchUtility.SearchInSector(transform.position, transform.forward, _otherEnemyCenter.position, _viewingAngle, _viewingDistance, _wallMask);

        if (seeOtherEnemy)
        {
            _lostOtherEnemyTimer = 0;
        }

        if (_lostOtherEnemyTimer > 4f)
        {
            _stateMachine.StartPatrolState();
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (_behaviourCoroutine != null)
        {
            StopCoroutine(_behaviourCoroutine);
        }
    }

    private IEnumerator Behaviour()
    {
        while (true)
        {
            _navMeshAgent.isStopped = true;
            _animator.SetBool(Walk, false);
            _animator.SetBool(Crouch, false);

            float timer = GetRandomTime();

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                SetAimPosition();

                _enemyShooting.Process();

                yield return null;
            }

            _navMeshAgent.isStopped = false;
            _animator.SetBool(Walk, true);
            _animator.SetBool(Crouch, true);

            timer = GetRandomTime();

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                _navMeshAgent.SetDestination(_otherEnemyCenter.transform.position);

                SetAimPosition();

                yield return null;
            }

            yield return null;
        }
    }

    private float GetRandomTime() => Random.Range(0.5f, 1.5f);

    private void SetAimPosition() => _aim.position = Vector3.Lerp(_aim.position, _otherEnemyCenter.position, Time.deltaTime * 5f);
}
