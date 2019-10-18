using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    //public GameObject colManager;
    public static float lazerLife = 10.0f;
    float lifeTimer = 0;
    public GameObject lazer;
    public float force = 1000.0f;
    // Use this for initialization
    void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(transform.up * 500.0f);
        Vector2 speed = transform.up * force;
        lazer.GetComponent<Particle2D>().AddForce(speed);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lazerLife)
        {
            Destroy(gameObject);
        }

    }
    
}
