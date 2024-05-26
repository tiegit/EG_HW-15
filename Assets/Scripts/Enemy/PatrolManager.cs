using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    [SerializeField] private TargetPoint[] _enemyTargetPoints;

	public TargetPoint  GetRandomTarget()
	{
		int	index = Random.Range(0, _enemyTargetPoints.Length);
		return _enemyTargetPoints[index];
	}
}
