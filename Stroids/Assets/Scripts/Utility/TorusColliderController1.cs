using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusColliderController1 : MonoBehaviour
{
    [SerializeField] GameObject map = null;
    [SerializeField] bool top = false;
    
    float xLimit;
    float yLimit;
    float yPosition;

    int mod = 1;

    // Start is called before the first frame update
    void Start()
    {
        xLimit = map.transform.localScale.x * .475f;
        yLimit = map.transform.localScale.y * .475f;

        if (top) { mod = 1; } else { mod = -1; }

        transform.position = new Vector3(0, yLimit * mod);
        transform.localScale = new Vector3(xLimit * 2, 2, 1);
    }
       
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boundry collider");
        if (collision.tag == "Boss") { return; }
        yPosition = collision.transform.position.y * -.95f;
        collision.transform.position = new Vector3(collision.transform.position.x, yPosition, 0);
    }
}
