using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroidScript : MonoBehaviour
{
    public GameObject colManager;
    public Particle2D particle2d;

    float boundsLeft = -122.0f;
    float boundsRight = 122.0f;
    float boundsUp = 66.0f;
    float boundsDown = -55.0f;


    public GameObject asteroid;
    float xRange = 10.0f;
    float yRange = 10.0f;
    public Vector2 directional;
    public float initialForce = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        directional.x = Random.Range(5, xRange);
        directional.y = Random.Range(5, yRange);
        AsteroidDirection(initialForce);
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
    }

    void AsteroidDirection(float force)
    {
        Vector2 speed = directional * force * 4.0f;
        asteroid.GetComponent<Particle2D>().AddForce(speed);

    }

    public void AsteroidBreak()
    {
        //increment score
        //colManager.GetComponent<UIManager>().IncreaseScoreSmall();
        Destroy(gameObject);
    }

    void CheckBounds()
    {
        Vector2 maxPosX;
        Vector2 maxPosY;

        //horizontal
        if (particle2d.position.x < boundsLeft)
        {
            maxPosX = new Vector2(boundsRight, particle2d.position.y);
            particle2d.position = maxPosX;
        }
        else if (transform.position.x > boundsRight)
        {
            maxPosX = new Vector2(boundsLeft, particle2d.position.y);
            particle2d.position = maxPosX;
        }

        //vertical
        if (particle2d.position.y < boundsDown)
        {
            maxPosY = new Vector2(particle2d.position.x, boundsUp);
            particle2d.position = maxPosY;
        }
        else if (transform.position.y > boundsUp)
        {
            maxPosY = new Vector2(particle2d.position.x, boundsDown);
            particle2d.position = maxPosY;
        }

    }
}
