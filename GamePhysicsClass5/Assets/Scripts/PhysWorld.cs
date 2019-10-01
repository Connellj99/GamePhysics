using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWorld : MonoBehaviour
{
    public static int objectAmount = 6;
    public GameObject[] physObjects = new GameObject[objectAmount];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //Circle AABB
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //Circle OBB
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
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //AABB AABB
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //AABB OBB
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
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.AABB)
                    {
                        //OBB AABB
                    }
                    else if (newHullDetect == CollisionHull2D.PhysDetect.OBB)
                    {
                        //OBB OBB
                    }
                }
            }
        }
     }

}
