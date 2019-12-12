using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Particle3D particleScript;
    public Vector3 forwardVector = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyChecks(Time.deltaTime);
    }

    void GetKeyChecks(float deltaTime)
    {
        if (Input.GetKey(KeyCode.W))
        {
            particleScript.worldTransformationMatrix.MultiplyVector(forwardVector);
            particleScript.velocity -= forwardVector * deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //particleScript.velocity.x += 2 * deltaTime;
            particleScript.angularAcceleration.y -= 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            particleScript.worldTransformationMatrix.MultiplyVector(-forwardVector);
            particleScript.velocity += forwardVector * deltaTime;

        }
        if (Input.GetKey(KeyCode.D))
        {
            particleScript.angularAcceleration.y += 2 * deltaTime;

            //particleScript.velocity.x -= 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.B))
        {
            particleScript.velocity *= 1 * deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            particleScript.velocity.y = 20f;
        }
        if(Input.GetMouseButtonDown(0))
        {

        }
    }
}
