using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Stroid : MonoBehaviour
{
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 5f;
    Vector3 direction = new Vector3(0, 0, 0);
    [SerializeField] float spin = 30f;
    [SerializeField] int scoreValue = 100;

    [SerializeField] AudioClip splosionSound = null;
    [SerializeField] string stroidToSpawn = "Stroid Jr";
    [SerializeField] int numberToSpawn = 2;

    GameSession gameSession;

    private void Start()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector3(x, y, 0);
        direction = direction.normalized * Random.Range(minSpeed, maxSpeed);

        gameSession = FindObjectOfType<GameSession>();
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * Time.timeScale, Space.World);
        transform.Rotate(0, 0, -spin * Time.deltaTime * Time.timeScale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player Bullet")
        {
            gameSession.AddToScore(scoreValue);
            GameObject splosion = ObjectPooler.SharedInstance.GetPooledObject("Splosion");
            if (splosion != null)
            {
                splosion.transform.position = transform.position;
                splosion.transform.rotation = transform.rotation;
                splosion.SetActive(true);
            }
            AudioSource.PlayClipAtPoint(splosionSound, transform.position);
            gameObject.SetActive(false);
            CameraShaker.Instance.ShakeOnce(4f, 1f, .1f, .1f);
            gameSession.NumberOfEnemies(-1);
            collision.gameObject.SetActive(false);
            Spawn(numberToSpawn);
            gameSession.NumberOfEnemies(numberToSpawn);
        }
    }

    private void Spawn(int numberToSpawn)
    {
        if(numberToSpawn == 0) { return; }

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject stroid = ObjectPooler.SharedInstance.GetPooledObject(stroidToSpawn);
            if (stroid != null)
            {
                stroid.transform.position = transform.position;
                stroid.transform.rotation = transform.rotation;
                stroid.SetActive(true);
            }
        }
    }



    /*[SerializeField] Vector2 movement = new Vector2(5f, 20f);
    [SerializeField] bool isMoving = true;
    [SerializeField] float spin = 30f;
    [SerializeField] bool isSpinning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.Translate(new Vector3(movement.x * Time.deltaTime, movement.y * Time.deltaTime, 0), Space.World);
        }
        if(isSpinning)
        {
            transform.Rotate(0, 0, -spin * Time.deltaTime);
        }
    }*/
}
