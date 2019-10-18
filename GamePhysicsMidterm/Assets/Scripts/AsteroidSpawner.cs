using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject colManager;
    public Transform spawnPos;
    public GameObject asteroid;
    float asteroidSpawnTimer = 0;
    float asteroidSpawnRate = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer >= asteroidSpawnRate)
        {
            SpawnAsteroid();
            asteroidSpawnTimer = 0.0f;
        }

    }

    void SpawnAsteroid()
    {
        Vector3 newPos = CreateAsteroidCoords();
        asteroid.GetComponent<Particle2D>().position = new Vector2(newPos.x, newPos.y);
        GameObject newAsteroid = Instantiate(asteroid, newPos, Quaternion.identity);
        colManager.GetComponent<PhysWorld>().AddObject(newAsteroid);

    }

    public Vector2 CreateAsteroidCoords()
    {
        Vector2 spawnPosition = new Vector2(0, spawnPos.position.y + 10);
        Vector2 randomVal = new Vector2(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
        spawnPosition += randomVal;
        return spawnPosition;
    }
}
