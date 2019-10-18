using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class CollisionInfo
    {
        //return this class instead of a boolean whether it collided or not
        public struct Contact
        {
            public Vector2 point;
            public Vector2 normal;
            public float restitution;
            public float penetration;
        }
        public Contact[] contacts = new Contact[4];
        public CollisionHull2D a;
        public CollisionHull2D b;

        bool status;
        public Vector2 closingVelocity;
        public Vector2 penetration;

        public CollisionInfo(CircleCollision2D colA, AxisAllignedBoundingBoxCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(AxisAllignedBoundingBoxCollision2D colA, AxisAllignedBoundingBoxCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(AxisAllignedBoundingBoxCollision2D colA, ObjectBoundingBoxCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(ObjectBoundingBoxCollision2D colA, ObjectBoundingBoxCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(CircleCollision2D colA, CircleCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }

        public CollisionInfo(CircleCollision2D colA, ObjectBoundingBoxCollision2D colB, Vector2 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle2D>();
            RigidBodyB = colB.GetComponent<Particle2D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }


        public Particle2D RigidBodyA { get; }
        public CollisionHull2D ShapeA { get; }
        public Particle2D RigidBodyB { get; }
        public CollisionHull2D ShapeB { get; }

        public Vector2 RelativeVelocity { get; }



        public void SetCVel(CollisionHull2D a, CollisionHull2D b)
        {
            //closingVelocity = a.transform.position
        }
    }

    public PhysDetect hullType;

    public enum PhysDetect
    {
        Circle,
        AABB,
        OBB
    }

    /*struct Collision
    {
        bool status;
    }*/
    abstract public CollisionInfo CollisionTests(CollisionHull2D other);

    struct CollisionPairType
    {
        CollisionHull2D shapeA;
        CollisionHull2D shapeB;
    }


    public CollisionHull2D CollisionHullType { get; }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static public CollisionInfo CircleCircle(CircleCollision2D colA, CircleCollision2D colB)
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
        if (Vector2.Dot(distance, distance) <= (radialSum * radialSum))
        {
            /*double angle = System.Math.Atan2(colB.center.y - colA.center.y, colB.center.x - colA.center.x);
            float circleDistance = (float)System.Math.Sqrt((colB.center.x - colA.center.x) * (colB.center.x - colA.center.x) + (colB.center.y - colA.center.y) * (colB.center.y - colA.center.y));
            float distToMove = radialSum - circleDistance;
            colB.center.x += (float)(System.Math.Cos(angle) * circleDistance);
            colB.center.y += (float)(System.Math.Cos(angle) * circleDistance);*/
            float fDistance = Mathf.Sqrt(Vector2.Dot(distance, distance));

            return new CollisionInfo(colA,colB,distance/fDistance,radialSum - fDistance);
        }
        else
        {
            return null;
        }



    }
    static public CollisionInfo CircleAABB(CircleCollision2D circle, AxisAllignedBoundingBoxCollision2D rect)
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
        Vector2 distanceComplete = circle.center - closestPoint;
        // If the distance is less than the circle's radius, an intersection occurs
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        float distance = Mathf.Sqrt(distanceSquared);
        //Debug.DrawLine(closestPoint, circle.center);

        if (distanceSquared <= (circle.radius * circle.radius))
        {
            return new CollisionInfo(circle, rect, -distanceComplete.normalized, circle.radius - distance);

        }
        return null;
        //return distanceSquared < (circle.radius * circle.radius);
    }

    static public CollisionInfo CircleOBB(CircleCollision2D circle, ObjectBoundingBoxCollision2D rect)
    {
        //transform circle into obb space transform.InverseTransformPoint(cirecle.postion);
        //then do circle AABB      
        Vector2 halfExtend = (rect.posMax - rect.posMin) / 2;
        Vector2 circleInOBB = rect.transform.InverseTransformPoint(circle.center);
        Vector2 circleBox = new Vector2(Mathf.Max(-halfExtend.x, Mathf.Min(circleInOBB.x, halfExtend.x)),
            Mathf.Max(-halfExtend.y, Mathf.Min(circleInOBB.y, halfExtend.y)));

        Vector2 distanceVec = circleInOBB - circleBox;
        float distanceSQ = Vector2.Dot(distanceVec, distanceVec);

        //Debug.DrawLine(circleBox, circle.center);


        if (distanceSQ <= (circle.radius * circle.radius))
        {
            //return true;
            float distance = Mathf.Sqrt(distanceSQ);
            return new CollisionInfo(circle, rect, rect.transform.TransformVector(-distanceVec).normalized, circle.radius - distance);

        }

        return null;
         
    }


    static public CollisionInfo AABBAABB(AxisAllignedBoundingBoxCollision2D colA, AxisAllignedBoundingBoxCollision2D colB)
    {
        //test if max0>=min1 and max1>=min0
        Vector2 distance = colB.center - colA.center;
        float x_overlap = colA.rectLeftRight + colB.rectLeftRight - Mathf.Abs(distance.x);

        if(x_overlap > 0.0f)
        {
            float y_overlap = colA.rectTopBot + colB.rectTopBot - Mathf.Abs(distance.y);
            if (y_overlap > 0.0f)
            {
                if(x_overlap < y_overlap)
                {
                    return new CollisionInfo(colA, colB, distance.x < 0.0f ? -Vector2.right : Vector2.right, x_overlap);
                }
                else
                {
                    return new CollisionInfo(colA, colB, distance.y < 0.0f ? -Vector2.up : Vector2.up, y_overlap);

                }
            }
        }
        
        /*if (colA.posMax.x >= colB.posMin.x && colB.posMax.x >= colA.posMin.x)
        {
            if(colA.posMax.y >= colB.posMin.y && colB.posMax.y >= colA.posMin.y)
            {
                return true;

            }
        }*/
        //Debug.Log("false");
        return null;
    }


    static public CollisionInfo AABBOBB(AxisAllignedBoundingBoxCollision2D colA, ObjectBoundingBoxCollision2D colB)
    {
        //AxisAllignedBoundingBoxCollision2D 
        //transform min and max into obb space, then do AABB AABB
        
            //if (colA.posMax.x >= colB.posMin.x && colB.posMax.x >= colA.posMin.x)
            //{
            //    if (colA.posMax.y >= colB.posMin.y && colB.posMax.y >= colA.posMin.y)
            //    {
            //        //return true;
            //        Vector2 abbMinTrans = colB.transform.InverseTransformPoint(colA.posMin);
            //        Vector2 abbMaxTrans = colB.transform.InverseTransformPoint(colA.posMax);

            //        if (abbMaxTrans.x >= colB.posMin.x && colB.posMax.x >= abbMinTrans.x)
            //        {
            //            if (abbMaxTrans.y >= colB.posMin.y && colB.posMax.y >= abbMinTrans.y)
            //            {
            //                return true;

            //            }
            //        }

            //    }
            //}
            //return false;
        List<Vector2> allAxis = new List<Vector2>();
        allAxis.AddRange(colA.normAxis);
        allAxis.AddRange(colB.normAxis);

        foreach (var axis in allAxis)
        {
            float AABBMin = float.MaxValue;
            float AABBMax = float.MinValue;

            foreach (var vert in colA.verticeCheck)
            {
                float dotValue = (vert.x * axis.x + vert.y * axis.y);
                if (dotValue < AABBMin)
                {
                    AABBMin = dotValue;
                }
                if (dotValue > AABBMax)
                {
                    AABBMax = dotValue;
                }
            }

            float OBBMin = float.MaxValue;
            float OBBMax = float.MinValue;
            foreach (var vert in colB.verticeCheck)
            {
                float dotValue = (vert.x * axis.x + vert.y * axis.y);
                if (dotValue < OBBMin)
                {
                    OBBMin = dotValue;
                }
                if (dotValue > OBBMax)
                {
                    OBBMax = dotValue;
                }
            }

            if (!(AABBMax < OBBMin && OBBMax < AABBMin))
            {


                Vector2 AtoB = colB.center - colA.center;
                float x_overlap = colA.halfExtends.x + colB.halfExtends.x - Mathf.Abs(AtoB.x);

                if (x_overlap > 0.0f)
                {
                    float y_overlap = colA.halfExtends.y + colB.halfExtends.y - Mathf.Abs(AtoB.y);
                    if (y_overlap > 0.0f)
                    {
                        if (x_overlap < y_overlap)
                        {
                            return new CollisionInfo(colA, colB, AtoB.x < 0.0f ? -Vector2.right : Vector2.right, x_overlap);
                        }
                        else
                        {
                            return new CollisionInfo(colA, colB, AtoB.y < 0.0f ? -Vector2.up : Vector2.up, y_overlap);
                        }
                    }
                }
            }
        }
        return null;
    }

    static public CollisionInfo OBBOBB(ObjectBoundingBoxCollision2D rectA, ObjectBoundingBoxCollision2D rectB)
    {
        List<Vector2> allAxis = new List<Vector2>();
        allAxis.AddRange(rectA.normAxis);
        allAxis.AddRange(rectB.normAxis);

        foreach (var axis in allAxis)
        {
            float OBB1Min = float.MaxValue;
            float OBB1Max = float.MinValue;

            foreach (var vert in rectA.verticeCheck)
            {
                float dotValue = (vert.x * axis.x + vert.y * axis.y);
                if (dotValue < OBB1Min)
                {
                    OBB1Min = dotValue;
                }
                if (dotValue > OBB1Max)
                {
                    OBB1Max = dotValue;
                }
            }

            float OBB2Min = float.MaxValue;
            float OBB2Max = float.MinValue;
            foreach (var vert in rectB.verticeCheck)
            {
                float dotValue = (vert.x * axis.x + vert.y * axis.y);
                if (dotValue < OBB2Min)
                {
                    OBB2Min = dotValue;
                }
                if (dotValue > OBB2Max)
                {
                    OBB2Max = dotValue;
                }
            }

            if (!(OBB1Max < OBB2Min && OBB2Max < OBB1Min))
            {
                Vector2 AtoB = rectB.center - rectA.center;
                float x_overlap = rectA.halfExtends.x + rectB.halfExtends.x - Mathf.Abs(AtoB.x);

                if (x_overlap > 0.0f)
                {
                    float y_overlap = rectA.halfExtends.y + rectB.halfExtends.y - Mathf.Abs(AtoB.y);
                    if (y_overlap > 0.0f)
                    {
                        if (x_overlap < y_overlap)
                        {
                            return new CollisionInfo(rectA, rectB, AtoB.x < 0.0f ? -Vector2.right : Vector2.right, x_overlap);
                        }
                        else
                        {
                            return new CollisionInfo(rectA, rectB, AtoB.y < 0.0f ? -Vector2.up : Vector2.up, y_overlap);
                        }
                    }
                }
            }
        }

        return null;


    }   


    /*void ResolveCollision(CircleCollision2D colA, CircleCollision2D colB,Vector2 norm)
    {
        Vector2 relativeV = (colB.center - colA.center);
        float velAlongNorm = Vector2.Dot(relativeV, norm);
        if(velAlongNorm > 0)
        {
            return;
        }
        float e = Mathf.Min(colA.restitution, colB.restitution);
        float j = -(1 + e) * velAlongNorm;
        j /= 1 / colA.mass + 1 / colB.mass;
        Vector2 impulse = j * norm;
        colA.velocity -= 1 / colA.mass * impulse;
        colB.velocity += 1 / colB.mass * impulse;

    }*/

   
}
