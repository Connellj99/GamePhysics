using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollision2D : CollisionHull2D
{
    public GameObject attachedShape;
    public float radius;
    //public float restitution;
    public Vector2 center = new Vector2(0,0);

    // Start is called before the first frame update
    void Start()
    {
        center = attachedShape.transform.position;
    }

    public override CollisionInfo CollisionTests(CollisionHull2D other)
    {
        switch (other.hullType)
        {
            case CollisionHull2D.PhysDetect.Circle:
                return CollisionHull2D.CircleCircle(this, other as CircleCollision2D);
            case CollisionHull2D.PhysDetect.AABB:
                return CollisionHull2D.CircleAABB(this, other as AxisAllignedBoundingBoxCollision2D);
            case CollisionHull2D.PhysDetect.OBB:
                return CollisionHull2D.CircleOBB(this, other as ObjectBoundingBoxCollision2D);

            default:
                break;
        }

        return null;
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
        center = attachedShape.transform.position;

    }
}
