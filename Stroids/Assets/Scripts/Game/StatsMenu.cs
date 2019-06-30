using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StatsMenu : MonoBehaviour
{
    GameSession gameSession;

    [SerializeField] TextMeshProUGUI shotsFiredText = null;
    [SerializeField] TextMeshProUGUI shotsHitText = null;
    [SerializeField] TextMeshProUGUI accuracyText = null;
    [SerializeField] TextMeshProUGUI stroidsDestroyedText = null;
    [SerializeField] TextMeshProUGUI aliensDestroyedText = null;

    int shotsFired = 0;
    int shotsHit = 0;
    float accuracy = 0f;
    int stroidsDestroyed = 0;
    int aliensDestroyed = 0;

    CanvasGroup canvasGroup = null;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountStats()
    {
        shotsFired = gameSession.GetShotsFired();
        shotsFiredText.text = shotsFired.ToString();
        shotsHit = gameSession.GetShotsHit();
        shotsHitText.text = shotsHit.ToString();
        stroidsDestroyed = gameSession.GetStroidsDestroyed();
        stroidsDestroyedText.text = stroidsDestroyed.ToString();
        aliensDestroyed = gameSession.GetAliensDestroyed();
        aliensDestroyedText.text = aliensDestroyed.ToString();

        accuracy = ((float)shotsHit/shotsFired)*100;
        accuracyText.text = accuracy.ToString("F2") + "%";
    }

    public void CanvasFadeOut()  //dotween not working?  used anim(did not work) coroutine
    {
        //canvasGroup.DOFade(0f, 2f);
        Debug.Log("fade out");
        //GetComponent<Animator>().SetBool("ShowPanel", false);//Play("StatsFadeOut");        
        StartCoroutine(FadeTo(0f, 2f));
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void CanvasFadeIn()
    {
        //canvasGroup.DOFade(1f, 2f);
        Debug.Log("fade in");
        //GetComponent<Animator>().SetBool("ShowPanel", false);//.Play("StatsFadeIn");
        StartCoroutine(FadeTo(1f, 2f));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator FadeTo(float finalValue, float fadeTime)
    {
        float startValue = canvasGroup.alpha;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime/fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, finalValue, t);
            yield return null;
        }
    }
}
