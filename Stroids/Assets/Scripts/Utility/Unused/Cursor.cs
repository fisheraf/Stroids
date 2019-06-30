using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public float mouseCoordinateY;
    public float mouseCoordinateX;
    public Vector3 currentMousePositionInWorld;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseCoordinateY = Input.mousePosition.y - (Screen.height / 2);
        mouseCoordinateX = Input.mousePosition.x - (Screen.width / 2);

        currentMousePositionInWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector2(currentMousePositionInWorld.x, currentMousePositionInWorld.y);
    }
}
