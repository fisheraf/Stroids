using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText = null;
    //[SerializeField] TextMeshProUGUI livesText = null;//images?
    public Image[] lives;
    [SerializeField] TextMeshProUGUI finalScoreText = null;
    [SerializeField] GameObject PauseMenuUI = null;
    [SerializeField] GameObject WinMenuUI = null;
    Player player;
    StroidSpawner stroidSpawner;
    AlienSpawner alienSpawner;
    [SerializeField] GameObject boss = null;

    public static bool gameIsPaused = true;

    //Collider2D cursorCollider = null;
    Button optionsButton = null;

    [SerializeField] int numberOfEnemies = 0;

    [SerializeField] AudioSource menuMusic = null;
    float audioLevel;

    int currentScore = 0;
    int currentLives = 3;

    int shotsFired = 0;
    int shotsHit = 0;
    int stroidsDestroyed = 0;
    int aliensDestroyed = 0;

    float timescale = 0f;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.enabled = true;
        //livesText.enabled = true;
        scoreText.text = currentScore.ToString();
        //livesText.text = currentLives.ToString();
        UpdateLives();
        player = FindObjectOfType<Player>();
        stroidSpawner = FindObjectOfType<StroidSpawner>();
        alienSpawner = FindObjectOfType<AlienSpawner>();
        audioLevel = menuMusic.volume;
        menuMusic.Play();
        WinMenuUI.SetActive(false);

        
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        timescale = Time.timeScale;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(Input.GetKeyDown("z"))
        {
            WonGame();
        }
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        player.PauseGame(true);
        player.gameObject.SetActive(false);
        stroidSpawner.PauseGame(true);
        alienSpawner.PauseGame(true);
        Time.timeScale = 0f;
        gameIsPaused = true;               
        StartCoroutine(FadeAudio(audioLevel, 2f, menuMusic));
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        player.PauseGame(false);
        player.gameObject.SetActive(true);
        stroidSpawner.PauseGame(false);
        alienSpawner.PauseGame(false);
        Time.timeScale = 1f;
        gameIsPaused = false;        
        StartCoroutine(FadeAudio(0f, 2f, menuMusic));
    }

    public void Options()
    {
        optionsButton = GameObject.Find("Options").GetComponent<Button>();
        GameObject.Find("Options Text").GetComponent<TextMeshProUGUI>().text =  ("TBA.");
    }

    public void Exit()
    {
        Application.Quit();
        optionsButton = GameObject.Find("Exit").GetComponent<Button>();
        GameObject.Find("Exit Text").GetComponent<TextMeshProUGUI>().text = ("Not available in HTML5.");
    }

    public IEnumerator FadeAudio(float targetVolume, float fadeTime, AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        if(targetVolume < startVolume)
        {
            while (audioSource.volume > targetVolume)
            {
                audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;
                yield return null;
            }
        }
        else if(targetVolume > startVolume)
        {
            while (audioSource.volume < targetVolume)
            {
                audioSource.volume += targetVolume * Time.unscaledDeltaTime / fadeTime;
                yield return null;
            }
        }
    }

    public void AddToScore(int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();
    }

    public int GetScore()
    {  return currentScore; }

    public int NumberOfEnemies(int enemyChange)
    {
        numberOfEnemies += enemyChange;
        return numberOfEnemies;
    }

    public void AddToShotsFired(int shotFired)
    {
        shotsFired += shotFired;
    }

    public int GetShotsFired()
    { return shotsFired; }

    public void AddToShotsHit(int shotHit)
    {
        shotsHit += shotHit;
    }

    public int GetShotsHit()
    { return shotsHit; }

    public void AddToStroidsDestroyed(int stroidDestroyed)
    {
        stroidsDestroyed += stroidDestroyed;
    }

    public int GetStroidsDestroyed()
    { return stroidsDestroyed; }

    public void AddToAliensDestroyed(int alienDestroyed)
    {
        aliensDestroyed += alienDestroyed;
    }

    public int GetAliensDestroyed()
    { return aliensDestroyed; }

    public void TakeLife()
    {
        if(currentLives > 0)
        {
            currentLives--;
            //livesText.text = currentLives.ToString();
            UpdateLives();
        }
        else if(currentLives <=0)
        {
            //end game
            Debug.Log("Dead");
            currentLives = 3;
            currentScore = 0;
            scoreText.text = currentScore.ToString();
            //livesText.text = currentLives.ToString();
            UpdateLives();
        }
    }

    void UpdateLives()
    {
        for(int i = 0; i < lives.Length; i++)
        {
            if(i < currentLives)
            {
                //lives[i].enabled = true;
                lives[i].DOFade(1, 2);
            }
            else
            {
                //lives[i].enabled = false;
                lives[i].DOFade(0, 2);
            }
        }

    }

    public void WonGame()
    {
        StartCoroutine(WonGameCoRo());
    }

    IEnumerator WonGameCoRo()
    {        
        yield return new WaitForSeconds(2f);
        boss.SetActive(false);
        Pause();
        PauseMenuUI.SetActive(false);
        WinMenuUI.SetActive(true);
        scoreText.enabled = false;
        //livesText.enabled = false;
        UpdateLives();
        finalScoreText.text = "Final score:\n" + currentScore.ToString();
    }

    Transform pool;

    public void PlayAgain()
    {        
        WinMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        pool = GameObject.Find("Pool").GetComponent<Transform>();
        foreach (Transform enemy in pool)
        {
            enemy.gameObject.SetActive(false);
        }
        //Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }
}
