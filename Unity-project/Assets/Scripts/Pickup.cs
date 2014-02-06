using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{

    float bezierTime;
    float curveX;
    float curveY;
    float curveZ;
    float startPointX;
    float startPointY;
    float startPointZ;
    float endPointX;
    float endPointY;
    float endPointZ;
    float controlPointX;
    float controlPointY;
    float controlPointZ;

    public float speed=3;

    bool isActive;

    //[HideInInspector]
    public Texture2D icon;

    //[HideInInspector]
    public resourceEnum resource;

    void Start()
    {
        gameObject.tag = "Pickup";

        startPointX = transform.position.x;
        startPointY = transform.position.y;
        startPointZ = transform.position.z;

        Vector3 endPoint = new Vector3(endPointX, endPointY, endPointZ);
        float distance = Vector3.Distance(transform.position, endPoint);

        controlPointX = Mathf.Lerp(endPointX, startPointX, 0.5f);
        controlPointY = startPointY + (distance * 1f);
        controlPointZ = Mathf.Lerp(endPointZ, startPointZ, 0.5f);

        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {
            bezierTime += speed * Time.deltaTime;

            if (bezierTime >= 1)
            {
                bezierTime = 1;
            }

            curveX = (((1 - bezierTime) * (1 - bezierTime)) * startPointX) + (2 * bezierTime * (1 - bezierTime) * controlPointX) + ((bezierTime * bezierTime) * endPointX);
            curveY = (((1 - bezierTime) * (1 - bezierTime)) * startPointY) + (2 * bezierTime * (1 - bezierTime) * controlPointY) + ((bezierTime * bezierTime) * endPointY);
            curveZ = (((1 - bezierTime) * (1 - bezierTime)) * startPointZ) + (2 * bezierTime * (1 - bezierTime) * controlPointZ) + ((bezierTime * bezierTime) * endPointZ);

            transform.position = new Vector3(curveX, curveY, curveZ);
        }

        if (bezierTime >= 1)
        {
            bezierTime = 0;
            isActive = false;
        }

    }

    void setEndPoints(Vector3 points)
    {
        endPointX = points.x;
        endPointY = points.y;
        endPointZ = points.z;
    }

    void setSpeed(float _speed)
    {
        speed = _speed;
    }
}
