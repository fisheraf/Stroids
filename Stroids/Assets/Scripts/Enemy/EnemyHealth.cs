using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int healthPoints = 100;
    [SerializeField] int scoreValue = 100;

    GameSession gameSession;

    [SerializeField] AudioClip deathSound = null;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player Bullet")
        {
            healthPoints -= 1;
            gameSession.AddToShotsHit(1);

            if(healthPoints <= 0)
            {
                DeathSequence(collision);
            }
        }
    }

    private void DeathSequence(Collider2D collision)
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
        gameSession.AddToAliensDestroyed(1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
}
