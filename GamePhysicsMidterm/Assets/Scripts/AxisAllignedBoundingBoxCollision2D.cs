using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAllignedBoundingBoxCollision2D : CollisionHull2D
{

    public Vector2 posMin;
    public Vector2 posMax;
    public Vector2 leftTop;
    public Vector2 rightBot;
    public GameObject rect;
    public Vector2 center;

    public float zRot = 0.0f;

    //public Vector2 halfLength;
    public float rectTopBot;
    public float rectLeftRight;
    public Vector2 halfExtends;
    public Vector2 left;
    public Vector2 right;
    public Vector2 top;
    public Vector2 bot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("ColManager").GetComponent<PhysWorld>().AddObject(rect);


    }


    // Update is called once per frame
    void Update()
    {
        center = rect.transform.position;
        left = new Vector2(center.x - (0.5f * rectLeftRight), center.y);
        right = new Vector2(center.x + (0.5f * rectLeftRight), center.y);
        top = new Vector2(center.x, center.y + (0.5f * rectTopBot));
        bot = new Vector2(center.x, center.y - (0.5f * rectTopBot));

        posMax = new Vector2(right.x, top.y);
        posMin = new Vector2(left.x, bot.y);
        leftTop = new Vector2(left.x, top.y);
        rightBot = new Vector2(right.x, bot.y);
        halfExtends = (posMax - posMin) / 2f;

    }

    public override CollisionInfo CollisionTests(CollisionHull2D other)
    {
        switch (other.hullType)
        {
            case CollisionHull2D.PhysDetect.Circle:
                return CollisionHull2D.CircleAABB(other as CircleCollision2D, this);
            case CollisionHull2D.PhysDetect.AABB:
                return CollisionHull2D.AABBAABB(this, other as AxisAllignedBoundingBoxCollision2D);
            case CollisionHull2D.PhysDetect.OBB:
                return CollisionHull2D.AABBOBB(this, other as ObjectBoundingBoxCollision2D);

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
        Gizmos.color = Color.white;
        /*Gizmos.DrawLine(right, left);
        Gizmos.DrawLine(right,bot);
        Gizmos.DrawLine(left, top);
        Gizmos.DrawLine(top, bot);*/
        Gizmos.DrawLine(rightBot, posMax);
        Gizmos.DrawLine(leftTop, posMin);
        Gizmos.DrawLine(leftTop, posMax);
        Gizmos.DrawLine(rightBot, posMin);



        //Gizmos.DrawLine(posMin, posMax);

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

            float[,] rotoMat = new float[,] { { Mathf.Cos(zRot * Mathf.Deg2Rad), -Mathf.Sin(zRot * Mathf.Deg2Rad) },{ Mathf.Sin(zRot * Mathf.Deg2Rad) , Mathf.Cos(zRot * Mathf.Deg2Rad) } };

            vert.Add(new Vector2(posMin.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0],posMin.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMax.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0],posMax.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMin.x * rotoMat[0, 0] + posMax.y * rotoMat[1, 0],posMin.x * rotoMat[0, 1] + posMax.y * rotoMat[1, 1]) + center);
            vert.Add(new Vector2(posMax.x * rotoMat[0, 0] + posMin.y * rotoMat[1, 0],posMax.x * rotoMat[0, 1] + posMin.y * rotoMat[1, 1]) + center);

            return vert;
        }
    }

    

   
}
