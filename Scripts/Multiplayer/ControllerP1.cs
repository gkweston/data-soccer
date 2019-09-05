using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// tracks ball at speed set in Unity then shoots when in shooting radius of goal
// conditionals and booleans need consolidation
// defined as controller for 1st player, but will act as foundation for all players

public class ControllerP1 : MonoBehaviour
{
    public Transform ballTrans;    // allows ball position to be shared with player

    // <public variables> //
    public float shotRadius;    // allows distance which player will take a shot to be assigned in Unity
    
    // <calculable distances> //
    private float _distanceToGoal;
    private float _distanceToBall;
    
    // <position vectors> //
    private Vector2 _goalLine;    // position of goal
    private Vector2 _startingPosition;
    
    // <booleans> //
    private bool _takeShot;
    private bool _possession;
    
    // <stamina variables> //
    //private bool _hasStamina;
    //public float stamina = 5;
    //private float _maxStamina;
    //private float _speedBoost = 2;
    //private float _speed;
    //private float _defaultSpeed = 1;
    
    private Vector2 _lastPosition;
    private Vector2 _currentPosition;
    private float _deltaDistance;
    
    private bool _hasStamina;
    public float stamina;
    private float _maxStamina = 5;
    private float _speed = 1;
    private float _defaultSpeed;
    private float _speedBoost = 2;

    
    private float _deltaPosition;    //for velocity based stamina system
    
    
    void Start()
    {
        _possession = false;    // defaults to player not having possession
        _goalLine.x = 0;    // defines position which player should shoot for the goal at
        _goalLine.y = 5.5f;
        
        // <stamina Counter> //

        _defaultSpeed = _speed;
        stamina = _maxStamina;
        
        _lastPosition = transform.position;

    }
    
    void MoveToBall()
    {
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(position, _goalLine);
        
        float step = _speed * Time.deltaTime;    // defines velocity which player moves
        
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
        _possession = FindObjectOfType<ControllerBall>().p1Possession;    // test for expensive invocation

        //if (!_possession)
        //{
        //    MoveToBall();

        //}
        

        // <stamina Counter> // if _hasStamina random /= 2
        /* This stamina timer system only works if players have different default stamina
         * however, that results in one player always winning a challenge. Refactor system
         * to be based around distance traveled rather than time passed, or another dynamic
         * method
         */

        _deltaDistance = Vector2.Distance(transform.position, _lastPosition);
         
        // if stamina < maxStamina, count up based on time

        if (stamina >= _maxStamina) _hasStamina = true;   // boolean definition of stamina
        if (stamina <= 0) _hasStamina = false;

        if (_hasStamina)
        {
            stamina -= _deltaDistance;
            _speed = _speedBoost;
        }
        if (!_hasStamina)
        {
            stamina += Time.deltaTime;
            _speed = _defaultSpeed;
        }

        
        
        _lastPosition = transform.position;
        
        // <Debug> //

        MoveToBall();
        
        print("P1 Poss:" + _possession);
    }
}
