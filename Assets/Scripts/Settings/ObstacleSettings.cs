using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSettings", menuName = "ScriptableObjects/Obstacle Settings")]
public class ObstacleSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float speedMultiplier = 1.2f;
    public float despawnDistance = -10f;
}
