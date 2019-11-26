using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PhysicDLL.CreatePhysicsWorld();
    }

    // Update is called once per frame
    void Update()
    {
        PhysicDLL.UpdatePhysicsWorld(Time.deltaTime);
    }
}
