using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolRotator : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
