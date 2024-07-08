using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPage : MonoBehaviour
{

    private GameObject _Loading;

    private void OnEnable()
    {
        _Loading = transform.Find("Loading").gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 rot = _Loading.transform.localEulerAngles;
        rot.z += 180f * Time.deltaTime;
        _Loading.transform.localEulerAngles = rot;
    }
}
