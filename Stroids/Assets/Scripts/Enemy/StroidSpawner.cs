using System.Collections;
using UnityEngine;


public class StroidSpawner : MonoBehaviour
{
    public enum SpawningState { Spawning, Waiting, Counting };

    [System.Serializable]
    public class EnemyWave
    {
        //[SerializeField] string enemyName;
        [SerializeField] GameObject enemyToSpawn;//not set on line 152
        public int numberToSpawn = 10;
        public bool randomNumber = false;
        public float minTimeBetweenSpawn = 1;
        public float maxTimeBetweenSpawn = 10;
    }

    [SerializeField] EnemyWave[] enemyWaves = null;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    [SerializeField] Transform[] spawnPoints = null;
    [SerializeField] float timeBetweenWaves = 3f;
    private float waveCountdown = 0f;

    bool gameIsPaused = true;
    private float searchCountdown = 1f;
    private SpawningState state = SpawningState.Counting;
    public SpawningState State
    {
        get { return state; }
    }

    [SerializeField] GameObject boss = null;

    GameSession gameSession = null;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points selected.");
        }
        waveCountdown = timeBetweenWaves;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SpawnStroids(enemyWaves[0]);
        //SpawnStroids(numberToSpawn, minTimeBetweenSpawn, maxTimeBetweenSpawn);   
    }

    public void PauseGame(bool paused)
    {
        gameIsPaused = paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawningState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawningState.Spawning)
            {
                StartCoroutine(Spawn(enemyWaves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime * Time.timeScale;
        }

    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawningState.Counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > enemyWaves.Length - 1)
        {
            //nextWave = 0;
            //Debug.Log("ALL WAVES COMPLETE! Looping...");
            boss.SetActive(true);
            FindObjectOfType<AlienSpawner>().ChangeSpawnRate(.5f);
            //boss.GetComponent<BossScript>().StartBossFight(); //called by start already
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (gameSession.NumberOfEnemies(0) <= 0)
            {
                return false;                
            }
        }
        return true;
    }


    /*public void SpawnStroids(EnemyWave enemyWave)
    {
        numberToSpawn = enemyWave
        StartCoroutine(Spawn(numberToSpawn, minTimeBetweenSpawn, maxTimeBetweenSpawn));
    }*/

    IEnumerator Spawn(EnemyWave enemyWave)
    {
        state = SpawningState.Spawning;
        int numberSpawned = 0;
        int numberToSpawn = 0;
        if(enemyWave.randomNumber)
        {
            numberToSpawn = Random.Range(1, enemyWave.numberToSpawn);
        }
        else
        {
             numberToSpawn = enemyWave.numberToSpawn;
        }
        while (numberSpawned < numberToSpawn)
        {
            yield return new WaitUntil(() => !gameIsPaused);
            yield return new WaitForSeconds(Random.Range(enemyWave.minTimeBetweenSpawn, enemyWave.maxTimeBetweenSpawn));
            GameObject stroid = ObjectPooler.SharedInstance.GetPooledObject("Stroid");//make pull from variable
            if (stroid != null)
            {
                //Debug.Log("Spawn");
                stroid.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                stroid.transform.rotation = transform.rotation;
                stroid.SetActive(true);
            }
            numberSpawned++;
            gameSession.NumberOfEnemies(1);
            
        }
        state = SpawningState.Waiting;
        yield break;
    }
}
