using UnityEngine;


public class EnemyBodyPart : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;

    private EnemyBodyPartManager _enemyBodyPartManager;
    private Rigidbody _rigidbody;

    public void Initialize(EnemyBodyPartManager enemyBodyPartManager)
    {
        _enemyBodyPartManager = enemyBodyPartManager;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MakePhysical()
    {
        _rigidbody.isKinematic = false;
    }

    public void MakeKinematic()
    {
        _rigidbody.isKinematic = true;
    }

    public void Hit(float damage, Vector3 direction)
    {
        _enemyBodyPartManager.Hit(damage * _damageMultiplier, this, direction);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }
}
