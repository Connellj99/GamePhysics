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
    static public bool CircleAABB(CollisionHull2D colA,CollisionHull2D colB)
    {
        /*
         * I think for this I need to project the aabb along the axis, and then also project the radius^2 on each axis and see if they overlap
         * 
         */
        return false;
    }

    static public bool CircleOBB(CollisionHull2D colA, CollisionHull2D colB)
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


    static public bool AABBOBB(CollisionHull2D colA, CollisionHull2D colB)
    {
        //AxisAllignedBoundingBoxCollision2D 
        return false;
    }

    static public bool OBBOBB(CollisionHull2D colA, CollisionHull2D colB)
    {

        return false;
    }

}
