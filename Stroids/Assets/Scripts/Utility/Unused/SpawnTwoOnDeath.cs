using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTwoOnDeath : MonoBehaviour
{
    [SerializeField] string stroidToSpawn = "Stroid Jr";
    [SerializeField] int numberToSpawn = 2;

    private void OnDisable()
    {
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

}
