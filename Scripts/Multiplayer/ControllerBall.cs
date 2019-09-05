using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ControllerBall : MonoBehaviour
{
    public Transform transP1;    // allows player position to be shared with ball
    public Transform transP2;    // allows player 2 positions to be shared with ball
    

    // figure 8 variables //
    public float rotateSpeed = 0.5f;
    private float _angle;
    public float radius = 2;
    
    
    // position vectors //
    private Vector2 _p1Goal;    // defines goal position on field
    private Vector2 _p2Goal;
    
    
    private Vector2 _center;    // alias for ball position set in Unity, origin of figure 8

    public float shotRadius;    // allows shot radius to be changed and tested in Unity

    // booleans //
    public bool p1Possession;
    private bool _hasShotP1;
    public bool p2Possession;
    private bool _hasShotP2;
    
    // distance to ball and goal //
    private float _p1DistanceToBall;        /*debug*/
    private float _p2DistanceToBall;
    private float _distanceTopGoal;
    private float _distanceBottomGoal;
    private float _possessionRadius;
    
    // stamina counters //
    public float p1Stamina;
    public float p2Stamina;

    // for S-curve shot //
    public float shotSpeed = 2f;
    public float sCurveConst = 20;    // defines 'A' in S-Curve for shooting mechanics
    private float _xComp;   // for S-curve
    private float _yComp;
    private float _randomNum;
    
    
    void Start()
    {
        
        _possessionRadius = 0.05f;
        p1Possession = false;
        _p1Goal.x = 0f;
        _p1Goal.y = 5.5f;
        
        p2Possession = false;
        _p2Goal.x = 0f;
        _p2Goal.y = -5.5f;
        

        _randomNum = Random.Range(-0.3f, 0.3f);
        sCurveConst = 10 * shotRadius;
        
        
        // <Debug> //

        _center = transform.position;
    }
    
    

    void ShotMechanics()    // implements 1/2 of s-curve whose deviation from line of shot depends on _randomNum range
    {
        for (int i = 0; i < 10; i++)
        {
            if (transP1.position.y > _p1Goal.y || transP2.position.y < -_p2Goal.y) break;    // conditional failsafe, remove in optimization //

            _xComp = _randomNum * i;
            _yComp = (sCurveConst / (1 + Mathf.Exp(-i))) - (sCurveConst / 2);

            var shotPosition = transform.position;
            transform.position = Vector2.MoveTowards(shotPosition, new Vector2(_xComp, _yComp), shotSpeed);

            _xComp = shotPosition.x;
            _yComp = shotPosition.y;
            
        }
    }
    

    void Update()
    {
        
        var p1Position = transP1.position;
        var p2Position = transP2.position;
        
        p1Stamina = FindObjectOfType<ControllerP1>().stamina;    // These may be expensive calls
        p2Stamina = FindObjectOfType<ControllerP2>().stamina;    // test in optimization stage
        
        var ballPosition = transform.position;
        _p1DistanceToBall = Vector2.Distance(ballPosition, p1Position);
        _p2DistanceToBall = Vector2.Distance(ballPosition, p2Position);

        /* defining possession and passing to player controllers */

        // this defines a stamina based challenge
        if (_p1DistanceToBall <= _possessionRadius && _p2DistanceToBall <= _possessionRadius)    
        {
            if (p1Stamina > p2Stamina)
            {
                p1Possession = true;
            }
            else
            {
                p2Possession = true;
            }
        }

        // these define challengless possessions
        if (_p1DistanceToBall <= _possessionRadius && !(_p2DistanceToBall <= _possessionRadius))
        {
            p1Possession = true;
        }

        if (_p2DistanceToBall <= _possessionRadius && !(_p1DistanceToBall <= _possessionRadius))
        {
            p2Possession = true;
        }
        
        // <Debug> //
        


        // Ball makes a figure eight, useful to test player movement and possession bools
        _angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
        transform.position = _center + offset;



        
        
        if (p1Possession && p2Possession)
        {
            print("It's broke yo!!!");
        }


    }
}
