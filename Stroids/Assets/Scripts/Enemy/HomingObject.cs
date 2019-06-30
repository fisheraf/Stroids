using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingObject : MonoBehaviour
{
    [SerializeField] Transform target = null;

    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float rotateSpeed = 200f;

    [SerializeField] bool continuousFollow = true;
    [SerializeField] float movementTime = 3f;
    [SerializeField] float waitTime = 3f;

    [SerializeField] bool onlyFollowInRange = false;
    [SerializeField] float attackRange = 5f;

    [SerializeField] bool fleeing = false;

    private Rigidbody2D rb;
    private float rotateAmount;
    private Vector2 targetDirection;
    private bool move = true;
    private bool inRange = false;

    private float speed;

    //private float angle;
    //private float rotationZ;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 heading = target.position - transform.position;

        if (heading.sqrMagnitude < attackRange * attackRange)
        {
            inRange = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onlyFollowInRange && !inRange) { return; }

        targetDirection = (Vector2)target.position - rb.position;
        targetDirection.Normalize();
        //rotationZ = (Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg) - 90;
        //angle = Vector3.Angle(rb.position, target.position);

        if (fleeing) { targetDirection *= -1; }

        rotateAmount = Vector3.Cross(targetDirection, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        
        if (continuousFollow)
        {
            if (Mathf.Abs(rotateAmount) > .7)
            {
                DOTween.To(() => speed, x => speed = x, maxSpeed / 3, 2);
                rb.velocity = transform.up * speed;
            }
            else
            {
                DOTween.To(() => speed, x => speed = x, maxSpeed, 1);
                rb.velocity = transform.up * speed;
            }

            //rb.angularVelocity = -rotateAmount * rotateSpeed;
        }
        else
        {
            if (move)
            {
                StartCoroutine(PeriodicMovement());
            }
        }
    }

    IEnumerator PeriodicMovement()
    {
        move = false;
        rb.velocity = transform.up * maxSpeed;
        rb.freezeRotation = true;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(movementTime);
        rb.velocity = Vector2.zero;
        //rb.DORotate(rotationZ, waitTime);
        rb.freezeRotation = false;
        yield return new WaitForSeconds(waitTime);
        move = true;
    }
}
