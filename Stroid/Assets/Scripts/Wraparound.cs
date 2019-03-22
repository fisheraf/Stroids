using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraparound : MonoBehaviour
{
    Camera gameCamera;
    float gameCameraSize = 0f;

    private float xPosition;
    private float yPosition;
    private float xLimit;
    private float yLimit;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = FindObjectOfType<Camera>();
        gameCameraSize = gameCamera.orthographicSize;

        xLimit = gameCameraSize * gameCamera.aspect + 2;
        yLimit = gameCameraSize + 2;

        /*Debug.DrawLine(new Vector3(xLimit, yLimit, 0), new Vector3(-xLimit, yLimit, 0), Color.red, 10f, false);
        Debug.DrawLine(new Vector3(xLimit, -yLimit, 0), new Vector3(-xLimit, -yLimit, 0), Color.red, 10f, false);
        Debug.DrawLine(new Vector3(xLimit, yLimit, 0), new Vector3(xLimit, -yLimit, 0), Color.red, 10f, false);
        Debug.DrawLine(new Vector3(-xLimit, yLimit, 0), new Vector3(-xLimit, -yLimit, 0), Color.red, 10f, false);*/
    }

    // Update is called once per frame
    void Update()
    {
        xPosition = transform.position.x;
        yPosition = transform.position.y;

        if (Mathf.Abs(xPosition) > xLimit)
        {
            xPosition *= -.95f;
            transform.position = new Vector3(xPosition, transform.position.y, 0);
        }

        if (Mathf.Abs(yPosition) > yLimit)
        {
            yPosition *= -.95f;
            transform.position = new Vector3(transform.position.x, yPosition, 0);            
        }
    }
}
