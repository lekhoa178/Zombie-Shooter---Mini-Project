using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public static EnemyHealthController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyHealthController>(true);
                Page = instance.GetComponent<Page>();
            }
            return instance;
        }
    }
    private static EnemyHealthController instance;
    public static Page Page { get; private set; }

    [field: SerializeField] public Transform parent { get; private set; }

    [field: SerializeField] private Image hpImage;
    [field: SerializeField] private Image hpDelayImage;
    [field: SerializeField] private TextMeshProUGUI hpTMPs;

    private bool isActive;

    private float targetRatio;
    private Coroutine delayCoroutine;

    private void Awake()
    {
        parent = transform;
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        if (health == 0)
        {
            UIManager.Instance.PopPage(Page);
            isActive = false;
            hpDelayImage.fillAmount = 1;

            return;
        }

        if (!isActive)
        {
            UIManager.Instance.PushPage(Page, true);
        }

        isActive = true;

        DrawHealth(health, maxHealth);
    }

    private void DrawHealth(float health, float maxHealth)
    {
        if (delayCoroutine != null)
        {
            hpDelayImage.fillAmount = targetRatio;
            StopCoroutine(delayCoroutine);
        }

        float ratio = health / maxHealth;
        targetRatio = ratio;

        hpImage.fillAmount = ratio;
        hpTMPs.text = ((int)(ratio * 100)).ToString();
        delayCoroutine = StartCoroutine(DrawDelayCoroutine());
    }

    private IEnumerator DrawDelayCoroutine()
    {
        float lerp = 0;

        while (lerp < 1)
        {
            float lerpAmount = Mathf.Lerp(hpDelayImage.fillAmount, targetRatio, lerp);
            hpDelayImage.fillAmount = lerpAmount;

            lerp += Time.deltaTime * 0.2f;

            yield return null;
        }
    }
}
