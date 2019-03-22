using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Bullet : MonoBehaviour
{
    [SerializeField] float existanceTime = 1f;
    [SerializeField] float bulletSpeed = 2f;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Timer(existanceTime));
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, bulletSpeed * Time.deltaTime * Time.timeScale, 0), Space.Self);
    }
}
