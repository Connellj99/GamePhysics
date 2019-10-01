using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollision2D : MonoBehaviour
{
    public GameObject attachedShape;
    public float radius;
    public Vector2 center;

    // Start is called before the first frame update
    void Start()
    {
        center = attachedShape.transform.position;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(center, radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
