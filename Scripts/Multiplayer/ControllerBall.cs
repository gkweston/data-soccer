using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControllerBall : MonoBehaviour
{
    public Transform transP1;    // allows player position to be shared with ball
    public Transform transP2;    // allows player 2 positions to be shared with ball
    

    // figure 8 variables //
    public float rotateSpeed = 5f;    // only necessary to make ball move in figure 8, which is useful for testing player script
    public float shotSpeed;
    private float _angle;
    public float radius = 0.1f;
    private float _possessionRadius;
    
    // position vectors //
    private Vector2 _p1Goal;    // defines goal position on field
    private Vector2 _p2Goal;
    
    
    private Vector2 _center;    // alias for ball position set in Unity, origin of figure 8

    public float shotRadius;    // allows shot radius to be changed and tested in Unity

    // booleans //
    private bool _possessionP1;
    private bool _hasShotP1;
    private bool _possessionP2;
    private bool _hasShotP2;
    
    // distance to ball and goal //
    private float _distanceToBallP1;
    private float _distanceToBallP2;
    private float _distanceTopGoal;
    private float _distanceBottomGoal;
    
    // stamina counters //
    public float staminaP1;
    public float staminaP2;

    // for S-curve shot //
    public float sCurveConst;    // defines 'A' in S-Curve for shooting mechanics
    private float _xComp;   // for S-curve
    private float _yComp;
    private float _randomNum;
    
    
    void Start()
    {
        _possessionP1 = false;
        _p1Goal.x = 0f;
        _p1Goal.y = 5.5f;
        _possessionP2 = false;
        _p2Goal.x = 0f;
        _p2Goal.y = -5.5f;
        

        _randomNum = Random.Range(-0.3f, 0.3f);
        sCurveConst = 10 * shotRadius;
        

        _possessionRadius = 0.25f;

    }
    
    private void MakeFigureEight()    // Ball makes a figure eight, useful to test player movement and possession bools
    {
        _center = transform.position;
        _angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle) * Mathf.Cos(_angle)) * radius;
        transform.position = _center + offset;
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
        
        staminaP1 = FindObjectOfType<ControllerP1>().staminaCounter;    // These may be expensive calls
        //staminaP2 = FindObjectOfType<ControllerP2>().staminaCounter;    // test in optimization stage


        var ballPosition = transform.position;
        _distanceToBallP1 = Vector2.Distance(ballPosition, transP1.position);
        _distanceToBallP2 = Vector2.Distance(ballPosition, transP2.position);

        
        // p1 began head to head
        if (_distanceToBallP1 <= _possessionRadius)
        {
            if (_distanceToBallP2 <= _possessionRadius)
            {
                if (staminaP1 > staminaP2)
                {
                    _possessionP1 = true;
                }

                if (staminaP2 > staminaP1)
                {
                    _possessionP2 = true;
                }
            }
        }
        else
        {
            _possessionP1 = true;
        }

        
        // p2 began head to head
        if (_distanceToBallP2 <= _possessionRadius)
        {
            if (_distanceToBallP1 <= _possessionRadius)
            {
                if (staminaP2 > staminaP1)
                {
                    _possessionP2 = true;
                }

                if (staminaP1 > staminaP2)
                {
                    _possessionP1 = true;
                }
            }
        }
        else
        {
            _possessionP2 = true;
        }

        
        // player 2 challenges player 1
        if (_possessionP1 && (_distanceToBallP2 <= _possessionRadius))
        {
            if (staminaP2 > staminaP1)
            {
                _possessionP1 = false;
                _possessionP2 = true;
            }
        }

        
        // player 1 challenges player 2
        if (_possessionP2 && (_distanceToBallP1 <= _possessionRadius))
        {
            if (staminaP1 > staminaP2)
            {
                _possessionP2 = false;
                _possessionP1 = true;
            }
        }
        
        
        // define ball position based on possession
        if (_possessionP1)
        {
            transform.position = transP1.position;
        }

        if (_possessionP2)
        {
            transform.position = transP2.position;
        }

        _distanceTopGoal = Vector2.Distance(transform.position, _p1Goal);    // create TakeShot() to generalize between P1 and P2
        if (_distanceTopGoal <= shotRadius)
        {
            ShotMechanics();
        }
        
        
        // <Debug> //
        
        if (!_possessionP1 && !_possessionP2)
        {
            MakeFigureEight();
        }
        
        
        
        print("P1 Poss[" + _possessionP1 + "] Stam[" + staminaP1 + "]");
        print("P2 Poss[" + _possessionP2 + "] Stam[" + staminaP2 + "]");

    }
}
