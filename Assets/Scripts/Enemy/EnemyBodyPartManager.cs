using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyBodyPartManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private EnemyHealth _enemyHealth;

    private EnemyBodyPart[] _bodyParts;

    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _enemyHealth.EnemyDie += OnEnemyDie;

        _bodyParts = GetComponentsInChildren<EnemyBodyPart>();
        MakeKinematic();
    }

    public void Hit(float damage, EnemyBodyPart hittedPart, Vector3 direction)
    {
        _enemyHealth.ApplyDamage(damage, hittedPart, direction);
    }

    private void OnEnemyDie(EnemyBodyPart hittedPart, Vector3 direction)
    {
        MakePhysical(hittedPart, direction);
    }

    private void MakeKinematic()
    {
        for (int i = 0; i < _bodyParts.Length; i++)
        {
            _bodyParts[i].Initialize(this);
            _bodyParts[i].MakeKinematic();
        }
    }

    private void MakePhysical(EnemyBodyPart hittedPart, Vector3 direction)
    {
        for (int i = 0; i < _bodyParts.Length; i++)
        {
            _bodyParts[i].MakePhysical();
        }

        hittedPart.SetVelocity(direction * 40f);

        _animator.enabled = false;
    }

    private void OnDestroy()
    {
        _enemyHealth.EnemyDie -= OnEnemyDie;        
    }
}
