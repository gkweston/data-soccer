using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeHandler : MonoBehaviour {
    
    /*
     * Defined to be expanded if post-possession challenges are implemented
     */
    
    public Transform Player1Transform, Player2Transform;
    public float challengeRadius;

    private float distance, p1Stamina, p2Stamina;
    private int rand;
    public int possession;
    private Vector2 p1Position, p2Position;
    private class PossessionError { };

    private int PossessionCheck() {
        p1Position = Player1Transform.position;
        p2Position = Player2Transform.position;
        distance = Mathf.Abs((p1Position - p2Position).magnitude);

        if (distance > challengeRadius) PossessionCheck();    // Recursive application works???
        
        p1Stamina = GetComponent<StaminaP1>().stamina;
        p2Stamina = GetComponent<StaminaP2>().stamina;
            
        if (p1Stamina > p2Stamina) return 1;
        if (p2Stamina > p1Stamina) return 2;
            
        rand = Random.Range(0, 100);
        return rand % 2 == 0 ? 1 : 2;
        }

    void Update() {
        possession = PossessionCheck();
    }
    
}
