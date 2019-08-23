using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tracks ball at speed set in Unity then shoots when in shooting radius of goal
// conditionals and booleans need consolidation
// defined as controller for 1st player, but will act as foundation for all players

public class ControllerP1 : MonoBehaviour
{
    public Transform ballTrans;    // allows ball position to be shared with player

    private bool _hasBall;
    
    public float speed;
    private Vector2 _goalLine;    // position of goal
    private Vector2 _startingPosition;
    private float _distanceToGoal;
    private float _distanceToBall;
    public float shotRadius;    // allows distance which player will take a shot to be assigned in Unity
    
    private bool _takeShot;
    
    
    void Start()
    {
        _hasBall = false;    // defaults to player not having possession
        _goalLine.x = 0;    // defines position which player should shoot for the goal at
        _goalLine.y = 5.5f;
        
    }

    void Update()
    {
        _distanceToBall = Vector2.Distance(transform.position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(transform.position, _goalLine);

        if (_distanceToBall <= .25f && !_takeShot)    // defines radius which allows player to take possession of ball
        {
            _hasBall = true;
        }
        else
        {
            _hasBall = false;
        }

        float step = speed * Time.deltaTime;    // defines velocity which player moves
        
        if (_hasBall && !_takeShot)    // if the player has ball, but is outside shooting radius, move towards goal
        {
            transform.position = Vector2.MoveTowards(transform.position, _goalLine, step);
        }
        else
        {
            if (!_takeShot)    // otherwise player does not have ball and must move towards it
            {
                transform.position = Vector2.MoveTowards(transform.position, ballTrans.position, step);
            }
        }

        if (_distanceToGoal < shotRadius)    // take shot when within radius
        {
            _takeShot = true;
        }
        
        if (_takeShot)
        {
            _hasBall = false;
        }
        
    }
}
