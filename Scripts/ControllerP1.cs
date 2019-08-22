using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tracks ball at speed set in Unity engine then shoots when in shooting radius of goal
// conditionals & booleans need consolidation

public class ControllerP1 : MonoBehaviour
{
    public Transform ballTrans;

    private bool _hasBall;
    
    public float speed;
    private Vector2 _goalLine;
    private Vector2 _startingPosition;
    private float _distanceToGoal;
    private float _distanceToBall;
    public float shotRadius;
    
    private bool _takeShot;
    
    
    void Start()
    {
        _hasBall = false;
        _goalLine.x = 0;
        _goalLine.y = 5.5f;
        
    }

    void Update()
    {
        _distanceToBall = Vector2.Distance(transform.position, ballTrans.position);
        _distanceToGoal = Vector2.Distance(transform.position, _goalLine);

        if (_distanceToBall <= .25f && !_takeShot)
        {
            _hasBall = true;
        }
        else
        {
            _hasBall = false;
        }

        float step = speed * Time.deltaTime;
        
        if (_hasBall && !_takeShot)
        {
            transform.position = Vector2.MoveTowards(transform.position, _goalLine, step);
        }
        else
        {
            if (!_takeShot)
            {
                transform.position = Vector2.MoveTowards(transform.position, ballTrans.position, step);
            }
        }

        if (_distanceToGoal < shotRadius)
        {
            _takeShot = true;
        }
        
        if (_takeShot)
        {
            _hasBall = false;
        }
        
    }
}
