using UnityEngine;
/* player index starting at 0 */
public class DB_PlayerCon : MonoBehaviour{
    /* private: */
    private Vector2 goalPosition;
    private float playerStamina;
    private int teamNumber;

    /* public: */
    public int playerNumber;
    public Transform ballTransform;
    public static float possessionRadius = DB_BallCon.playerPossRad;
    public int standardSpeed;
    public int staminaBoost;

    private void AssignToTeam(){
        // (team 1 odd), (team 2 even)
        if (playerNumber % 2 == 0){
            goalPosition = new Vector2(0f, -5.5f);
            teamNumber = 2;
        }
        else{
            goalPosition = new Vector2(0f, 5.5f);
            teamNumber = 1;
        }
    }

    public int GetTeamNumber(){ return teamNumber; }

    private void PlayerStaminaUpdate(){ playerStamina = DB_StaminaHandler.GetStaminaList()[playerNumber]; }

    private void PlayerMovement(){
        PlayerStaminaUpdate();
        
        var hasStamina = playerStamina > 0;
        var boostSpeed = staminaBoost * standardSpeed;
        var playerSpeed = hasStamina ? standardSpeed : boostSpeed;
        var step = playerSpeed * Time.deltaTime;
        
        var ballPosition = ballTransform.position;
        var playerPosition = transform.position;

        var distanceToBall = Vector2.Distance(playerPosition, ballPosition);
        if (distanceToBall > possessionRadius){
            transform.position = Vector2.MoveTowards(playerPosition, ballPosition, step);
        }
        else{
            transform.position = Vector2.MoveTowards(playerPosition, goalPosition, step);
        }

    }

    void Start(){
        AssignToTeam();
        PlayerStaminaUpdate();
    }

    void Update(){ PlayerMovement(); }
}
