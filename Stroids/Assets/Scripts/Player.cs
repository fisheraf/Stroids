using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10;
    [SerializeField] float rotationSpeed = 150;
    [SerializeField] bool holdDownToFire = false;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] bool autoFire = false;
    //[SerializeField] float bulletExistanceTime = 1f;
    //[SerializeField] float bulletSpeed = 2f;
    [SerializeField] GameObject bullet = null;

    [SerializeField] AudioSource audioSourceShoot = null;
    [SerializeField] AudioSource audioSourceThrust = null;
    [SerializeField] AudioSource audioSourceDeath = null;
    //[SerializeField] AudioClip shootSound;
    //SerializeField] AudioClip thrustSound;

    private Rigidbody2D playerRigidbody;
    private float spawnInvincibleTime = 2f;

    bool isInvincible = false;
    bool isReloading = false;
    bool gameIsPaused = true;

    GameSession gameSession;
    [SerializeField] ParticleSystem thruster = null;
    [SerializeField] ParticleSystem explosion = null;
    [SerializeField] GameObject shieldGFX = null;
    [SerializeField] Transform laserPosition = null;

    // Start is called before the first frame update
    private void Start()
    {
        //SpawnPlayer();
    }
    void OnEnable()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        gameSession = FindObjectOfType<GameSession>();        
        thruster.Play();
        //SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ShootingController();
    }

    public void PauseGame(bool paused)
    {
        gameIsPaused = paused;
    }

    private void Movement()
    {
        if(gameIsPaused) { return; }
        float translation = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        //forward movement only
        if(translation > 0)
        {
            playerRigidbody.AddRelativeForce(Vector2.up * movementSpeed, ForceMode2D.Impulse);
            var em = thruster.emission;
            em.enabled = true;
            if(!audioSourceThrust.isPlaying)
            {
                audioSourceThrust.Play();
            }
            //thruster.Play();
        }
        else
        {
            var em = thruster.emission;
            em.enabled = false;
            audioSourceThrust.Stop();
        }

        transform.Rotate(0, 0, -rotation);
    }

    private void ShootingController()
    {
        if (gameIsPaused || isInvincible) { return; }
        if (holdDownToFire)
        {
            if (Input.GetButton("Fire1"))
            {
                if (isReloading) { return; }
                /*GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Player Bullet");
                if (bullet != null)
                {
                    audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
                    bullet.transform.position = laserPosition.position; //add spot to fire from(turret)
                    bullet.transform.rotation = transform.rotation;                
                    bullet.SetActive(true);
                    gameSession.AddToShotsFired(1);
                }*/
                Shoot();
                StartCoroutine(Reload());

            }
        }
        else if (!holdDownToFire)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        if(autoFire)
        {
            if (isReloading) { return; }
            Shoot();
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, laserPosition.position, laserPosition.rotation);
        audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
        gameSession.AddToShotsFired(1);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isInvincible) { return; }  //auto death unless collides with safe object?
        if(collision.gameObject.tag == "Enemy")//Stroid
        {
            Death();
        }
        if (collision.gameObject.tag == "Stroid Jr")
        {
            Death();
        }
        if (collision.gameObject.tag == "Stroid Tiny")
        {
            Death();
        }
        if (collision.gameObject.tag == "Boss")
        {
            Death();
        }
        if (collision.gameObject.tag == "Boss Laser")
        {
            Death();
        }
        if (collision.gameObject.tag == "Enemy Bullet")
        {
            Death();
        }
        if (collision.gameObject.tag == "Alien Ship")
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) { return; }
        if (collision.tag == "Enemy Bullet")
        {
            collision.gameObject.SetActive(false);
            Death();
        }
        if (collision.tag == "Boss")
        {
            collision.gameObject.SetActive(false);
            Death();
        }
    }

    /*private void OnParticleCollision(GameObject other)
    {
        if (isInvincible) { return; }
        Debug.Log("Particle collisison.");
        Death();
    }*/


    public void Death()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        Debug.Log("Dead");
        audioSourceDeath.PlayOneShot(audioSourceDeath.clip);
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        CameraShaker.Instance.ShakeOnce(10f, 2f, 2f, 2f, new Vector3(.5f, .5f, .5f), new Vector3(1f, 1f, 1f));
        explosion.Play();
        gameSession.TakeLife();
        isInvincible = true;
        yield return new WaitForSeconds(2f);              
        SpawnPlayer();
        yield break;
    }

    public void SpawnPlayer()
    {
        StartCoroutine(IsInvincible(spawnInvincibleTime));
        transform.position = Vector3.zero;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.identity;
        playerRigidbody.constraints = RigidbodyConstraints2D.None;
        playerRigidbody.velocity = Vector3.zero;
    }

    IEnumerator IsInvincible(float timeInvincible)
    {
        isInvincible = true;//show shield
        shieldGFX.SetActive(true);
        shieldGFX.transform.position = transform.position;
        yield return new WaitForSeconds(timeInvincible);
        isInvincible = false;
        shieldGFX.SetActive(false);
    }

    /*public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public float GetBulletLifetime()
    {
        return bulletExistanceTime;
    }*/
}
