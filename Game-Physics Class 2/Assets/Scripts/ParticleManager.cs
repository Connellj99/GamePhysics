using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public GameObject capsule;

    public static float createdTime;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cube);
        //save time when instantiated
        createdTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    void SpawnSphere()
    {
        Instantiate(sphere);
    }

    void SpawnCapsule()
    {
        Instantiate(capsule);
    }

    void SpawnCube()
    {
        Instantiate(cube);
    }

}
