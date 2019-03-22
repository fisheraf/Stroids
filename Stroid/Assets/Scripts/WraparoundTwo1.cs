using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraparoundTwo1 : MonoBehaviour
{
    [SerializeField] bool right = false;
    [SerializeField] float buffer = 3f;
    Camera gameCamera;
    float gameCameraSize = 0f;

    private float xPosition;
    private float xLimit;
    private float yLimit;

    int mod = 1;

    private void Start()
    {
        gameCamera = FindObjectOfType<Camera>();
        gameCameraSize = gameCamera.orthographicSize;

        xLimit = gameCameraSize * gameCamera.aspect + buffer;
        yLimit = gameCameraSize + buffer;

        if (right) { mod = 1; } else { mod = -1; }

        transform.position = new Vector3(xLimit * mod, 0, 0);
        transform.localScale = new Vector3(2, yLimit * 2, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss") { return; }
        xPosition = collision.transform.position.x * -.95f;
        collision.transform.position = new Vector3(xPosition, collision.transform.position.y, 0);
    }

}
