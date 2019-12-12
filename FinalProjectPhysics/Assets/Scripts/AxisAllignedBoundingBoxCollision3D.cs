using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAllignedBoundingBoxCollision3D : CollisionHull3D
{

    public Vector3 posMin;
    public Vector3 posMax;
    //public Vector3 leftTop;
    //public Vector3 rightBot;
    public GameObject rect;
    public Vector3 center;

    public float zRot = 0.0f;

    //public Vector2 halfLength;
    public float rectTopBot;
    public float rectLeftRight;
    public Vector3 halfExtends;
    /*public Vector3 left;
    public Vector3 right;
    public Vector3 top;
    public Vector3 bot;*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("ColManager").GetComponent<PhysWorld>().AddObject(rect);
        rectLeftRight = halfExtends.x;
        rectTopBot = halfExtends.y;
    }


    // Update is called once per frame
    void Update()
    {
        center = rect.transform.position;
        //left = new Vector3(center.x - rectLeftRight, center.y);
       // right = new Vector3(center.x + rectLeftRight, center.y);
        //top = new Vector3(center.x, center.y + rectTopBot);
        //bot = new Vector3(center.x, center.y - rectTopBot);

        //posMax = new Vector3(right.x, top.y);
        posMax = new Vector3(center.x + halfExtends.x, center.y + halfExtends.y, center.z + halfExtends.z);
        //posMin = new Vector3(left.x, bot.y);
        posMin = new Vector3(center.x - halfExtends.x, center.y - halfExtends.y, center.z - halfExtends.z);

        //leftTop = new Vector3(left.x, top.y);
        //rightBot = new Vector3(right.x, bot.y);
        halfExtends = (posMax - posMin) / 2f;
        transform.eulerAngles = new Vector3(0, 0, zRot);

    }

    public override CollisionInfo CollisionTests(CollisionHull3D other)
    {
        switch (other.hullType)
        {
            case CollisionHull3D.PhysDetect.Circle:
                return CollisionHull3D.CircleAABB(other as CircleCollision3D, this);
            case CollisionHull3D.PhysDetect.AABB:
                return CollisionHull3D.AABBAABB(this, other as AxisAllignedBoundingBoxCollision3D);
            case CollisionHull3D.PhysDetect.OBB:
                return CollisionHull3D.AABBOBB(this, other as ObjectBoundingBoxCollision3D);

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
        Gizmos.color = Color.white;
        /*Gizmos.DrawLine(right, left);
        Gizmos.DrawLine(right,bot);
        Gizmos.DrawLine(left, top);
        Gizmos.DrawLine(top, bot);*/
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, halfExtends);



        //Gizmos.DrawLine(posMin, posMax);

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

    public IEnumerable<Vector3> verticeCheck
    {
        get
        {
            List<Vector3> vert = new List<Vector3>();

            float[,] rotoMat = new float[,] { { Mathf.Cos(zRot * Mathf.Deg2Rad), -Mathf.Sin(zRot * Mathf.Deg2Rad) },{ Mathf.Sin(zRot * Mathf.Deg2Rad) , Mathf.Cos(zRot * Mathf.Deg2Rad) } };

            vert.Add(new Vector3(posMin.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0],posMin.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMax.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0],posMax.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMin.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0],posMin.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector3(posMax.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0],posMax.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);

            return vert;
        }
    }

    

   
}
