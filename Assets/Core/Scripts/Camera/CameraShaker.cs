using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;
    private CinemachineImpulseSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        source.GenerateImpulseWithForce(0.1f);
    }
}