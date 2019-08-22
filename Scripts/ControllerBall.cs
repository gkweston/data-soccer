using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// transforms the ball continuously in a figure 8 to test player controller scripts
// conditions & booleans need consolidation

public class ControllerBall : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float shotSpeed;
    public float radius = 0.1f;
    public Transform player1Trans;
    private bool _hasBall;
    
    private Vector2 _center;
    private Vector2 _goalLine;
    private float _angle;
    public float shotRadius;

    private float _distanceToBall;
    private float _distanceToGoal;
    private bool _takeShot;
    
    
    void Start()
    {
        _center = transform.position;
        _hasBall = false;
        _goalLine.x = 0f;
        _goalLine.y = 5.5f;

    }

    void Update()
    {
        _distanceToBall = Vector2.Distance(transform.position, player1Trans.position);
        _distanceToGoal = Vector2.Distance(player1Trans.position, _goalLine);

        if (_distanceToBall <= .25f && !_takeShot)
        {
            _hasBall = true;
        }
        else
        {
            _hasBall = false;
        }
        
        
        if (!_hasBall && !_takeShot)
        {
            _angle += rotateSpeed * Time.deltaTime;
        
            var offset = new Vector2( Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
            transform.position = _center + offset;
        }
        else
        {
            if (!_takeShot)
            {
                transform.position = player1Trans.position;
            }
        }

        if (_distanceToGoal < shotRadius)
        {
            _takeShot = true;
        }

        if (_takeShot)
        {
            transform.position = Vector2.MoveTowards(transform.position, _goalLine, shotSpeed);
        }
    }
}
