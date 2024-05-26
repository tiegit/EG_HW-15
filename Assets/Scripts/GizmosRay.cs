using UnityEngine;

public class GizmosRay : MonoBehaviour
{
    [SerializeField] private float _length = 30f;
    [SerializeField] private bool _show = true;

    private void OnDrawGizmos()
    {
        if (_show)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * _length);
        }
    }
}
