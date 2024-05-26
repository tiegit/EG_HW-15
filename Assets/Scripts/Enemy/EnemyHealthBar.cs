using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _scaleTransform;
    [SerializeField] private float _healthBarHeight = 2f;

    private Transform _target;
    private Transform _cameraTransform;

    public void Initialize(Transform target)
    {
        _cameraTransform = Camera.main.transform;
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        transform.position = _target.position + Vector3.up * _healthBarHeight;
        
        transform.LookAt(_cameraTransform);
        //transform.rotation = _cameraTransform.rotation;
    }

    public void SetHealth(float health, float maxHealth)
    {
        float xScale = health / maxHealth;
        xScale = Mathf.Clamp01(xScale);

        _scaleTransform.localScale = new Vector3(xScale, 1, 1);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
