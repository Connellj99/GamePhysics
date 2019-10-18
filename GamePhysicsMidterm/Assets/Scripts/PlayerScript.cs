using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject player;
    public Particle2D particle2d;
    float xMove = 0f;
    float yMove = 0f;

    float boundsLeft = -122.0f;
    float boundsRight = 122.0f;
    float boundsUp = 66.0f;
    float boundsDown = -55.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckThrust();
        Rotate();
        CheckBounds();
    }

    void CheckThrust()
    {
        xMove = Input.GetAxis("Horizontal") / 2.0f;
        yMove = Input.GetAxis("Vertical");
        //Debug.Log(xMove);
        SpaceThruster(yMove);
    }

    void SpaceThruster(float force)
    {
        Vector2 speed = transform.up * force * 2.0f;
        player.GetComponent<Particle2D>().AddForce(speed * 10.0f);
        //Particle2D.AddForce(speed);
        //rb.AddForce(speed);
    }

    void Rotate()
    {
        player.GetComponent<Particle2D>().updateRotationKinematic(xMove);
        //player.GetComponent<ObjectBoundingBoxCollision2D>().zRot += xMove;
        //transform.Rotate(0, 0, -rotation);

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
