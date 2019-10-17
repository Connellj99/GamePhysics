using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollision2D : CollisionHull2D
{
    public GameObject attachedShape;
    public Vector2 center;
    public Vector2 botLeft;
    public Vector2 topRight;
    public Vector2 botRight;
    public Vector2 topLeft;
    public Vector2 xNormal;
    public Vector2 yNormal;
    public Vector2 halfLength;

    public Vector2 halfExtends;

    public Vector2 posMin;
    public Vector2 posMax;



    public float zRot = 0.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        center = attachedShape.transform.position;
        botLeft = Quaternion.Euler(0, 0, zRot) * new Vector3(-halfLength.x, -halfLength.y) + new Vector3(center.x, center.y);
        botRight = Quaternion.Euler(0, 0, zRot) * new Vector3(halfLength.x, -halfLength.y) + new Vector3(center.x, center.y);
        topRight = Quaternion.Euler(0, 0, zRot) * new Vector3(halfLength.x, halfLength.y) + new Vector3(center.x, center.y);
        topLeft = Quaternion.Euler(0, 0, zRot) * new Vector3(-halfLength.x, halfLength.y) + new Vector3(center.x, center.y);
        xNormal = Quaternion.Euler(0, 0, zRot) * new Vector3(1, 0, 0).normalized;
        yNormal = Quaternion.Euler(0, 0, zRot) * new Vector3(0, 1, 0).normalized;

        posMin = botLeft;
        posMax = topRight;
        halfExtends = (posMax - posMin) / 2f;

    }

    public override CollisionInfo CollisionTests(CollisionHull2D other)
    {
        switch (other.hullType)
        {
            case CollisionHull2D.PhysDetect.Circle:
                return CollisionHull2D.CircleOBB(other as CircleCollision2D, this);
            case CollisionHull2D.PhysDetect.AABB:
                return CollisionHull2D.AABBOBB(other as AxisAllignedBoundingBoxCollision2D, this);
            case CollisionHull2D.PhysDetect.OBB:
                return CollisionHull2D.OBBOBB(this, other as ObjectBoundingBoxCollision2D);

            default:
                break;
        }

        return null;
    }

    private Vector2 rightAxis
    {
        get
        {
            return new Vector2(Mathf.Cos(zRot * Mathf.Deg2Rad), -Mathf.Sin(zRot * Mathf.Deg2Rad));
        }
    }

    private Vector2 upAxis
    {
        get
        {
            return new Vector2(Mathf.Sin(zRot * Mathf.Deg2Rad), Mathf.Cos(zRot * Mathf.Deg2Rad));
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
    }


    public IEnumerable<Vector2> normAxis
    {
        get
        {
            List<Vector2> newAxis = new List<Vector2>();
            newAxis.Add(rightAxis);
            newAxis.Add(upAxis);
            return newAxis;
        }
    }

    public IEnumerable<Vector2> verticeCheck
    {
        get
        {
            List<Vector2> vert = new List<Vector2>();

            float[,] rotoMat = new float[,] { { Mathf.Cos(zRot * Mathf.Deg2Rad), -Mathf.Sin(zRot * Mathf.Deg2Rad) }, { Mathf.Sin(zRot * Mathf.Deg2Rad), Mathf.Cos(zRot * Mathf.Deg2Rad) } };

            vert.Add(new Vector2(posMin.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0], posMin.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMax.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0], posMax.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMin.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0], posMin.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMax.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0], posMax.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);

            return vert;
        }
    }

   
   
}
