using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float speedBoost;
    [SerializeField] float turnBoost;
    [SerializeField] float driftBoost;

    [SerializeField] float fireRateBoost;
    [SerializeField] float bulletSpeedBoost;
    [SerializeField] float bulletTimeBoost;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //FindObjectOfType<Player>().
        Destroy(gameObject);
    }
}
