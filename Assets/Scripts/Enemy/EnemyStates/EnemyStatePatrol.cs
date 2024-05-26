using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemyStatePatrol : EnemyState
{
    private Transform _aim;
    private Transform _aimDefaultPosition;

    private Animator _animator;
    private PatrolManager _patrolManager;
    private Transform _playerCenter;

    private float _viewingDistance;
    private float _viewingAngle;
    private LayerMask _layerMask;

    protected Transform _otherEnemyCenter;

    public void Initialize(EnemyStateMachine stateMachine, Animator animator, PatrolManager patrolManager, Transform playerCenter, float viewingDistance, float viewingAngle, LayerMask layerMask)
    {
        BaseInitialize(stateMachine);

        _animator = animator;
        _patrolManager = patrolManager;
        _playerCenter = playerCenter;
        _viewingDistance = viewingDistance;
        _viewingAngle = viewingAngle;
        _layerMask = layerMask;

        _aim = GetComponentInChildren<Aim>().GetComponent<Transform>();
        _aimDefaultPosition = GetComponentInChildren<AimDefaultPosition>().GetComponent<Transform>();
    }

    public override void Enter()
    {
        base.Enter();

        _navMeshAgent.isStopped = false;
        _animator.SetBool(Walk, true);

        SetTargetPoint();
    }

    public override void Process()
    {
        _aim.position = Vector3.Lerp(_aim.position, _aimDefaultPosition.position, Time.deltaTime * 4f);

        if (_navMeshAgent.remainingDistance < 0.5f)
        {
            SetTargetPoint();
        }

        _otherEnemyCenter = FindClosestEnemy();

        if (_otherEnemyCenter != null)
        {
            bool canSeeEnemy = SearchUtility.SearchInSector(transform.position + Vector3.up * 1.5f, transform.forward, _otherEnemyCenter.position, _viewingAngle, _viewingDistance, _layerMask);

            if (canSeeEnemy)
            {
                _stateMachine.StartAttackEnemyState(_otherEnemyCenter);
            }
        }

        bool canSeePlayer = SearchUtility.SearchInSector(transform.position + Vector3.up * 1.5f, transform.forward, _playerCenter.position, _viewingAngle, _viewingDistance, _layerMask);

        if (canSeePlayer)
        {
            _stateMachine.StartFollowState();
        }
    }

    private Transform FindClosestEnemy()
    {
        var allUnits = FindObjectsOfType<Enemy>().Where(i => i.EnemyType != GetComponent<Enemy>().EnemyType).ToList();

        float minDistance = Mathf.Infinity;

        Transform closestEnemy = null;

        for (int i = 0; i < allUnits.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = allUnits[1].EnemyCenter.transform;
            }
        }

        return closestEnemy;
    }

    private void SetTargetPoint()
    {
        TargetPoint targetPoint = _patrolManager.GetRandomTarget();
        _navMeshAgent.SetDestination(targetPoint.transform.position);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0.1f, 0.3f, 0.2f);
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0, -_viewingAngle, 0) * transform.forward, _viewingAngle * 2f, _viewingDistance);
    }
#endif
}
