using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Collider2D))]
public class ColliderDetectMouseover : MonoBehaviour
{
    Collider2D myCollider;
    RectTransform rectTransform;
    Camera mainCamera;

    public Vector2 currentMousePosition = Vector2.zero;
    public Vector2 currentMousePositionInWorld = Vector2.zero;
    public bool overLap;

    public float mouseCoordinateX = 0f;
    public float mouseCoordinateY = 0f;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        rectTransform = GetComponent<RectTransform>();
        mainCamera = FindObjectOfType<Camera>();
    }

    public bool isMouseOver()
    {
        return myCollider.OverlapPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    private void Update()
    {
        currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        currentMousePositionInWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        /*if(Input.mousePosition.y > Screen.height/2)
        {
            mouseCoordinateY = Input.mousePosition.y / 2;
        }
        else
        {
            mouseCoordinateY = Input.mousePosition.y - (Screen.width / 2);
        }

        if (Input.mousePosition.x > Screen.width / 2)
        {
            mouseCoordinateX = Input.mousePosition.x ;
        }
        else
        {
            mouseCoordinateX = Input.mousePosition.x - (Screen.width / 2);
        }*/

        mouseCoordinateY = Input.mousePosition.y - (Screen.height / 2);
        mouseCoordinateX = Input.mousePosition.x - (Screen.width / 2);

        overLap = myCollider.OverlapPoint(new Vector2(currentMousePositionInWorld.x, currentMousePositionInWorld.y));
        //myCollider.ove
    }
}