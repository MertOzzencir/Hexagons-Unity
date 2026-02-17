using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineInputAxisController camInputController;
    void Awake()
    {
        camInputController = GetComponent<CinemachineInputAxisController>();
        camInputController.enabled = false;
    }
    private void OrbitalFollow(bool obj)
    {
        Debug.Log(obj);
        camInputController.enabled = obj;
    }
    void OnEnable()
    {
        InputManager.OnRightClick += OrbitalFollow;
    }
    private void OnDisable()
    {
        InputManager.OnRightClick -= OrbitalFollow;
    }

}
