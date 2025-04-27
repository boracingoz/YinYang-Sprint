using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSettings", menuName = "ScriptableObjects/Obstacle Settings")]
public class ObstacleSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float speedMultiplier = 1.2f;
    public float despawnDistance = -10f;

    [Header("Visaul Settings")]
    public Color obstacleColor = Color.red;

    [Header("Gameplay Settings")]
    public int damageAmount = 1;
    public bool isDestructible = false;
    public int scoreValue = 0;

    [Header("Rotation Settings")]
    public Vector3 spawnRotation = Vector3.zero;
}
