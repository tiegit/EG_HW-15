using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemyType EnemyType {  get; private set; }
}
