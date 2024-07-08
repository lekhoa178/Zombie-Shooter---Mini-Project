using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class AnimationHelper
{
    public static IEnumerator SlideIn(RectTransform rectTransform, Direction direction, float speed, UnityEvent? onEnd)
    {
        Vector2 startPosition = Vector2.zero;
        switch (direction)
        {
            case Direction.UP:
                startPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.RIGHT:
                startPosition = new Vector2(-Screen.width, 0);
                break;
            case Direction.DOWN:
                startPosition = new Vector2(0, Screen.height);
                break;
            case Direction.LEFT:
                startPosition = new Vector2(Screen.width, 0);
                break;
        }

        float time = 0;
        while (time < 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        onEnd?.Invoke();
    }

    public static IEnumerator SlideOut(RectTransform rectTransform, Direction direction, float speed, UnityEvent? onEnd)
    {
        Vector2 endPosition = Vector2.zero;
        switch (direction)
        {
            case Direction.UP:
                endPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.RIGHT:
                endPosition = new Vector2(-Screen.width, 0);
                break;
            case Direction.DOWN:
                endPosition = new Vector2(0, Screen.height);
                break;
            case Direction.LEFT:
                endPosition = new Vector2(Screen.width, 0);
                break;
        }

        float time = 0;
        while (time < 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        onEnd?.Invoke();
        rectTransform.gameObject.SetActive(false);
    }

    public static IEnumerator ZoomIn(RectTransform rectTransform, float speed, UnityEvent? onEnd)
    {
        float time = 0;
        while (time < 1)
        {
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one,time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        rectTransform.localScale = Vector3.one;

        onEnd?.Invoke();
    }

    public static IEnumerator ZoomOut(RectTransform rectTransform, float speed, UnityEvent? onEnd)
    {
        float time = 0;
        while (time < 1)
        {
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        rectTransform.localScale = Vector3.zero;

        onEnd?.Invoke();
        rectTransform.gameObject.SetActive(false);
    }

    public static IEnumerator FadeIn(CanvasGroup canvasGroup, float speed, UnityEvent? onEnd)
    {
        float time = 0;
        while (time < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        canvasGroup.alpha = 1;

        onEnd?.Invoke();
    }

    public static IEnumerator FadeOut(CanvasGroup canvasGroup, float speed, UnityEvent? onEnd)
    {
        float time = 0;
        while (time < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        canvasGroup.alpha = 0;

        onEnd?.Invoke();
        canvasGroup.gameObject.SetActive(false);
    }
}
