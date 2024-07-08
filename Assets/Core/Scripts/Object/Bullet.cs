using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GunType type;
    private int bulletId;
    private float bulletSpeed;
    private float damage;
    private float dstToDestroy;
    private int explodeId;
    private float radius;
    private int sustain;

    private float cdst = 0;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float height = 5f;
    private float time;
    private Vector3 controlPoint;

    private GameObject antGO;

    private void OnEnable()
    {
        string[] spl = gameObject.name.Split('_');
        bulletId = int.Parse(spl[1]);
        bulletSpeed = float.Parse(spl[2]);
        explodeId = int.Parse(spl[3]);
        dstToDestroy = float.Parse(spl[5]);
        damage = int.Parse(spl[6]);
        type = (GunType)int.Parse(spl[7]);
        radius = float.Parse(spl[8]);
        sustain = int.Parse(spl[9]);

        endPoint.x = float.Parse(spl[10]);
        endPoint.z = float.Parse(spl[11]);
        endPoint.y = 0;

        antGO = null;

        time = 0;
        cdst = 0;

        if (type == GunType.Parabolic)
        {
            startPoint = transform.position;
            CalculateControlPoint();
            antGO = AnticipationHandler.Instance.CircularAnticipation(endPoint, radius);
        }
    }

    private void OnDisable()
    {
        PoolingManager.Instance.GetPool(bulletId).Return(gameObject);

        if (explodeId != -1)
            ExplosionHandler.Instance.Initialize(explodeId, transform.position, radius, damage, sustain);

        if (antGO != null)
        {
            AnticipationHandler.Instance.ReturnCircularAnticipation(antGO);
        }
    }

    private void FixedUpdate()
    {
        switch (type)
        {
            case GunType.Straight:
                cdst += Time.deltaTime * bulletSpeed;
                transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
                break;

            case GunType.Parabolic:
                time += Time.deltaTime * bulletSpeed;
                Vector3 currentPos = CalculateParabolicPoint(time);
                transform.position = currentPos;

                break;
        }
        
    }

    private void Update()
    {
        if (cdst > dstToDestroy || time > 1f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == GunType.Straight && (LevelManager.Instance.LayerData.IsPreventerLayer(other.gameObject.layer)
            || LevelManager.Instance.LayerData.IsEnemyLayer(other.gameObject.layer)
            || LevelManager.Instance.LayerData.IsPlayerLayer(other.gameObject.layer)))
        {
            gameObject.SetActive(false);
        }
    }

    void CalculateControlPoint()
    {
        Vector3 midPoint = (startPoint + endPoint) / 2;
        controlPoint = new Vector3(midPoint.x, midPoint.y + height, midPoint.z);
    }

    Vector3 CalculateParabolicPoint(float t)
    {
        Vector3 point = Mathf.Pow(1 - t, 2) * startPoint +
                        2 * (1 - t) * t * controlPoint +
                        Mathf.Pow(t, 2) * endPoint;

        return point;
    }
}
