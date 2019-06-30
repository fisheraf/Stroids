using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraparoundTwo : MonoBehaviour
{
    [SerializeField] bool top = false;
    [SerializeField] float buffer = 3f;
    Camera gameCamera;
    float gameCameraSize = 0f;

    private float yPosition;
    private float xLimit;
    private float yLimit;

    int mod = 1;

    private void Start()
    {
        gameCamera = FindObjectOfType<Camera>();
        gameCameraSize = gameCamera.orthographicSize;

        xLimit = gameCameraSize * gameCamera.aspect + buffer;
        yLimit = gameCameraSize + buffer;

        if(top) { mod = 1; } else { mod = -1; }

        transform.position = new Vector3(0, yLimit * mod, 0);
        transform.localScale = new Vector3(xLimit * 2, 2, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Boss") { return; }
        yPosition = collision.transform.position.y * -.95f;
        collision.transform.position = new Vector3(collision.transform.position.x, yPosition, 0);
    }

    public float GetXLimit()
    {
        return xLimit;
    }

}
