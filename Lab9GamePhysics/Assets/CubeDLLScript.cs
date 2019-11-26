using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDLLScript : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float zPos;
    public float mass;

    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
        mass = 1;
        PhysicDLL.AddParticle3D(ref mass, ref xPos, ref yPos, ref zPos);
    }

    // Update is called once per frame
    void Update()
    {
        PhysicDLL.AddForce(2.0f, 2.0f, 0.0f);
        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
