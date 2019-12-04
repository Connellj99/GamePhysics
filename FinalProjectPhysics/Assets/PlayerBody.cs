using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public GameObject player;
    public Particle3D bodyPhysics;
    public float bodyYOffset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bodyPhysics.position = player.GetComponent<Particle3D>().position;
        bodyPhysics.position.y = player.GetComponent<Particle3D>().position.y + bodyYOffset;
    }
}
