using UnityEngine;

public class ControllerP1 : MonoBehaviour {
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

    private StaminaP1 staminaMethod;
    
    [HideInInspector]
    public float playerStam;
    private int speed;
    
    // stall timer //
    private bool _stall = false;
    
    
    void Start() {
        _possession = false;   
        _goalLine.x = 0;    
        _goalLine.y = 5.5f;
    }
    
    // Implement stall based on challenge here:
    void PlayerMovement() {
        /*
         * Refactor based on Stamina/Challenge updates
         */
        var position = transform.position;
        _distanceToBall = Vector2.Distance(position, ballTrans.position);   
        _distanceToGoal = Vector2.Distance(position, _goalLine);
        
        // Stamina based speed:
        playerStam = staminaMethod.stamina;
        speed = playerStam > 0 ? 2 : 1;
        var step = speed * Time.deltaTime;    // movement velocity

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

    void Stall() {
        // stall after lost challenge, secondary to knockback method
    }

    void Knockback() {
        // knockback after lost challenge -> refer above
    }

    void FixedUpdate() {
        //_stall = GetComponent<ControllerBall>().p1Stall;
        _possession = GetComponent<ControllerBall>().p1Possession;

        PlayerMovement();
    }
}
