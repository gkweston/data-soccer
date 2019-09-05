using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ControllerP2 : MonoBehaviour
{
    /*
     * In simultaneous development with ControllerP1 as some functions generalize between the two
     */
    
    public Transform ballTrans;

    private bool _possession;
    private bool _takeShot;

    private float _distanceToBall;
    private float _distanceToGoal;
    
    private float speed = 1;
    
    private Vector2 _goalLine;
    private Vector2 _startingPosition;

    public float stamina;
    
    
    void Start()
    {

        _possession = false;    // defaults to player not having possession before Finding possP2 object
        
        _goalLine.x = 0f;
        _goalLine.y = -5.5f;

        stamina = 5;

    }

    void MoveToBall()
    {
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(position, _goalLine);

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
        _possession = FindObjectOfType<ControllerBall>().p2Possession;    // check for expensive invocation in testing

        //if (!_possession)
        //{
        //    MoveToBall();

        //}
        
        // <Debug> //

        MoveToBall();
        
        print("P2 Poss:" + _possession);
    }
}
