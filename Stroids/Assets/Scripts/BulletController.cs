using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject bulletObject = null;
    Bullet bullet = null;

    // Start is called before the first frame update
    void Start()
    {
        bullet = bulletObject.GetComponent<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("9"))
        { AddBulletSpeed(); Debug.Log("bulletspeedincrease"); }
    }

    private void AddBulletSpeed()
    {
        bullet.ChangeSpeed(-1);
    }
}
