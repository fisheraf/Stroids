using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class AlienShip : MonoBehaviour
{
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 5f;
    Vector3 direction = new Vector3(0, 0, 0);
    [SerializeField] int scoreValue = 100;

    [SerializeField] float attackRange = 10f;
    [SerializeField] float reloadTime = 3f;
    public Vector3 heading;
    public float bulletHeading;

    public GameObject bullet;
    public float lastTimeShot;
    public bool attacking = false;

    [SerializeField] AudioSource audioSourceShoot = null;
    [SerializeField] AudioClip deathSound = null;

    GameSession gameSession;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        float x = 0;
        float y = 0;
        attacking = false;
        if(transform.position.x > 0)
        {
            x = Random.Range(-1f, -.5f);
        }
        else
        {
            x = Random.Range(.5f, 1f);
        }

        if (transform.position.y > 0)
        {
            y = Random.Range(-.2f, 0f);
        }
        else
        {
            y = Random.Range(0f, .2f);
        }
        direction = new Vector3(x, y, 0);
        direction = direction.normalized * Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * Time.timeScale, Space.World);
        heading = player.transform.position - transform.position;
        bulletHeading = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg - 90f;

        if (heading.sqrMagnitude < attackRange * attackRange && !attacking)
        {
            StartCoroutine(Shoot(reloadTime));
            attacking = true;            
        }
        
        /*if(Time.time > lastTimeShot + reloadTime && heading.sqrMagnitude < attackRange * attackRange)
        {
            //GameObject newbullet = Instantiate(bullet, transform.position, Quaternion.AngleAxis(bulletHeading, Vector3.forward));
            //Shoot();
            Debug.Log("Update shoot");
            lastTimeShot = Time.time;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player Bullet")
        {
            gameSession.AddToScore(scoreValue);
            GameObject splosion = ObjectPooler.SharedInstance.GetPooledObject("Splosion");
            if (splosion != null)
            {
                splosion.transform.position = transform.position;
                splosion.transform.rotation = transform.rotation;
                splosion.SetActive(true);
            }
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            gameObject.SetActive(false);
            CameraShaker.Instance.ShakeOnce(5f, 1f, .1f, .1f);
            gameSession.NumberOfEnemies(-1);
            collision.gameObject.SetActive(false);
            gameSession.AddToShotsHit(1);
            gameSession.AddToAliensDestroyed(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject splosion = ObjectPooler.SharedInstance.GetPooledObject("Splosion");
            if (splosion != null)
            {
                splosion.transform.position = transform.position;
                splosion.transform.rotation = transform.rotation;
                splosion.SetActive(true);
            }
            gameObject.SetActive(false);
            gameSession.NumberOfEnemies(-1);
        }

    }

    private IEnumerator Shoot(float reloadTime)
    {
        Shoot();
        audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
        yield return new WaitForSeconds(.05f);
        Shoot();
        yield return new WaitForSeconds(reloadTime);
        attacking = false;
        yield break;
        /*GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Enemy Bullet");
        if (bullet != null)
        {


            //audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
            yield return new WaitForSeconds(reloadTime);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.AngleAxis(bulletHeading, Vector3.forward);//heading.normalized);
            
        }*/

    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Enemy Bullet");
        if (bullet != null)
        {
            //audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.AngleAxis(bulletHeading, Vector3.forward);//heading.normalized);
        }
    }
}
