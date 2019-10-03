using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHull2D : MonoBehaviour
{

    public PhysDetect hullType;

    public enum PhysDetect
    {
        Circle,
        AABB,
        OBB
    }

    struct CollisionPairType
    {
        CollisionHull2D shapeA;
        CollisionHull2D shapeB;
    }

    //private Dictionary<CollisionPairType, Func<CollisionHullType2D>

    public CollisionHull2D CollisionHullType { get; }

    /*private CollisionHull2D(CollisionHullType2D hullType)
    {
        CollisionHullType = hullType;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //PhysicWorld.Instance.AddCollision(this);
    }

    private void OnDisable()
    {
        //PhysicWorld.Instance.RemoveInstance(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static public bool CircleCircle(CircleCollision2D colA, CircleCollision2D colB)
    {

        //check to see if the distance between the two centers is less than or equal to the sum of the radius, otherwise return false

        /* 
         * if(radialSum >= Vector2.Distance(colA.center,colB.center)
         * { return true;}
         * else { return false;}
         */

        //How the slides say to do it
        //Step1: Calculate distance from center to center, distance = colB.center - colA.center
        //Step2: Add the two radius together colB.radius + colA.radius
        //Step3: Do DotProduct(distance,distance)<radius together^2

        Vector2 distance = colB.center - colA.center;
        float radialSum = colA.radius + colB.radius;
        if (Vector3.Dot(distance, distance) <= (radialSum * radialSum))
        {
            return true;
        }
        else
        {
            return false;
        }



    }
    static public bool CircleAABB(CircleCollision2D circle, AxisAllignedBoundingBoxCollision2D rect)
    {
        /*
         * Examples:
         * Clamp circle with closest point from the square to see 
         * 
         * Circle-point
         *   
         *  DeltaX = CircleX - PointX; --> deltaX = circle.x - square.x
            DeltaY = CircleY - PointY;
            return (DeltaX * DeltaX + DeltaY * DeltaY) < (CircleRadius * CircleRadius);
         *
        */

        /*
         * float closestPointX = Mathf.Max(square.position.x - square.halfLength.x, Mathf.Min(circle.position.x, square.position.x + square.halfLength.x));
        float closestPointY = Mathf.Max(square.position.y - square.halfLength.y, Mathf.Min(circle.position.y, square.position.y + square.halfLength.y));
         * 
         */

        // clamp(value, min, max) - limits value to the range min..max

        // Find the closest point to the circle within the rectangle
        float closestX = Mathf.Clamp(circle.center.x, rect.left.x, rect.right.x);
        float closestY = Mathf.Clamp(circle.center.x, rect.top.y, rect.bot.y);

        // Calculate the distance between the circle's center and this closest point
        float distanceX = circle.center.x - closestX;
        float distanceY = circle.center.y - closestY;


        // If the distance is less than the circle's radius, an intersection occurs
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        return distanceSquared < (circle.radius * circle.radius);
    }

    static public bool CircleOBB(CircleCollision2D circle, ObjectBoundingBoxCollision2D rect)
    {

        return false;
    }


    static public bool AABBAABB(AxisAllignedBoundingBoxCollision2D colA, AxisAllignedBoundingBoxCollision2D colB)
    {
        //test if max0>=min1 and max1>=min0
        if(colB.posMax.x >= colA.posMin.x || colA.posMax.x >= colB.posMin.x)
        {
            if(colB.posMax.y >= colA.posMin.y || colA.posMax.y >= colB.posMin.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    static public bool AABBOBB(AxisAllignedBoundingBoxCollision2D axisRect, ObjectBoundingBoxCollision2D objRect)
    {
        //AxisAllignedBoundingBoxCollision2D 
        return false;
    }

    static public bool OBBOBB(ObjectBoundingBoxCollision2D rectA, ObjectBoundingBoxCollision2D rectB)
    {

        return false;
    }

}
