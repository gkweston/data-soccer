using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tracks ball at speed set in Unity engine

public class ControllerP1 : MonoBehaviour
{
    private Transform _player1Trans;
    public Transform targetBall;

    public float speed;
    
    void Start()
    {
        
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetBall.position, step);
    }
}
