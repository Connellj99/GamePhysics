using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHull2D : MonoBehaviour
{
    public class CollisionInfo
    {
        //return this class instead of a boolean whether it collided or not
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitution;
        }
        public Contact[] contacts = new Contact[4];
        public CollisionHull2D a;
        public CollisionHull2D b;

        bool status;
        public Vector2 closingVelocity;
        public Vector2 penetration;
    }
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
        float closestY = Mathf.Clamp(circle.center.y, rect.bot.y, rect.top.y);

        Vector2 closestPoint = new Vector2(closestX, closestY);

        // Calculate the distance between the circle's center and this closest point
        float distanceX = circle.center.x - closestX;
        float distanceY = circle.center.y - closestY;


        // If the distance is less than the circle's radius, an intersection occurs
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        Debug.DrawLine(closestPoint, circle.center);
        return distanceSquared < (circle.radius * circle.radius);
    }

    static public bool CircleOBB(CircleCollision2D circle, ObjectBoundingBoxCollision2D rect)
    {
        //transform circle into obb space transform.InverseTransformPoint(cirecle.postion);
        //then do circle AABB      
        Vector2 halfExtend = (rect.posMax - rect.posMin) / 2;
        Vector2 circleInOBB = rect.transform.InverseTransformPoint(circle.center);
        Vector2 circleBox = new Vector2(Mathf.Max(-halfExtend.x, Mathf.Min(circleInOBB.x, halfExtend.x)),
            Mathf.Max(-halfExtend.y, Mathf.Min(circleInOBB.y, halfExtend.y)));

        Vector2 distance = circleInOBB - circleBox;
        float distanceSQ = Vector2.Dot(distance, distance);

        Debug.DrawLine(circleBox, circle.center);


        if (distanceSQ <= (circle.radius * circle.radius))
        {
            return true;
        }

        return false;
         
    }


    static public bool AABBAABB(AxisAllignedBoundingBoxCollision2D colA, AxisAllignedBoundingBoxCollision2D colB)
    {
        //test if max0>=min1 and max1>=min0
        if(colA.posMax.x >= colB.posMin.x && colB.posMax.x >= colA.posMin.x)
        {
            if(colA.posMax.y >= colB.posMin.y && colB.posMax.y >= colA.posMin.y)
            {
                return true;
            }
        }
        //Debug.Log("false");
        return false;
    }


    static public bool AABBOBB(AxisAllignedBoundingBoxCollision2D colA, ObjectBoundingBoxCollision2D colB)
    {
        //AxisAllignedBoundingBoxCollision2D 
        //transform min and max into obb space, then do AABB AABB
        
        if (colA.posMax.x >= colB.posMin.x && colB.posMax.x >= colA.posMin.x)
        {
            if (colA.posMax.y >= colB.posMin.y && colB.posMax.y >= colA.posMin.y)
            {
                //return true;
                Vector2 abbMinTrans = colB.transform.InverseTransformPoint(colA.posMin);
                Vector2 abbMaxTrans = colB.transform.InverseTransformPoint(colA.posMax);

                if (abbMaxTrans.x >= colB.posMin.x && colB.posMax.x >= abbMinTrans.x)
                {
                    if (abbMaxTrans.y >= colB.posMin.y && colB.posMax.y >= abbMinTrans.y)
                    {
                        return true;
                                             
                    }
                }

            }
        }
        return false;
    }

    static public bool OBBOBB(ObjectBoundingBoxCollision2D rectA, ObjectBoundingBoxCollision2D rectB)
    {
        //grab 4 axis of both shapes
        // grab all vertices of both shapes, then put them onto each axis of both shapes
        //
        Vector2 rightA = new Vector2(Mathf.Cos(rectA.zRot), -Mathf.Sin(rectA.zRot));
        Vector2 upA = new Vector2(Mathf.Sin(rectA.zRot), Mathf.Cos(rectA.zRot));
        Vector2 rightB = new Vector2(Mathf.Cos(rectB.zRot), -Mathf.Sin(rectB.zRot));
        Vector2 upB = new Vector2(Mathf.Sin(rectB.zRot), Mathf.Cos(rectB.zRot));

        if (OBBTest(rightA, rectA, rectB))
        {
            if (OBBTest(upA, rectA, rectB))
            {
                if (OBBTest(rightB, rectB, rectA))
                {
                    if (OBBTest(upB, rectB, rectB))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    static private bool OBBTest(Vector2 norm, ObjectBoundingBoxCollision2D proj, ObjectBoundingBoxCollision2D main)
    {
        //top left is min.x, max y
        //bottom right is max.x, min.y

        Vector2 Max1;
        Vector2 Min1;
        Vector2 Max2;
        Vector2 Min2;


        Vector2 p1 = Vector2.Dot(proj.topRight, norm) * norm;
        Vector2 p2 = Vector2.Dot(proj.botLeft, norm) * norm;
        Vector2 p3 = Vector2.Dot(new Vector2(proj.topRight.x, proj.botLeft.y), norm) * norm;
        Vector2 p4 = Vector2.Dot(proj.botLeft, norm) * norm;

        if (p1.x <= p3.x && p1.y <= p3.y)
        {
            p1 = p3;
        }
        if (p2.x >= p4.x && p2.y >= p4.y)
        {
            p2 = p4;
        }

        Max1 = p1;
        Min1 = p2;

        Max2 = Vector2.Dot(main.topRight, norm) * norm;
        Min2 = Vector2.Dot(main.botLeft, norm) * norm;

        if (Max1.x >= Min2.x && Max1.y >= Min2.y && Max2.x >= Min1.x && Max2.y >= Min1.y)
        {
            return true;
        }
        return false;
    }

}
