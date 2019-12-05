using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Particle3D particleScript;
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
            particleScript.velocity.z -= 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            particleScript.velocity.x += 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            particleScript.velocity.z += 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            particleScript.velocity.x -= 2 * deltaTime;
        }
        if (Input.GetKey(KeyCode.B))
        {
            particleScript.velocity *= 1 * deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            particleScript.velocity.y = 20f;
        }
    }
}
