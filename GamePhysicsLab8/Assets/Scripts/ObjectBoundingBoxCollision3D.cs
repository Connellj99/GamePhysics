using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollision3D : CollisionHull3D
{
    public GameObject attachedShape;
    public Vector3 center;
    public Vector3 botLeft;
    public Vector3 topRight;
    public Vector3 botRight;
    public Vector3 topLeft;
    public Vector3 xNormal;
    public Vector3 yNormal;
    public Vector3 halfLength;

    public Vector3 halfExtends;

    public Vector3 posMin;
    public Vector3 posMax;



    public float zRot = 0.0f;

    void Start()
    {
        GameObject.Find("ColManager").GetComponent<PhysWorld>().AddObject(attachedShape);
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

    public override CollisionInfo CollisionTests(CollisionHull3D other)
    {
        switch (other.hullType)
        {
            case CollisionHull3D.PhysDetect.Circle:
                return CollisionHull3D.CircleOBB(other as CircleCollision3D, this);
            case CollisionHull3D.PhysDetect.AABB:
                return CollisionHull3D.AABBOBB(other as AxisAllignedBoundingBoxCollision3D, this);
            case CollisionHull3D.PhysDetect.OBB:
                return CollisionHull3D.OBBOBB(this, other as ObjectBoundingBoxCollision3D);

            default:
                break;
        }

        return null;
    }

    private Vector3 rightAxis
    {
        get
        {
            return new Vector3(Mathf.Cos(zRot * Mathf.Deg2Rad), -Mathf.Sin(zRot * Mathf.Deg2Rad));
        }
    }

    private Vector3 upAxis
    {
        get
        {
            return new Vector3(Mathf.Sin(zRot * Mathf.Deg2Rad), Mathf.Cos(zRot * Mathf.Deg2Rad));
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


    public IEnumerable<Vector3> normAxis
    {
        get
        {
            List<Vector3> newAxis = new List<Vector3>();
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

            vert.Add(new Vector3(posMin.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0], posMin.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMax.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0], posMax.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMin.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0], posMin.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMax.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0], posMax.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);

            return vert;
        }
    }

   
   
}
