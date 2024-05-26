using System.Collections;
using UnityEngine;

public class EnemyStateFollow : EnemyState
{
    private EnemyShooting _enemyShooting;
    private Transform _aim;

    private Animator _animator;
    private Transform _playerCenter;
    private float _viewingDistance;
    private float _viewingAngle;
    private LayerMask _wallMask;

    private Coroutine _behaviourCoroutine;
    private float _lostPlayerTimer;

    public void Initialize(EnemyStateMachine stateMachine, Animator animator, Transform playerCenter, float viewingDistance, float viewingAngle, LayerMask wallMask)
    {
        BaseInitialize(stateMachine);

        _animator = animator;
        _playerCenter = playerCenter;
        _viewingDistance = viewingDistance;
        _viewingAngle = viewingAngle;
        _wallMask = wallMask;

        _enemyShooting = GetComponentInChildren<EnemyShooting>();
        _aim = GetComponentInChildren<Aim>().GetComponent<Transform>();
    }

    public override void Enter()
    {
        base.Enter();

        _lostPlayerTimer = 0;
        _behaviourCoroutine = StartCoroutine(Behaviour());

        //_navMeshAgent.isStopped = false;
        //_animator.SetBool(Walk, true);
        //_animator.SetBool(Crouch, true);
    }

    public override void Process()
    {
        _lostPlayerTimer += Time.deltaTime;

        bool seePlayer = SearchUtility.SearchInSector(transform.position, transform.forward, _playerCenter.position, _viewingAngle, _viewingDistance, _wallMask);

        if (seePlayer)
        {
            _lostPlayerTimer = 0;
        }

        if (_lostPlayerTimer > 4f)
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

                _navMeshAgent.SetDestination(_playerCenter.transform.position);

                SetAimPosition();

                yield return null;
            }

            yield return null;
        }
    }

    private float GetRandomTime() => Random.Range(0.5f, 1.5f);

    private void SetAimPosition() => _aim.position = Vector3.Lerp(_aim.position, _playerCenter.position, Time.deltaTime * 5f);
}