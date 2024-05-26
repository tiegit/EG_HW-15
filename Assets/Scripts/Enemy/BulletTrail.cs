using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2f;

    private LineRenderer _lineRenderer;
    private Material _material;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Vector3 a, Vector3 b, Material material)
    {
        _lineRenderer.SetPosition(0, a);
        _lineRenderer.SetPosition(1, b);
        _material = material;

        StartCoroutine(LifeProcess());
    }

    private IEnumerator LifeProcess()
    {
        for (float t = 0; t < 1f; t+= Time.deltaTime / _lifeTime)
        {
            float alpha = 1 - t;
            _lineRenderer.material = _material;
            yield return null;
        }

        Destroy(gameObject);
    }
}
