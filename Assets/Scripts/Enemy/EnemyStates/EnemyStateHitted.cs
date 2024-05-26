using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyStateHitted : EnemyState
{    
    [SerializeField] private AnimationCurve _rigWaightCurve;

    private Animator _animator;
    private Rig _rig;

    public void Initialize(EnemyStateMachine stateMachine, Animator animator, Rig rig)
    {
        BaseInitialize(stateMachine);

        _animator = animator;
        _rig = rig;
    }

    public override void Enter()
    {
        base.Enter();

        StartCoroutine(HitProcess());
    }
    
    public override void Exit()
    {
        base.Exit();

        _rig.weight = 1;
        StopCoroutine(HitProcess());
    }

    private IEnumerator HitProcess()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetTrigger(Hit);

        for (float t = 0; t < 1f; t+= Time.deltaTime / 0.6f)
        {
            _rig.weight = _rigWaightCurve.Evaluate(t);
            yield return null;
        }

        _rig.weight = 1;
        _stateMachine.StartFollowState();
    }
}