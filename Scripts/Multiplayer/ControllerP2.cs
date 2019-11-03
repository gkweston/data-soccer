using UnityEngine;

public class ControllerP2 : MonoBehaviour {
    public Transform ballTrans;
    public float shotRadius = 2f;   
    
    // calculable distances //
    private float _distanceToGoal, _distanceToBall;
    
    // position vectors //
    private Vector2 _goalLine;    
    private Vector2 _startingPosition;
    
    // booleans //
    public bool takeShot;    
    private bool _possession;    
    private bool _p2Possession;
    
    // Stamina System //

    //private Stamina staminaMethod;
    
    [HideInInspector]
    public float playerStam;
    private int speed;
    
    // stall timer //
    private bool _stall = false;
    
    
    void Start() {
        _possession = false;    // defaults to player not having possession
        _goalLine.x = 0;    // defines position which player should shoot for the goal at
        _goalLine.y = 5.5f;
    }
    
    void PlayerMovement() {
        /*
         * Refactor based on Stamina/Challenge updates
         */
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);    // updates distance to ball and goal to allow for player transform
        _distanceToGoal = Vector2.Distance(position, _goalLine);
        
        // Stamina based speed:
        //playerStam = staminaMethod.stamina;
        Debug.Log(playerStam);
        speed = playerStam > 0 ? 2 : 1;
        var step = speed * Time.deltaTime;    // defines velocity which player moves

        if (_possession) {
            if (_distanceToGoal <= shotRadius) {
                takeShot = true;
            }
            else {
                if (_stall) return;
                transform.position = Vector2.MoveTowards(position, _goalLine, step);
            }
        }
        else {
            if (_stall) return;
            transform.position = Vector2.MoveTowards(position, ballTrans.position, step);
        }
    }

    void FixedUpdate() {
        _possession = FindObjectOfType<ControllerBall>().p2Possession;    // test for expensive invocation
        PlayerMovement();
    }
}
