using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ControllerP2 : MonoBehaviour
{
    /*
     * In simultaneous development with ControllerP2 as functions generalize between the two
     */
    
    public Transform ballTrans;

    private bool _possession;
    private bool _takeShot;

    private float _distanceToBall;
    private float _distanceToGoal;
    private float speed = 1;
    
    private Vector2 _goalLine;
    private Vector2 _startingPosition;
    
    
    void Start()
    {
        _possession = false;
        _goalLine.x = 0f;
        _goalLine.y = -5.5f;
        
    }

    void MoveToBall()
    {
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(position, _goalLine);

        if (_distanceToBall <= .25f && !_takeShot)    // defines radius which allows player to take possession of ball
        {
            _possession = true;
        }
        else
        {
            _possession = false;
        }

        float step = speed * Time.deltaTime;    // defines velocity which player moves
        
        if (_possession && !_takeShot)    // if the player has ball, but is outside shooting radius, move towards goal
        {
            transform.position = Vector2.MoveTowards(position, _goalLine, step);
        }
        else
        {
            if (!_takeShot)    // otherwise player does not have ball and must move towards it
            {
                transform.position = Vector2.MoveTowards(position, ballTrans.position, step);
            }
        }
    }
    
    void Update()
    {
        var position = transform.position;
        var ballPosition = ballTrans.position;
        _distanceToBall = Vector2.Distance(position, ballPosition);
        _distanceToGoal = Vector2.Distance(position, _goalLine);
        


        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(position, ballPosition, step);
        
        MoveToBall();
        
        // <Debug> //

    }
}
