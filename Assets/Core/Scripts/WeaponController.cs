using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponController>(true);
                Page = instance.GetComponent<Page>();
            }
            return instance;
        }
    }
    private static WeaponController instance;
    public static Page Page { get; private set; }

    [field: SerializeField] private GameObject weaponPrefab;
    [field: SerializeField] private GameObject weaponSelected;

    private int currentIndex = 0;
    private int offset = 100;

    private GameObject selector;
    private int total = 0;

    public void Initialize(List<Sprite> sprites)
    {
        currentIndex = 0;
        total = sprites.Count;

        Vector3 pos = Vector3.zero;

        for (int i = 0; i < sprites.Count; ++i)
        {
            GameObject go = Instantiate(weaponPrefab, transform);

            pos.x = -(offset * (sprites.Count - 1) / 2.0f) + i * offset;
            go.transform.localPosition = pos;

            go.GetComponent<Image>().sprite = sprites[i];
        }

        selector = Instantiate(weaponSelected, transform);
        pos.x = -(offset * (sprites.Count - 1) / 2.0f);
        selector.transform.localPosition = pos;

        UIManager.Instance.PushPage(Page, true);
    }

    public void Select(int index)
    {
        if (weaponSelected != null)
        {
            Vector3 pos = Vector3.zero;
            pos.x = -(offset * (total - 1) / 2.0f) + index * offset;
            selector.transform.localPosition = pos;
        }
    }
}
