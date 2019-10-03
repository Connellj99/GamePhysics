using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollision2D : MonoBehaviour
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

    public Vector2 posMin;
    public Vector2 posMax;



    public float zRot = 0.0f;


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
    }
    // Start is called before the first frame update
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
        topLeft = Quaternion.Euler(0, 0, zRot) * new Vector3(-halfLength.x, halfLength.y) +new Vector3(center.x, center.y);
        xNormal = Quaternion.Euler(0, 0, zRot) * new Vector3(1, 0, 0).normalized;
        yNormal = Quaternion.Euler(0, 0, zRot) * new Vector3(0, 1, 0).normalized;

        posMin = botLeft;
        posMax = topRight;

    }
}
