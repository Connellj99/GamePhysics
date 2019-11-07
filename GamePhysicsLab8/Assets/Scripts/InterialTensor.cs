using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterialTensor : MonoBehaviour
{
    public GameObject shape;
    /*
     *  Solid sphere
        Hollow sphere
        Solid box & cube
        Hollow box & cube
        Solid cylinder
        Solid cone
     */


    /*      2D code
     *    private void findInertia()
{
    if(shapetype == particleShape.Sphere)
    {
        float radius = shape.transform.localScale.x;
        inertia = 0.5f * mass * (radius * radius);
    }

    if (shapetype == particleShape.Cube)
    {
        float dxy = shape.transform.localScale.x;
        inertia = 0.083f * mass * (dxy * dxy);

    }

    if (shapetype == particleShape.Rectangle)
    {
        float dx = shape.transform.localScale.x;
        float dy = shape.transform.localScale.y;
        inertia = 0.083f * mass * (dx * dy);
    }
    invInertia = 1.0f/inertia;
}
     */
    public float radius = 0;
    public float width = 0;
    public float height = 0;
    public float depth = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Matrix4x4 findInertia()
    {
        Matrix4x4 inertia = new Matrix4x4();
        Particle3D.particleShape newPartShape = shape.GetComponent<Particle3D>().shapetype;
        if (newPartShape == Particle3D.particleShape.SolidSphere)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.4f * mass * (radius * radius), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.4f * mass * (radius * radius), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.4f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));

        }

        if (newPartShape == Particle3D.particleShape.HollowSphere)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.66f * mass * (radius * radius), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.66f * mass * (radius * radius), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.66f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));

        }

        if (newPartShape == Particle3D.particleShape.SolidBox)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.083f * mass * ((height * height) + (depth * depth)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.083f * mass * ((depth * depth) + (width * width)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.083f * mass * ((height * height) + (width * width)), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.HollowBox)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(1.66f * mass * ((height * height) + (depth * depth)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 1.66f * mass * ((depth * depth) + (width * width)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 1.66f * mass * ((height * height) + (width * width)), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.SolidCylinder)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.083f * mass *(3 * (radius * radius) + (height * height)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.083f * mass * (3 * (radius * radius) + (height * height)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.5f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.SolidCone)
        {
            float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(((0.6f * mass * (height * height)) + (0.15f * mass * (radius * radius))), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, ((0.6f * mass * (height * height)) + (0.15f * mass * (radius * radius))), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.3f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        
        return inertia.inverse;
    }
}
