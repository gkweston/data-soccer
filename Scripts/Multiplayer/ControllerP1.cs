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
    private bool _hasStamina;
    public float staminaCounter = 5;
    private float _maxStamina;
    private float _staminaBoost = 2;
    private float _speed;
    private float _defaultSpeed = 1;
    
    
    private float _deltaPosition;    //for velocity based stamina system

    
    void Start()
    {
        _possession = false;    // defaults to player not having possession
        _goalLine.x = 0;    // defines position which player should shoot for the goal at
        _goalLine.y = 5.5f;
        
        // <stamina Counter> //
        _maxStamina = staminaCounter;
        _defaultSpeed = _speed;

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
        
        // <stamina Counter> // if _hasStamina random /= 2
        /* This stamina timer system only works if players have different default stamina
         * however, that results in one player always winning a challenge. Refactor system
         * to be based around distance traveled rather than time passed, or another dynamic
         * method
         */ 
        
        if (_hasStamina) staminaCounter -= Time.deltaTime;

        if (staminaCounter <= _maxStamina - _maxStamina) _hasStamina = false;

        if (!_hasStamina) staminaCounter += Time.deltaTime;

        if (staminaCounter >= _maxStamina)  _hasStamina = true;
        
        if (_hasStamina) _speed = _staminaBoost;

        if (!_hasStamina) _speed = _defaultSpeed;



        MoveToBall();

        if (_distanceToGoal < shotRadius)    // take shot when within radius
        {
            _takeShot = true;
        }
        
        if (_takeShot)
        {
            _possession = false;
        }
        
        // <Debug> //
        
        
    }
}
