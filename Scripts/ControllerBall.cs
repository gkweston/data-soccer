using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// transforms the ball continuously in a figure 8 to test player controller scripts

public class ControllerBall : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float radius = 0.1f;

    private Vector2 _center;
    private float _angle;
    
    
    void Start()
    {
        _center = transform.position;
    }

    void Update()
    {

        _angle += rotateSpeed * Time.deltaTime;
        
        var offset = new Vector2(Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
        transform.position = _center + offset;
        
    }
}
