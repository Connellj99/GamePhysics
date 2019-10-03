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
        Gizmos.color = Color.white;
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, y, 0);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos = transform.position + new Vector3(x, y, 0);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
