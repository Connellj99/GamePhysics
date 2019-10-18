using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWorld : MonoBehaviour
{
    public GameObject physManager;
    private List<CollisionHull2D.CollisionInfo> allCollisions = new List<CollisionHull2D.CollisionInfo>();

    public List<GameObject> activeCollisions;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
            
    }

    void FixedUpdate()
    {
        //loop through list, colliding based on enum
         foreach (var col in activeCollisions)
        {
            foreach (var col2 in activeCollisions)
            {
                if (col != col2)
                {
                    activeCollisions.RemoveAll(GameObject => GameObject == null);
                    
                    var collisionInfo = col.GetComponent<CollisionHull2D>().CollisionTests(col2.GetComponent<CollisionHull2D>());
                    if ((col.tag == "Lazer" && col2.tag == "Player") || (col.tag == "Player" && col2.tag == "Lazer"))
                    {
                        collisionInfo = null;
                    }

                    if ((col.tag == "Lazer" && col2.tag == "Lazer"))
                    {
                        collisionInfo = null;

                    }
                    //Debug.Log(collisionInfo); //very laggy
                    if (collisionInfo != null)
                    {
                        if ((col.tag == "Lazer" && col2.tag == "Rock"))
                        {
                            //Do rock split
                            col2.GetComponent<AsteroidScript>().AsteroidSplit();
                            RemoveObject(col);
                            Destroy(col);
                            
                        }
                        
                        if ((col.tag == "Rock" && col2.tag == "Lazer"))
                        {
                            //Do rock split
                            col.GetComponent<AsteroidScript>().AsteroidSplit();
                            RemoveObject(col2);
                            Destroy(col2);
                        }
                        if ((col.tag == "Lazer" && col2.tag == "SmallRock"))
                        {
                            //Do rock split
                            col2.GetComponent<SmallAsteroidScript>().AsteroidBreak();
                            RemoveObject(col);
                            Destroy(col);

                        }
                        if ((col.tag == "SmallRock" && col2.tag == "Lazer"))
                        {
                            //Do rock split
                            col.GetComponent<SmallAsteroidScript>().AsteroidBreak();
                            RemoveObject(col2);
                            Destroy(col2);

                        }

                        if ((col.tag == "Player" && col2.tag == "Rock") || (col.tag == "Rock" && col2.tag == "Player"))
                        {
                            //Remove life
                            physManager.GetComponent<UIManager>().DecreseLife();
                        }

                        if ((col.tag == "Player" && col2.tag == "SmallRock") || (col.tag == "SmallRock" && col2.tag == "Player"))
                        {
                            //Remove life
                            physManager.GetComponent<UIManager>().DecreseLife();

                        }

                        allCollisions.Add(collisionInfo);
                    }
                }
            }
        }
        DoCollisions();
    }

    
    public void AddObject(GameObject collider)
    {
        activeCollisions.Add(collider);
    }

    public void RemoveObject(GameObject collider)
    {
        activeCollisions.Remove(collider);
    }

    //cause collisions
    private void DoCollisions()
    {
        foreach( var colInfo in allCollisions)
        {
            ResolveVelocity(colInfo);
            ResolvePenetration(colInfo);
        }
        allCollisions.Clear();
    }

    //how the objects forces act upon each other
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
    
    //how deep the objects intersect
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


}
