using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float movementSpeed = 2f;
    public List<Transform> wayPoints = new List<Transform>();
    int currentWayPoint = 0;
    bool movingForward = true;

    void FixedUpdate()
    {
        if (wayPoints.Count == 0)
            return;

        Transform targetWaypoint = wayPoints[currentWayPoint];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (movingForward)
            {
                if (currentWayPoint < wayPoints.Count - 1)
                {
                    currentWayPoint++;
                }
                else
                {
                    movingForward = false;
                    currentWayPoint--;
                }
            }
            else
            {
                if (currentWayPoint > 0)
                {
                    currentWayPoint--;
                }
                else
                {
                    movingForward = true;
                    currentWayPoint++;
                }
            }
        }
    }
}
