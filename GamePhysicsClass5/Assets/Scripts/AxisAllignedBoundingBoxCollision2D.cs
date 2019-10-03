using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAllignedBoundingBoxCollision2D : MonoBehaviour
{

    public Vector2 posMin;
    public Vector2 posMax;
    public GameObject rect;
    public Vector2 center;
    //public Vector2 halfLength;
    public float rectTopBot;
    public float rectLeftRight;

    public Vector2 left;
    public Vector2 right;
    public Vector2 top;
    public Vector2 bot;

    // Start is called before the first frame update
    void Start()
    {
       

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(right, left);
        Gizmos.DrawLine(right,bot);
        Gizmos.DrawLine(right, top);
        Gizmos.DrawLine(left, bot);
        Gizmos.DrawLine(left, top);
        Gizmos.DrawLine(top, bot);


    }

    // Update is called once per frame
    void Update()
    {
        center = rect.transform.position;

        left = new Vector2(center.x - (0.5f * rectLeftRight), center.y);
        right = new Vector2(center.x + (0.5f * rectLeftRight), center.y);
        top = new Vector2(center.x, center.y + (0.5f * rectTopBot));
        bot = new Vector2(center.x, center.y - (0.5f * rectTopBot));

        posMin = new Vector2(right.x, top.y);
        posMax = new Vector2(left.x, bot.y);

    }
}
