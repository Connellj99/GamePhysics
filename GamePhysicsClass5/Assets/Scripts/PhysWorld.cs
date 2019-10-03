using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWorld : MonoBehaviour
{
    public static int objectAmount = 3;
    public GameObject[] physObjects = new GameObject[objectAmount];
    public GameObject test1;
    public GameObject test2;
    public GameObject test3;
    public GameObject test4;
    public GameObject test5;
    public GameObject test6;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //CheckCollisions();
        if (CollisionHull2D.AABBAABB(test3.GetComponent<AxisAllignedBoundingBoxCollision2D>(), test4.GetComponent<AxisAllignedBoundingBoxCollision2D>()))
        {
            Debug.Log("Collision AABB AABB");
        }
        if (CollisionHull2D.CircleCircle(test1.GetComponent<CircleCollision2D>(), test2.GetComponent<CircleCollision2D>()))
        {
            Debug.Log("Collision Circ Circ");
        }
        if (CollisionHull2D.CircleAABB(test1.GetComponent<CircleCollision2D>(), test3.GetComponent<AxisAllignedBoundingBoxCollision2D>()))
        {
            Debug.Log("Collision Circ AABB");


        }
        if(CollisionHull2D.AABBOBB(test3.GetComponent<AxisAllignedBoundingBoxCollision2D>(), test5.GetComponent<ObjectBoundingBoxCollision2D>()))
        {
            Debug.Log("Collision AABB OBB");

        }
        if (CollisionHull2D.CircleOBB(test2.GetComponent<CircleCollision2D>(), test6.GetComponent<ObjectBoundingBoxCollision2D>()))
        {
            Debug.Log("Collision Circ OBB");
        }



    }

    /*
     * create an array of all objects in the world, and create a nested loop that loops through each one with the other
     * comparing if the item collides with another based on the enum type of the other to decide the  collision
     * 
     * 
     */
    void CheckCollisions()
     {
        int currentShape = 0;
        for(int i = currentShape; i < objectAmount; i++)
        {
            CollisionHull2D.PhysDetect newHullDetect = physObjects[i].GetComponent<CollisionHull2D>().hullType;
            if (newHullDetect == CollisionHull2D.PhysDetect.Circle)
            {
                for(int j = currentShape + 1; j < objectAmount; j++)
                {
                    CollisionHull2D.PhysDetect newHullDetect2 = physObjects[j].GetComponent<CollisionHull2D>().hullType;
                    if (newHullDetect == CollisionHull2D.PhysDetect.Circle)
                    {
                        //Circle Circle
                        if(CollisionHull2D.CircleCircle(physObjects[i].GetComponent<CircleCollision2D>(), physObjects[j].GetComponent<CircleCollision2D>()))
                        {
                            Debug.Log("Collision");
                        }
                        
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //Circle AABB
                        if(CollisionHull2D.CircleAABB(physObjects[i].GetComponent<CircleCollision2D>(), physObjects[j].GetComponent<AxisAllignedBoundingBoxCollision2D>()))
                        {
                            Debug.Log("Collision");

                        }

                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //Circle OBB
                        CollisionHull2D.CircleOBB(physObjects[i].GetComponent<CircleCollision2D>(), physObjects[j].GetComponent<ObjectBoundingBoxCollision2D>());

                    }
                }
            }
            else if(newHullDetect == CollisionHull2D.PhysDetect.AABB)
            {
                for (int j = currentShape + 1; j < objectAmount; j++)
                {
                    CollisionHull2D.PhysDetect newHullDetect2 = physObjects[j].GetComponent<CollisionHull2D>().hullType;
                    if (newHullDetect == CollisionHull2D.PhysDetect.Circle)
                    {
                        //AABB Circle
                        if(CollisionHull2D.CircleAABB(physObjects[j].GetComponent<CircleCollision2D>(), physObjects[i].GetComponent<AxisAllignedBoundingBoxCollision2D>()))
                        {
                            Debug.Log("Collision");
                        }

                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //AABB AABB
                        if(CollisionHull2D.AABBAABB(physObjects[i].GetComponent<AxisAllignedBoundingBoxCollision2D>(), physObjects[j].GetComponent<AxisAllignedBoundingBoxCollision2D>()))
                        {
                            Debug.Log("Collision");
                        }
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //AABB OBB
                        CollisionHull2D.AABBOBB(physObjects[i].GetComponent<AxisAllignedBoundingBoxCollision2D>(), physObjects[j].GetComponent<ObjectBoundingBoxCollision2D>());

                    }
                }
            }
            else if(newHullDetect == CollisionHull2D.PhysDetect.OBB)
            {
                for (int j = currentShape + 1; j < objectAmount; j++)
                {
                    CollisionHull2D.PhysDetect newHullDetect2 = physObjects[j].GetComponent<CollisionHull2D>().hullType;
                    if (newHullDetect == CollisionHull2D.PhysDetect.Circle)
                    {
                        //OBB Circle
                        CollisionHull2D.CircleOBB(physObjects[j].GetComponent<CircleCollision2D>(), physObjects[i].GetComponent<ObjectBoundingBoxCollision2D>());

                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //OBB AABB
                        CollisionHull2D.AABBOBB(physObjects[j].GetComponent<AxisAllignedBoundingBoxCollision2D>(), physObjects[i].GetComponent<ObjectBoundingBoxCollision2D>());

                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //OBB OBB
                        CollisionHull2D.OBBOBB(physObjects[i].GetComponent<ObjectBoundingBoxCollision2D>(), physObjects[j].GetComponent<ObjectBoundingBoxCollision2D>());

                    }
                }
            }
        }
     }

}
