using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using DG.Tweening;

public class BossScript : MonoBehaviour
{
    [SerializeField] int healthPoints = 100;
    [SerializeField] float speed = 2f;

    GameSession gameSession;
    Camera gameCamera;
    private Rigidbody2D rb;
    float gameCameraSize = 0f;
    private float xPosition;
    public float xLimit;

    public Vector2 direction = new Vector2(0, -1);

    public Vector2 positionV2;
    public Vector2 movement;
    private Vector2 targetPosition;

    [SerializeField] GameObject laserObject = null;
    //ParticleSystem laser;
    BoxCollider2D boxCollider2D;
    bool laserOn = false;
    bool hasDied = false;

    [SerializeField] Slider slider = null;
    [SerializeField] ParticleSystem explosion = null;

    IEnumerator lastRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameCamera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        //laser = GetComponentInChildren<ParticleSystem>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        //slider = FindObjectOfType<Slider>();


        gameCameraSize = gameCamera.orthographicSize;
        xLimit = gameCameraSize * gameCamera.aspect * 2f;

        lastRoutine = BossFightScript();

        StartBossFight();
    }

    // Update is called once per frame
    void Update()
    {
        //Wraparound();
        if (healthPoints <= 0 && !hasDied) { DeathSequence(); }

        /*if (Input.GetKeyDown(KeyCode.T)) { MoveLeft(); }
        if (Input.GetKeyDown(KeyCode.Y)) { MoveRight(); }
        if (Input.GetKeyDown(KeyCode.G)) { Swapsides(); }
        if (Input.GetKeyDown(KeyCode.F)) { laserOn = !laserOn; }*/

        FireLaser();

        xPosition = transform.position.x;
        positionV2 = transform.position;
        direction = targetPosition - positionV2;
        movement = new Vector2(Mathf.Clamp(direction.x, -speed, speed), Mathf.Clamp(direction.y, -speed, speed));
    }

    private void FixedUpdate()
    {
        //rb.velocity = movement;
    }

    public void StartBossFight()
    {
        StartCoroutine(lastRoutine);
        slider.gameObject.SetActive(true);
        slider.maxValue = healthPoints;
        slider.value = healthPoints;
        //targetPosition = new Vector2(0, 8);
    }

    private void MoveRight()
    {
        targetPosition = new Vector2(xLimit * 1.1f, positionV2.y);
    }

    private void MoveLeft()
    {
        targetPosition = new Vector2(-xLimit * 1.1f, positionV2.y);
    }

    private void Swapsides()
    {
        xPosition *= -1;
        transform.position = new Vector3(xPosition, transform.position.y, 0);
        Stop();
    }

    private void Stop()
    {
        targetPosition = transform.position;
        direction = Vector2.zero;
        movement = Vector2.zero;
    }

    void FireLaser()
    {
        //laserOn = !laserOn;
        //var em = laser.emission;
        //em.enabled = laserOn;
        //boxCollider2D.enabled = laserOn;
        laserObject.SetActive(laserOn);
    }

    IEnumerator LaserOn(float fireTime)
    {
        //play warmup sound then fire
        laserOn = true;
        yield return new WaitForSeconds(fireTime);
        laserOn = false;
        yield break;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player Bullet")
        {
            collision.gameObject.SetActive(false);
            healthPoints -= 1;
            slider.value = healthPoints;
            gameSession.AddToShotsHit(1);
        }
    }

    IEnumerator BossFightScript()
    {
        Tween myTween = transform.DOMoveY(8, 5); //down
        yield return myTween.WaitForCompletion();
        yield return StartCoroutine(LaserOn(3f));
        yield return new WaitForSeconds(2f);
        myTween = transform.DOMoveX(xLimit, 8); //right
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(-xLimit, 16); //left
        StartCoroutine(LaserOn(16f));
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(0, 8); //center
        yield return myTween.WaitForCompletion();
        StartCoroutine(LaserOn(100f));
        myTween = transform.DOMoveX(-xLimit, 8); //left
        yield return myTween.WaitForCompletion();
        Swapsides();
        myTween = transform.DOMoveX(-xLimit, 16); //left
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(xLimit, 16); //right
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(-xLimit, 14); //left
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(xLimit, 14); //right
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(-xLimit, 12); //left
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(xLimit, 12); //right
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(-xLimit, 10); //left
        yield return myTween.WaitForCompletion();
        myTween = transform.DOMoveX(xLimit, 10); //right
        yield return myTween.WaitForCompletion();


        /*targetPosition = new Vector2(0, 8);
        yield return new WaitForSeconds(5f);

        speed = 8f;

        MoveLeft();
        LaserOn();
        yield return new WaitForSeconds(3f);
        LaserOn();
        yield return new WaitForSeconds(2f);

        MoveRight();
        LaserOn();
        yield return new WaitForSeconds(10f);
        LaserOn();

        MoveLeft();
        yield return new WaitForSeconds(5f);

        MoveRight();
        yield return new WaitForSeconds(6f);
        Swapsides();*/

    }

    private void DeathSequence()
    {
        hasDied = true;
        StopCoroutine(lastRoutine);
        CameraShaker.Instance.ShakeOnce(10f, 2f, 2f, 2f, new Vector3(.5f, .5f, .5f), new Vector3(1f, 1f, 1f));
        explosion.Play();
        gameSession.AddToScore(6666);//score multiplier by time taken to kill
        slider.gameObject.SetActive(false);            
        gameSession.WonGame();            
      
        //main menu with start over? credits
    }
}
