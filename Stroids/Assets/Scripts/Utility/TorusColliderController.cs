using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusColliderController : MonoBehaviour
{
    [SerializeField] GameObject map = null;
    [SerializeField] bool right = false;
    
    float xLimit;
    float yLimit;
    float xPosition;

    int mod = 1;

    // Start is called before the first frame update
    void Start()
    {
        xLimit = map.transform.localScale.x * .475f;
        yLimit = map.transform.localScale.y * .475f;

        if (right) { mod = 1; } else { mod = -1; }

        transform.position = new Vector3(xLimit * mod, 0);
        transform.localScale = new Vector3(2, yLimit *2, 1);
    }
       
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boundry collider");
        if (collision.tag == "Boss") { return; }
        if (collision.tag == "Shield") { return; }
        xPosition = collision.transform.position.x * -.95f;
        collision.transform.position = new Vector3(xPosition, collision.transform.position.y, 0);
    }
}
