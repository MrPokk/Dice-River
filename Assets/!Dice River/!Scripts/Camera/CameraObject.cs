using BitterECS.Integration;
using Unity.Cinemachine;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public CinemachineCamera cinemachineCamera;
    private void Awake()
    {
        _camera ??= GetComponentInChildren<Camera>();
        cinemachineCamera ??= GetComponentInChildren<CinemachineCamera>();
    }

    public void SetTarget(Transform target)
    {
        cinemachineCamera.Follow = target;
    }

}

