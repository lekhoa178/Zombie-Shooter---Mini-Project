using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    private bool entered;

    private void OnTriggerEnter(Collider other)
    {
        if (!LevelManager.Instance.LayerData.IsPlayerLayer(other.gameObject.layer) || entered)
        {
            return;
        }

        Debug.Log("Next ZOne!!");
        entered = true;

        LevelManager.Instance.NextZone();
    }
}
