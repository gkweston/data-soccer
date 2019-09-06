using UnityEngine;

public class ControllerP2 : MonoBehaviour
{
    /*
     * In simultaneous development with ControllerP1 as some functions generalize between the two
     */
    
    public Transform ballTrans;
    public float shotRadius = 2f;
    
    private bool _possession;
    public bool takeShot;

    private float _distanceToBall;
    private float _distanceToGoal;
    
    private float _speed = 1f;
    
    private Vector2 _goalLine;
    private Vector2 _startingPosition;

    public float stamina;

    private bool _stall;
    
    
    void Start()
    {

        _possession = false;    // defaults to player not having possession before Finding possP2 object
        
        _goalLine.x = 0f;
        _goalLine.y = -5.5f;

        stamina = 50f;

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
    
    void Update()
    {
        _stall = FindObjectOfType<ControllerBall>().p2Stall;
        _possession = FindObjectOfType<ControllerBall>().p2Possession;    // check for expensive invocation in testing
        
        PlayerMovement();
        
        // <Debug> //


        
    }
}
