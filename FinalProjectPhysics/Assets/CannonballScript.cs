using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour
{
    // Start is called before the first frame update
    Particle3D self;

    Particle3D cannon;
    void Start()
    {
        cannon = GameObject.FindGameObjectWithTag("Player").GetComponent<Particle3D>();
        self.rotation = cannon.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
