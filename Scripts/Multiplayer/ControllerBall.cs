using UnityEngine;
using Random = UnityEngine.Random;

public class ControllerBall : MonoBehaviour
{
    public Transform p1Trans;    // allows player position to be shared with ball
    public Transform p2Trans;    // allows player 2 positions to be shared with ball

    // figure 8 variables //
    public float rotateSpeed = 0.5f;
    private float _angle;
    public float radius = 2;
    
    
    // position vectors //
    private Vector2 _p1Goal;    // defines goal position on field
    private Vector2 _p2Goal;
    
    
    private Vector2 _center;    // alias for ball position set in Unity, origin of figure 8

    private float _shotRadius;    // allows shot radius to be changed and tested in Unity

    // booleans //
    public bool p1Possession;
    public bool p2Possession;
    public bool p1Stall;
    public bool p2Stall;

    // distance to ball and goal //
    private float _p1DistanceToBall;        /*debug*/
    private float _p2DistanceToBall;
    private float _possessionRadius;
    
    // stamina counters //
    public float p1Stamina;
    public float p2Stamina;
    private float _p1StallTimer = 0;
    private float _p2StallTimer = 0;

    // for S-curve shot //
    public float shotSpeed = 5f;
    public float sCurveConst = 20;    // defines 'A' in S-Curve for shooting mechanics
    private float _xComp;   // for S-curve
    private float _yComp;
    private float _randomNum;

    private bool _p1TakeShot;
    private bool _p2TakeShot;
    
    
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
        sCurveConst = 10 * _shotRadius;
        
        
        // <Debug> //

        _center = transform.position;
    }
    
    void ShotMechanics()    // implements 1/2 of s-curve whose deviation from line of shot depends on _randomNum range
    {
        for (var i = 10 - 1; i >= 0; i--)
        {
            
            _xComp = _randomNum * i;
            _yComp = (sCurveConst / (1 + Mathf.Exp(-i))) - (sCurveConst / 2);
    
            var shotPosition = transform.position;
            transform.position = Vector2.MoveTowards(shotPosition, new Vector2(_xComp, _yComp), shotSpeed);
    
            _xComp = shotPosition.x;
            _yComp = shotPosition.y;
    
            //shotPosition.x = _xComp;
            //shotPosition.y = _yComp;    // may debug shot mechanics
    
        }
    }

    void PlayerPossession()    // defines possession between players
    {
        var p1Position = p1Trans.position;
        var p2Position = p2Trans.position;
        
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
                p2Possession = false;
            }
            else
            {
                p1Possession = false;
                p2Possession = true;
            }
        }

        // these define challengeless possessions
        if (_p1DistanceToBall <= _possessionRadius && !(_p2DistanceToBall <= _possessionRadius))
        {
            p1Possession = true;
            p2Possession = false;
        }

        if (_p2DistanceToBall <= _possessionRadius && !(_p1DistanceToBall <= _possessionRadius))
        {
            p1Possession = true;
            p2Possession = true;
        }

        if (p1Possession)
        {
            p2Possession = false;
            transform.position = p1Position;
        }

        if (p2Possession)
        {
            p1Possession = false;
            transform.position = p2Position;
        }
    }

    void PlayerChallenge()
    {
        var distancePlayers = Vector2.Distance(p1Trans.position, p2Trans.position);

        if (!(distancePlayers < 0.5f)) return;
        if (p1Possession && (p1Stamina > p2Stamina))
        {
            p2Stall = true;
        }

        if (p1Possession && (p1Stamina < p2Stamina))
        {
            p1Stall = true;
            p2Possession = true;
        }


        if (p2Possession && p2Stamina > p1Stamina)
        {
            p1Stall = true;
        }

        if (p2Possession && p2Stamina < p1Stamina)
        {
            p2Stall = true;
            p1Possession = true;
        }

        if (!p1Stall || !(_p1StallTimer <= 2))
        {
            p1Stall = false;
        }
        else
        {
            _p1StallTimer += Time.deltaTime;
        }

        if (!p2Stall || !(_p2StallTimer <= 2))
        {
            p2Stall = false;
        }
        else
        {
            _p2StallTimer += Time.deltaTime;
        }
    }
    
    void BallDebugMove()    // Ball makes a figure eight, useful to test player movement and possession bools
    {
        _angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
        transform.position = _center + offset;
    }

    void Update()
    {

        _p1TakeShot = FindObjectOfType<ControllerP1>().takeShot;
        _p2TakeShot = FindObjectOfType<ControllerP2>().takeShot;
        
        
        PlayerPossession();    // Test for expensive invocation
        PlayerChallenge();
        
        // <Debug> //

        if (p1Possession || p2Possession)
        {
        }
        else
        {
            BallDebugMove();
        }


        if (p1Possession && p2Possession)
        {
            print("***Shared Possession***");
        }
        
        print("P1Stall:" + p1Stall);
        print("P2Stall:" + p2Stall);

    }
}
