using UnityEngine;

public static class SearchUtility
{
    public static bool SearchInSector(Vector3 viewerPosition, Vector3 viewerDirection, Vector3 targetPosition, float viewingAngle,  float viewingDistance, LayerMask wallsMask)
    {
        Vector3 toPlayer = targetPosition - viewerPosition;

        Vector3 start = viewerPosition;
        Vector3 end = targetPosition;

        float angle = Vector3.Angle(viewerDirection, toPlayer);
        Vector3 toPlayerXZ = new Vector3(toPlayer.x, 0, toPlayer.z);
        float distance = toPlayerXZ.magnitude;

        if (angle < viewingAngle && distance < viewingDistance)
        {
            if (Physics.Linecast(start, end, wallsMask) == false)
            {
                return true;
            }
        }

        return false;
    }
}