using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] float mapHeight = 0f;
    [SerializeField] float mapWidth = 0f;

    [SerializeField] int numberOfPlanetsToSpawn = 5;

    //[SerializeField] GameObject[] planet = null;
    [SerializeField] GameObject planet2 = null;
    [SerializeField] Sprite[] planetSprites = null;
    [SerializeField] float spawnSpacing = 10f;

    public float xLoc = 0f;
    public float yLoc = 0f;
    public Vector2 location;

    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawLine(new Vector3(mapWidth, mapHeight), new Vector3(-mapWidth, mapHeight), Color.green, 120f);
        Debug.DrawLine(new Vector3(mapWidth, -mapHeight), new Vector3(-mapWidth, -mapHeight), Color.green, 120f);
        Debug.DrawLine(new Vector3(mapWidth, mapHeight), new Vector3(mapWidth, -mapHeight), Color.green, 120f);
        Debug.DrawLine(new Vector3(-mapWidth, mapHeight), new Vector3(-mapWidth, -mapHeight), Color.green, 120f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
            { Spawn(); }
    }

    //int j = 0;

    void Spawn()
    {
        for (int j = 0; j < numberOfPlanetsToSpawn; j++)
        {
            FindLocation();
            if (CheckIfEmpty() == false)
            {
                for (int i = 0; i < 15; i++)
                {
                    Debug.Log("occupied");
                    FindLocation();
                    if (CheckIfEmpty() == true) { break; }
                    if (i == 14) { Debug.Log("too many tries"); return; }
                }
            }

            Debug.Log("spawnWorld");
            //j = Random.Range(0, planet.Length);
            //Instantiate(planet[j], location, Quaternion.identity);
            RandomPlanetSpawn();
            //j++;
        }

    }

    private void RandomPlanetSpawn()
    {
        //if (j >= numberOfPlanetsToSpawn) { return; }
        GameObject p = Instantiate(planet2, location, Quaternion.identity) as GameObject;
        float randomScale = Random.Range(100f, 200f);
        p.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        SpriteRenderer spriteRenderer = p.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Random.ColorHSV();
        spriteRenderer.sprite = planetSprites[Random.Range(0, planetSprites.Length)];
        //random rotation(scroll)
    }

    bool CheckIfEmpty()
    {
        if (Physics2D.OverlapCircle(location, spawnSpacing))
        {
            return false;
        }
        else return true;
    }

    private void FindLocation()
    {
        xLoc = UnityEngine.Random.Range(-mapWidth, mapWidth);
        yLoc = UnityEngine.Random.Range(-mapHeight, mapHeight);
        location = new Vector2(xLoc, yLoc);
    }
}
