using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// transforms the ball continuously in a figure 8 to test player controller scripts
// update so player shots are defined by a randomized s-curve, randomized parameters of figure 8

public class ControllerBall : MonoBehaviour
{
    public float rotateSpeed = 5f;    // only necessary to make ball move in figure 8, which is useful for testing player script
    public float shotSpeed;
    private float _angle;
    public float radius = 0.1f;
    private Vector2 _center;    // alias for ball position set in Unity, origin of figure 8
    
    public Transform player1Trans;    // allows player position to be shared with ball
    private bool _hasBall;
    

    private Vector2 _goalLine;    // defines goal position on field

    public float shotRadius;    // allows shot radius to be changed and tested in Unity

    private float _distanceToBall;
    private float _distanceToGoal;
    private bool _takeShot;

    public float constant;    // defines A in S-Curve for shooting mechanics
    float xComp;   // for S-curve
    float yComp;
    private float randomNumber;
    
    void Start()    // assign center of figure 8, default the player to not having possession, define goal position
    {
        _center = transform.position;
        _hasBall = false;
        _goalLine.x = 0f;
        _goalLine.y = 5.5f;

        randomNumber = Random.Range(-0.3f, 0.3f);    // try in for loop for more variations
        constant = 10 * shotRadius;
    }

    void Update()
    {
        
        _distanceToBall = Vector2.Distance(transform.position, player1Trans.position);    // update player distances
        _distanceToGoal = Vector2.Distance(player1Trans.position, _goalLine);

        if (_distanceToBall <= .25f && !_takeShot)    // player has possession, but cannot take a shot
        {
            _hasBall = true;
        }
        else    // otherwise default to player not having possession
        {
            _hasBall = false;
        }
        
        
        if (!_hasBall && !_takeShot)    // rotate in figure 8 if player doesn't have possession
        {
            _angle += rotateSpeed * Time.deltaTime;
        
            var offset = new Vector2( 10 * randomNumber * Mathf.Sin(_angle) * Mathf.Cos(_angle), 2 * randomNumber * Mathf.Sin(_angle)) * radius;
            transform.position = _center + offset;    // randomized figure 8
        }
        else    // otherwise translate ball with player until it is in shooting radius
        {
            if (!_takeShot)
            {
                transform.position = player1Trans.position;
            }
        }

        if (_distanceToGoal < shotRadius && transform.position.y < 5.5f)    // player has ball and is within shooting radius
        {
            _takeShot = true;
        }


        if (_takeShot)    // player takes shot, with choppy randomized s-curve defining chance of scoring on goal
        {
            for (var i = 0; i < 10; i ++)
            {

                if (transform.position.y > 5.5)
                {
                    break;
                }
                
                xComp = randomNumber * i;
                yComp = (constant / (1 + Mathf.Exp(-i))) - (constant / 2);

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(xComp, yComp), shotSpeed);

                xComp = transform.position.x;
                yComp = transform.position.y;
                
            }
        }
    }
}
