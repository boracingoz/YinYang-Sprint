using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 5f;


    [Header("Look Settings")]
    public bool lookAtTarget = true;
    public Vector3 lookOffset = new Vector3(0, 1, 0);

    [Header("Rotation Settings")]
    public bool rotateWithTarget = false;

    [Header("Damping Settings")]
    public bool dampYMovement = true;
    public float verticalSmoothness = 0.5f;
}
