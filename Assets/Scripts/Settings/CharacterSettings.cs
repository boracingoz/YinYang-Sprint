using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaterSettings", menuName ="ScriptableObjects/Character Settings")]
public class CharacterSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 5f;
    public float jumpingForce = 4f;


    [Header("Rotation Settings")]
    public float tiltAngel = 15f;
    public float rotationSmoothness = 5f;
}
