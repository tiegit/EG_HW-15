using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemyType EnemyType {  get; private set; }
    public EnemyCenter EnemyCenter {  get; private set; }

    private void Awake()
    {
        EnemyCenter = GetComponentInChildren<EnemyCenter>();
    }
}
