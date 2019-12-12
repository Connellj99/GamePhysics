using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWorld : MonoBehaviour
{
    public GameObject physManager;
    private List<CollisionHull3D.CollisionInfo> allCollisions = new List<CollisionHull3D.CollisionInfo>();

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
            //col.GetComponent<Renderer>().material.color = Color.green;
            foreach (var col2 in activeCollisions)
            {
                if (col != col2)
                {
                    activeCollisions.RemoveAll(GameObject => GameObject == null);
                    var collisionInfo = col.GetComponent<CollisionHull3D>().CollisionTests(col2.GetComponent<CollisionHull3D>());
                    if ((col.tag == "Wall" && col2.tag == "Wall"))
                    {
                        collisionInfo = null;
                    }
                    if ((col.tag == "Player" && col2.tag == "PlayerBody"))
                    {
                        collisionInfo = null;
                    }
                    if ((col.tag == "Goal" && col2.tag == "Wall"))
                    {
                        collisionInfo = null;
                    }
                    if ((col.tag == "Wall" && col2.tag == "Goal"))
                    {
                        collisionInfo = null;
                    }
                    //Debug.Log(collisionInfo); //very laggy
                    if (collisionInfo != null)
                    {
                       
                        allCollisions.Add(collisionInfo);
                        //col.GetComponent<Renderer>().material.color = Color.red;
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
    void ResolveVelocity(CollisionHull3D.CollisionInfo colInfo)
    {
        float separatingVelocity = Vector3.Dot(colInfo.RelativeVelocity, colInfo.contacts[0].normal);

        if(separatingVelocity > 0.0f)
        {
            return;
        }
        float newSepVelocity = -separatingVelocity * colInfo.contacts[0].restitution;
        //Resting
        Vector3 accCausedVelocity = colInfo.RigidBodyB.acceleration - colInfo.RigidBodyA.acceleration;
        float accCausedSepVelocity = Vector3.Dot(accCausedVelocity,colInfo.contacts[0].normal) * Time.fixedDeltaTime;
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
        Vector3 impulsePerMass = colInfo.contacts[0].normal * impulse;

        colInfo.RigidBodyA.velocity -= colInfo.RigidBodyA.invMass * impulsePerMass;
        colInfo.RigidBodyB.velocity += colInfo.RigidBodyB.invMass * impulsePerMass;


    }
    
    //how deep the objects intersect
    private void ResolvePenetration(CollisionHull3D.CollisionInfo colInfo)
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
        Vector3 movePerMass = colInfo.contacts[0].normal * (colInfo.contacts[0].penetration / totalInverseMass);

        colInfo.RigidBodyA.position -= colInfo.RigidBodyA.invMass * movePerMass;
        colInfo.RigidBodyB.position += colInfo.RigidBodyB.invMass * movePerMass;

    }


}
