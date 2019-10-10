using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWorld : MonoBehaviour
{
    public static int objectAmount = 6;
    public GameObject[] physObjects = new GameObject[objectAmount];
    public GameObject circle1;
    public GameObject circle2;
    public GameObject aabb1;
    public GameObject aabb2;
    public GameObject obb1;
    public GameObject obb2;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
            
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < objectAmount; ++i)
        {
            GameObject colA = physObjects[i];
            //colA.GetComponent<Renderer>().material.color = Color.green;

            for(int j = i+1; j < objectAmount; ++j)
            {
                GameObject colB = physObjects[j];
                
                
            }
        }
    }

 

   /* private void ProcessCollisions()
    {
        foreach( var colInfo in allCollisions)
        {
            ResolveVelocity(colInfo);
            ResolvePenetration(colInfo);
        }
    }*/

    void ResolveVelocity(CollisionHull2D.CollisionInfo colInfo)
    {
        float separatingVelocity = Vector2.Dot(colInfo.RelativeVelocity, colInfo.contacts[0].normal);

        if(separatingVelocity > 0.0f)
        {
            return;
        }
        float newSepVelocity = -separatingVelocity * colInfo.contacts[0].restitution;
        //Resting
        Vector2 accCausedVelocity = colInfo.RigidBodyB.acceleration - colInfo.RigidBodyA.acceleration;
        float accCausedSepVelocity = Vector2.Dot(accCausedVelocity,colInfo.contacts[0].normal) * Time.fixedDeltaTime;
        if(accCausedSepVelocity < 0)
        {
            newSepVelocity += colInfo.contacts[0].restitution * accCausedSepVelocity;
            if(newSepVelocity < 0)
            {
                newSepVelocity = 0.0f;
            }
        }

        float deltaVelocity = newSepVelocity - separatingVelocity;
        float totalInverseMass = colInfo.RigidBodyA.invMass + colInfo.RigidBodyB.invMass;
        if(totalInverseMass <= 0.0f)
        {
            //no fx
            return;
        }
        float impulse = deltaVelocity / totalInverseMass;
        Vector2 impulsePerMass = colInfo.contacts[0].normal * impulse;

        colInfo.RigidBodyA.velocity -= colInfo.RigidBodyA.invMass * impulsePerMass;
        colInfo.RigidBodyB.velocity += colInfo.RigidBodyB.invMass * impulsePerMass;


    }
    
    private void ResolvePenetration(CollisionHull2D.CollisionInfo colInfo)
    {
        if(colInfo.contacts[0].penetration <= 0.0f)
        {
            return;
        }

        float totalInverseMass = colInfo.RigidBodyA.invMass + colInfo.RigidBodyB.invMass;
        if (totalInverseMass <= 0.0f)
        {
            return;
        }
        Vector2 movePerMass = colInfo.contacts[0].normal * (colInfo.contacts[0].penetration / totalInverseMass);

        colInfo.RigidBodyA.position -= colInfo.RigidBodyA.invMass * movePerMass;
        colInfo.RigidBodyB.position += colInfo.RigidBodyB.invMass * movePerMass;

    }

    /*
     * create an array of all objects in the world, and create a nested loop that loops through each one with the other
     * comparing if the item collides with another based on the enum type of the other to decide the  collision
     * 
     * 
     */
    /*void CheckCollisions()
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
     }*/

}
