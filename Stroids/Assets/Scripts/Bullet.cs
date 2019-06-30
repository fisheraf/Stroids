using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Bullet : MonoBehaviour
{
    //[SerializeField] Player player = null;

    [SerializeField] float existanceTime = 1.8f;
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] LayerMask collisionMask;

    private void Start()
    {
        //player = FindObjectOfType<Player>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //GetStats();
        StartCoroutine(Timer(existanceTime));
    }

    public void GetStats()
    {
        //existanceTime = player.GetBulletLifetime();
        //bulletSpeed = player.GetBulletSpeed();
    }

    public void ChangeSpeed(float change)
    {
        bulletSpeed *= change;
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        //gameObject.SetActive(false); not using pool with different bullet stats
        Destroy(gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, bulletSpeed * Time.deltaTime * Time.timeScale, 0), Space.Self);
        //Richochet(); //Thanks to Sebastian Lague
    }

    /*private void Richochet()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        /*if (Physics.Raycast(ray, out hit, Time.deltaTime * bulletSpeed + .5f, 1 << LayerMask.NameToLayer("Reflective")))
        {
            Vector3 reflectDirection = Vector3.Reflect(ray.direction, hit.normal);
            float rotation = 90 - Mathf.Atan2(reflectDirection.z, reflectDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rotation, 0);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Reflective")
        {
            Vector3 reflectDirection = Vector3.Reflect(transform.up, collision.contacts[0].normal);
            float rotation = 90 - Mathf.Atan2(reflectDirection.z, reflectDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, rotation);
        }
        else { return; }
    }
}
