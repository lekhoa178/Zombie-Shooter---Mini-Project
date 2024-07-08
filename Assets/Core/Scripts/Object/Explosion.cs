using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    List<GameObject> collided;

    private float damage;
    private bool sustain;
    private SphereCollider hitboxCollider;

    private float startTime;

    private void Awake()
    {
        hitboxCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        collided = new List<GameObject>();

        string[] splt = gameObject.name.Split('_');

        damage = float.Parse(splt[0]);
        sustain = int.Parse(splt[1]) == 1;

        startTime = Time.time;
    }

    private void OnDisable()
    {
        collided.Clear();

        hitboxCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collided.Contains(other.gameObject))
            return;

        Transform parent = other.GetComponentInParent<Rigidbody>().transform;

        if (LevelManager.Instance.LayerData.IsEnemyLayer(other.gameObject.layer))
        {
            collided.Add(other.gameObject);
            parent.SendMessage("TakeDamage", damage);
        }

        if (LevelManager.Instance.LayerData.IsPlayerLayer(other.gameObject.layer))
        {
            collided.Add(other.gameObject);
            parent.SendMessage("TakeDamage", 0.5f);
        }

    }

    private void Update()
    {
        if (Time.time > startTime + 0.1f)
            hitboxCollider.enabled = false;
    }

}