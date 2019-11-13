using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    public class CollisionInfo
    {
        //return this class instead of a boolean whether it collided or not
        public struct Contact
        {
            public Vector3 point;
            public Vector3 normal;
            public float restitution;
            public float penetration;
        }
        public Contact[] contacts = new Contact[4];
        public CollisionHull3D a;
        public CollisionHull3D b;

        bool status;
        public Vector3 closingVelocity;
        public Vector3 penetration;

        public CollisionInfo(CircleCollision3D colA, AxisAllignedBoundingBoxCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(AxisAllignedBoundingBoxCollision3D colA, AxisAllignedBoundingBoxCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(AxisAllignedBoundingBoxCollision3D colA, ObjectBoundingBoxCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(ObjectBoundingBoxCollision3D colA, ObjectBoundingBoxCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }
        public CollisionInfo(CircleCollision3D colA, CircleCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }

        public CollisionInfo(CircleCollision3D colA, ObjectBoundingBoxCollision3D colB, Vector3 normal, float penetration)
        {
            RigidBodyA = colA.GetComponent<Particle3D>();
            RigidBodyB = colB.GetComponent<Particle3D>();


            RelativeVelocity = RigidBodyB.velocity - RigidBodyA.velocity;

            contacts[0].normal = normal;
            contacts[0].penetration = penetration;
            contacts[0].restitution = Mathf.Min(RigidBodyA.restitution, RigidBodyB.restitution);


        }


        public Particle3D RigidBodyA { get; }
        public CollisionHull3D ShapeA { get; }
        public Particle3D RigidBodyB { get; }
        public CollisionHull3D ShapeB { get; }

        public Vector3 RelativeVelocity { get; }



        public void SetCVel(CollisionHull3D a, CollisionHull3D b)
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
    abstract public CollisionInfo CollisionTests(CollisionHull3D other);

    struct CollisionPairType
    {
        CollisionHull3D shapeA;
        CollisionHull3D shapeB;
    }


    public CollisionHull3D CollisionHullType { get; }


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
    static public CollisionInfo CircleCircle(CircleCollision3D colA, CircleCollision3D colB)
    {

        //How the slides say to do it
        //Step1: Calculate distance from center to center, distance = colB.center - colA.center
        //Step2: Add the two radius together colB.radius + colA.radius
        //Step3: Do DotProduct(distance,distance)<radius together^2
        
        Vector3 distance = colB.center - colA.center;
        float radialSum = colA.radius + colB.radius;
        if (Vector3.Dot(distance, distance) <= (radialSum * radialSum))
        {
            /*double angle = System.Math.Atan2(colB.center.y - colA.center.y, colB.center.x - colA.center.x);
            float circleDistance = (float)System.Math.Sqrt((colB.center.x - colA.center.x) * (colB.center.x - colA.center.x) + (colB.center.y - colA.center.y) * (colB.center.y - colA.center.y));
            float distToMove = radialSum - circleDistance;
            colB.center.x += (float)(System.Math.Cos(angle) * circleDistance);
            colB.center.y += (float)(System.Math.Cos(angle) * circleDistance);*/
            float fDistance = Mathf.Sqrt(Vector3.Dot(distance, distance));
            Debug.Log("Circ - collision");
            
            return new CollisionInfo(colA,colB,distance/fDistance,radialSum - fDistance);
        }
        else
        {
            return null;
        }



    }
    static public CollisionInfo CircleAABB(CircleCollision3D circle, AxisAllignedBoundingBoxCollision3D rect)
    {
        //Vector3 halfExtend = (rect.posMax - rect.posMin) / 2;

        // clamp(value, min, max) - limits value to the range min..max
        Vector3 circleBox = new Vector3(
            Mathf.Max(rect.posMin.x + rect.center.x, Mathf.Min(circle.center.x, rect.posMax.x + rect.center.x)),
            Mathf.Max(rect.posMin.y + rect.center.y, Mathf.Min(circle.center.y, rect.posMax.y + rect.center.y)),
            Mathf.Max(rect.posMin.z + rect.center.z, Mathf.Min(circle.center.z, rect.posMax.z + rect.center.z)));
        // Find the closest point to the circle within the rectangle
        float closestX = Mathf.Clamp(circle.center.x, -rect.halfExtends.x, rect.halfExtends.x);
        float closestY = Mathf.Clamp(circle.center.y, -rect.halfExtends.y, rect.halfExtends.y);
        float closestZ = Mathf.Clamp(circle.center.z, -rect.halfExtends.z, rect.halfExtends.z);

        Vector3 closestPoint = new Vector3(closestX, closestY,closestZ);

        // Calculate the distance between the circle's center and this closest point
        //float distanceX = circle.center.x - closestX;
        //float distanceY = circle.center.y - closestY;
        //float distanceZ = circle.center.z - closestZ;
        Vector3 disVec = circle.center - circleBox;

        //Vector3 distanceComplete = closestPoint - closestPoint; //possibly change to circle vox
        // If the distance is less than the circle's radius, an intersection occurs
        //float distSq = Vector3.Dot(distanceComplete, distanceComplete);
        float distSq = Vector3.Dot(disVec, disVec);

        //float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ);
        float distance = Mathf.Sqrt(distSq);
        //Debug.DrawLine(closestPoint, circle.center);

        if (distSq <= (circle.radius * circle.radius))
        {
            Debug.Log("square circ collision)");
            //return new CollisionInfo(circle, rect, -distanceComplete.normalized, circle.radius - distance);
            return new CollisionInfo(circle, rect, -disVec.normalized, circle.radius - distance);


        }
        return null;
        //return distanceSquared < (circle.radius * circle.radius);
    }

    static public CollisionInfo CircleOBB(CircleCollision3D circle, ObjectBoundingBoxCollision3D rect)
    {
        //transform circle into obb space transform.InverseTransformPoint(cirecle.postion);
        //then do circle AABB      
        Vector3 halfExtend = (rect.posMax - rect.posMin) / 2;
        Vector3 circleInOBB = rect.GetComponent<Particle3D>().GetWorldToObject().MultiplyPoint(circle.center);//rect.transform.InverseTransformPoint(circle.center);
        Vector3 circleBox = new Vector3(
            Mathf.Max(-halfExtend.x, Mathf.Min(circleInOBB.x, halfExtend.x)),
            Mathf.Max(-halfExtend.y, Mathf.Min(circleInOBB.y, halfExtend.y)),
            Mathf.Max(-halfExtend.z, Mathf.Min(circleInOBB.z, halfExtend.z)));

        Vector3 distanceVec = circleInOBB - circleBox;
        float distanceSQ = Vector3.Dot(distanceVec, distanceVec);

        //Debug.DrawLine(circleBox, circle.center);


        if (distanceSQ <= (circle.radius * circle.radius))
        {
            //return true;
            Debug.Log("circ obb collision");
            float distance = Mathf.Sqrt(distanceSQ);
            return new CollisionInfo(circle, rect, rect.transform.TransformVector(-distanceVec).normalized, circle.radius - distance);

        }

        return null;
         
    }


    static public CollisionInfo AABBAABB(AxisAllignedBoundingBoxCollision3D colA, AxisAllignedBoundingBoxCollision3D colB)
    {
        //test if max0>=min1 and max1>=min0
        Vector3 distance = colB.center - colA.center;
        float x_overlap = colA.halfExtends.x + colB.halfExtends.x - Mathf.Abs(distance.x);

        if(x_overlap > 0.0f)
        {
            float y_overlap = colA.halfExtends.y + colB.halfExtends.y - Mathf.Abs(distance.y);
            if (y_overlap > 0.0f)
            {
                float z_overlap = colA.halfExtends.z + colB.halfExtends.z - Mathf.Abs(distance.z);
                if (z_overlap > 0.0f)
                {

                    float minOverlap = Mathf.Min(x_overlap, y_overlap, z_overlap);
                    if (minOverlap == x_overlap)
                    {
                        Debug.Log("Square square collision");
                        return new CollisionInfo(colA, colB, distance.x < 0.0f ? -Vector3.right : Vector3.right, x_overlap);
                    }
                    else if (minOverlap == y_overlap)
                    {
                        Debug.Log("Square square collision");

                        return new CollisionInfo(colA, colB, distance.y < 0.0f ? -Vector3.up : Vector3.up, y_overlap);
                    }
                    else if (minOverlap == z_overlap)
                    {
                        Debug.Log("Square square collision");

                        return new CollisionInfo(colA, colB, distance.z < 0.0f ? -Vector3.forward : Vector3.forward, z_overlap);
                    }

                }
            }
        }
        

        return null;
    }


    static public CollisionInfo AABBOBB(AxisAllignedBoundingBoxCollision3D colA, ObjectBoundingBoxCollision3D colB)
    {
      
        List<Vector3> allAxis = new List<Vector3>();
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


                Vector3 AtoB = colB.center - colA.center;
                float x_overlap = colA.halfExtends.x + colB.halfExtends.x - Mathf.Abs(AtoB.x);

                if (x_overlap > 0.0f)
                {
                    float y_overlap = colA.halfExtends.y + colB.halfExtends.y - Mathf.Abs(AtoB.y);
                    if (y_overlap > 0.0f)
                    {
                        if (x_overlap < y_overlap)
                        {
                            Debug.Log("ABB OBB");
                            return new CollisionInfo(colA, colB, AtoB.x < 0.0f ? -Vector3.right : Vector3.right, x_overlap);
                        }
                        else
                        {
                            Debug.Log("ABB OBB");
                            return new CollisionInfo(colA, colB, AtoB.y < 0.0f ? -Vector3.up : Vector3.up, y_overlap);
                        }
                    }
                }
            }
        }
        return null;
    }

    static public CollisionInfo OBBOBB(ObjectBoundingBoxCollision3D rectA, ObjectBoundingBoxCollision3D rectB)
    {
        List<Vector3> allAxis = new List<Vector3>();
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
                Vector3 AtoB = rectB.center - rectA.center;
                float x_overlap = rectA.halfExtends.x + rectB.halfExtends.x - Mathf.Abs(AtoB.x);

                if (x_overlap > 0.0f)
                {
                    float y_overlap = rectA.halfExtends.y + rectB.halfExtends.y - Mathf.Abs(AtoB.y);
                    if (y_overlap > 0.0f)
                    {
                        if (x_overlap < y_overlap)
                        {
                            Debug.Log("OBB OBB");

                            return new CollisionInfo(rectA, rectB, AtoB.x < 0.0f ? -Vector3.right : Vector3.right, x_overlap);
                        }
                        else
                        {
                            Debug.Log("OBB OBB");

                            return new CollisionInfo(rectA, rectB, AtoB.y < 0.0f ? -Vector3.up : Vector3.up, y_overlap);
                        }
                    }
                }
            }
        }

        return null;


    }   


   
}
