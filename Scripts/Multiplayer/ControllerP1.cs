using UnityEngine;

public class ControllerP1 : MonoBehaviour
{
    public Transform ballTrans;
    public float shotRadius = 2f;   
    
    // calculable distances //
    private float _distanceToGoal;
    private float _distanceToBall;
    
    // position vectors //
    private Vector2 _goalLine;    // position of goal
    private Vector2 _startingPosition;
    
    // booleans //
    public bool takeShot;    // reads true if w/i shooting radius
    private bool _possession;    // read in from ControllerBall.cs
    private bool _p2Possession;
    
    // distance based stamina system variables //
    private Vector2 _lastPosition;
    private Vector2 _currentPosition;
    private float _deltaPosition;
    
    private bool _hasStamina;
    public float stamina;
    private float _maxStamina = 5f;
    private float _speed = 1f;
    private float _defaultSpeed;
    private float _speedBoost = 3f;
    
    // stall timer //
    private bool _stall;
    
    
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
    
    void PlayerMovement()
    {
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(position, _goalLine);
        
        var step = _speed * Time.deltaTime;    // defines velocity which player moves

        if (_possession)
        {
            if (_distanceToGoal <= shotRadius)
            {
                takeShot = true;
            }
            else
            {
                if (_stall) return;
                transform.position = Vector2.MoveTowards(position, _goalLine, step);
            }
        }
        else
        {
            if (_stall) return;
            transform.position = Vector2.MoveTowards(position, ballTrans.position, step);
        }
    }

    public void Stamina()
    {
        _deltaPosition = Vector2.Distance(transform.position, _lastPosition);
        
        if (stamina >= _maxStamina) _hasStamina = true;
        if (stamina <= 0) _hasStamina = false;

        if (_hasStamina)
        {
            stamina -= _deltaPosition;
            _speed = _speedBoost;
        }

        if (!_hasStamina)
        {
            stamina += Time.deltaTime;
            _speed = _defaultSpeed;
        }
        
        _lastPosition = transform.position;
    }

    void Update()
    {
        _stall = FindObjectOfType<ControllerBall>().p1Stall;
        _possession = FindObjectOfType<ControllerBall>().p1Possession;    // test for expensive invocation

        PlayerMovement();
        Stamina();
        
        // <Debug> //


    }
}
