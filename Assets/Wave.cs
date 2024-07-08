using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Wave : MonoBehaviour
{
    private bool _Soaking = false;

    private float _StartTime = 0;

    private float _SoakingTime = 4f;
    private float _DestroyTime = 7f;
    private bool _BigWave = false;

    private void Awake()
    {
        _Soaking = false;
        _StartTime = Time.time;

        if (transform.name == "BigWave")
        {
            _BigWave = true;
            _SoakingTime = 12f;
            _DestroyTime = 20f;
        }
    }

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0);

        StartCoroutine(DisableCollider());
    }

    private void Update()
    {

        if (!_Soaking && Time.time > _StartTime + _SoakingTime)
            _Soaking = true;

        if (Time.time > _StartTime + _DestroyTime)
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (_BigWave)
        {
            float scale = transform.localScale.x;
            scale = Mathf.Min(2f, scale + Time.deltaTime * 0.45f);

            transform.localScale = new Vector3(scale, scale, scale);
        }

        if (_Soaking)
        {
            float amount = 0.1f;
            if (_BigWave)
                amount = 0.35f;

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - amount * Time.deltaTime,
                transform.position.z);

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        Enemy enemy = other.gameObject.GetComponent<Enemy>();
    //        StunEnemy(enemy);
    //    }
    //}

    //private void StunEnemy(Enemy enemy)
    //{
    //    enemy.Stunned(2f);
    //    enemy.Agent.ResetPath();

    //    Vector3 repelledDirection = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z) 
    //        - new Vector3(transform.position.x, 0, transform.position.z);

    //    repelledDirection = repelledDirection.normalized * (3f - repelledDirection.sqrMagnitude);
    //    Vector3 repelledForce = repelledDirection;
    //    enemy.gameObject.GetComponent<Rigidbody>().AddForce(repelledForce, ForceMode.Impulse);
    //}

    private IEnumerator DisableCollider()
    {
        yield return new WaitForNextFrameUnit();
        GetComponent<Collider>().enabled = false;
    }
}
