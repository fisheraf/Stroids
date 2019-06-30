using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrowOnMouseOver : MonoBehaviour
{
    [SerializeField] float fadeInTime = 0f;
    [SerializeField] float fadeOutTime = 0f;
    [SerializeField] Vector3 targetSize = Vector3.zero;
    RectTransform rectTransform = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseEnter()
    {
        rectTransform.DOScale(2f, 1f);
        Debug.Log("mouseover");
    }

    private void OnMouseExit()
    {
        rectTransform.DOScale(1f, 1f);
        Debug.Log("mouseexit");
    }

    Coroutine co;

    public void Grow()
    {
        if (co != null)
            { StopCoroutine(co); }
        co = StartCoroutine(GrowTo(targetSize, fadeInTime));
        Debug.Log("mouseover");
    }

    public void Shrink()
    {
        //snap closed?
        if (co != null)
            { StopCoroutine(co); }
        //rectTransform.localScale = Vector3.one;
        co = StartCoroutine(GrowTo(new Vector3(1f, 1f), fadeOutTime));
        Debug.Log("mouseexit");
    }

    IEnumerator GrowTo(Vector3 targetSize, float fadeTime)
    {
        Vector3 startValue = rectTransform.localScale;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / fadeTime)
        {
            rectTransform.localScale = Vector3.Lerp(startValue, targetSize, t);
            yield return null;
            if(t > 1)
            {
                rectTransform.localScale = targetSize;
            }
        }
    }
}
