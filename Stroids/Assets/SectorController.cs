using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SectorController : MonoBehaviour
{
    Player player;

    [SerializeField] TextMeshProUGUI coordinatesText = null;
    [SerializeField] float gameWidth = 200f;
    //[SerializeField] float numberOfSectors = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        coordinatesText = GetComponent<TextMeshProUGUI>();

        //float x = gameWidth / numberOfSectors;
    }

    // Update is called once per frame
    void Update()
    {
        //if((player.transform.position.x + (gameWidth/2) < x)

        int x = Mathf.RoundToInt(player.transform.position.x + gameWidth / 2);
        int y = Mathf.RoundToInt(player.transform.position.y + gameWidth / 2);

        coordinatesText.text =  x.ToString() + "," + y.ToString();
    }
}
