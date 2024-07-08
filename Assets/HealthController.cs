using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HealthController : MonoBehaviour
{
    public static HealthController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<HealthController>();

            return instance;
        }
    }

    private static HealthController instance;

    [field: SerializeField] private List<Sprite> heartSprites;
    [field: SerializeField] private GameObject heartPrefab;

    private List<GameObject> hearts;

    public void Initialize(float health)
    {
        if (hearts != null)
        {
            foreach(GameObject heart in hearts)
            {
                Destroy(heart);
            }
        }

        hearts = new List<GameObject>();

        int[] meta = CalculateMeta(health, health);
        DrawHeart(meta);
    }

    private void DrawHeart(int[] meta, bool update = false)
    {
        for (int i = 0; i < meta.Length; ++i)
        {
            GameObject go;
            if (update)
                go = hearts[i];
            else
            {
                go = Instantiate(heartPrefab, transform, false);

                Vector3 pos = (go.transform as RectTransform).anchoredPosition;
                pos.x = 30 + i * 80;
                (go.transform as RectTransform).anchoredPosition = pos;

            }

            switch (meta[i])
            {
                case 1:
                    go.GetComponent<Image>().sprite = heartSprites[1];
                    break;
                case 2:
                    go.GetComponent<Image>().sprite = heartSprites[2];
                    break;
                case 0:
                    go.GetComponent<Image>().sprite = heartSprites[0];
                    break;
            }

            hearts.Add(go);
        }
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        int[] curMeta = CalculateMeta(health, maxHealth);
        DrawHeart(curMeta, true);
    }

    private int[] CalculateMeta(float health, float maxHealth)
    {
        int[] meta = new int[(int)Mathf.Ceil(maxHealth)];
        int fullHeart = Mathf.FloorToInt(health);
        float remainHeart = health % 1;

        int nextIdx = fullHeart;

        for (int i = 0; i < fullHeart; ++i)
        {
            meta[i] = 2;
        }

        if (remainHeart != 0)
        {
            meta[fullHeart] = 1;
            nextIdx += 1;
        }

        for (int i = nextIdx; i < meta.Length; ++i)
        {
            meta[i] = 0;
        }

        return meta;
    }

}
