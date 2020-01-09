using UnityEngine;
using Random = UnityEngine.Random;
/* player index starting at 0 */
public class DB_BallCon : MonoBehaviour{
    /* private: */
    private int randomPossession;
    private float staminaP1, staminaP2;
    
    /* public: */
    public Transform player1Transform;
    public Transform player2Transform;
    public float possessionRadius;
    public static float playerPossRad;    // 0.1 - 0.15 is an acceptable radius for possession

    void BasicPossession(){
        var ballPosition = transform.position;
        var player1Position = player1Transform.position;
        var player2Position = player2Transform.position;

        var distanceP1 = Vector2.Distance(ballPosition, player1Position);
        var distanceP2 = Vector2.Distance(ballPosition, player2Position);

        if (distanceP1 < possessionRadius && distanceP2 < possessionRadius){
            randomPossession = Random.Range(0, 2);
            // ...continuous stamina system additions here

            print("random:" + randomPossession);
            transform.position = randomPossession % 2 == 0 ? player2Position : player1Position;
        }
        else
        {
            if (distanceP1 < possessionRadius) transform.position = player1Position;
            if (distanceP2 < possessionRadius) transform.position = player2Position;   
        }

    }

    void Start(){
        playerPossRad = possessionRadius;
        transform.position = new Vector3(0, 0, -1);
    }

    void Update(){ BasicPossession(); }
    
}
