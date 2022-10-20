using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMover : MonoBehaviour
{
    [SerializeField] float speed = 1f,parabollicSpeed=10f, offset = 3f, xMin, xMax=10,zMin, zMax=10,yMin,yMax, distanceFromOriginalPos,
        wayPointMoveTime= Mathf.Infinity, timeToReachWaypoint= 30f, timeSinceArrivedAtWaypoint = Mathf.Infinity, 
        wayPointDwellTime= 5f, distanceToWaypoint, wayPointTolrance=1f;
    [SerializeField] bool move=true;
    [SerializeField] Vector3 randomWaypoint, originalPosition, parabollicWaypoint;

    void Start()
    {
        transform.position = new Vector3(Random.Range(0, 100), transform.position.y, Random.Range(0, 100));
        originalPosition = transform.position;
    }
    void Update()
    {
        if (GetComponent<Health>().IsDead()) { move = true; return; }        
        UpdateTimers();
        distanceFromOriginalPos = Vector3.Distance(transform.position, originalPosition);
        if (move)
        {
            Move();
        }
        else
        {
            Parabolla();
        }
    }

    private void UpdateTimers()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        wayPointMoveTime += Time.deltaTime;
    }

    private bool AtWayPoint()
    {
        distanceToWaypoint = Vector3.Distance(transform.position, randomWaypoint);
        return distanceToWaypoint <= wayPointTolrance;
    }
    void Move()
    {
        if (AtWayPoint() || wayPointMoveTime > timeToReachWaypoint)
        {
            if (distanceFromOriginalPos <= 1f)
            {
                float xPoint = Random.Range(-xMax, xMax);
                float zPoint = Random.Range(-zMax, zMax);
                xPoint = xPoint < 0 ? transform.position.x + xPoint - offset : transform.position.x + xPoint + offset;
                zPoint = zPoint < 0 ? transform.position.z + zPoint - offset : transform.position.z + zPoint + offset;
                randomWaypoint = new Vector3(xPoint, transform.position.y, zPoint);
            }
            else
            {
                randomWaypoint = originalPosition;
            }
            timeSinceArrivedAtWaypoint = 0;
            wayPointMoveTime = 0;
        }
        if (timeSinceArrivedAtWaypoint > wayPointDwellTime)
        {
            transform.LookAt(randomWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, randomWaypoint, speed * Time.deltaTime);
        }
    }
    void Parabolla()
    {
        if (Mathf.Abs(transform.position.x- parabollicWaypoint.x)<=offset &&
            Mathf.Abs(transform.position.z - parabollicWaypoint.z) <= offset)
        {
            move = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, parabollicWaypoint, parabollicSpeed * Time.deltaTime);
        }
    }
    public void Kick()
    {
        float xPoint = Random.Range(xMin, xMax);
        float yPoint = Random.Range(yMin, yMax);
        float zPoint = Random.Range(zMin, zMax);
        xPoint = xPoint < 0 ? transform.position.x + xPoint : transform.position.x + xPoint;
        yPoint = yPoint < 0 ? transform.position.y + yPoint : transform.position.y + yPoint;
        zPoint = zPoint < 0 ? transform.position.z + zPoint : transform.position.z + zPoint;
        parabollicWaypoint = new Vector3(xPoint, yPoint, zPoint);
        move = false;
    }
}
