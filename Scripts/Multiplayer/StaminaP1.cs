using UnityEngine;

public class StaminaP1 : MonoBehaviour {
    
    public Transform playerTrans;
    private Vector2 lastPosition;
    private float deltaP;
    private const int maxStam = 5;
    private bool increase;
    
    [HideInInspector]
    public float stamina = maxStam;

    void Start() {
        lastPosition = transform.position;
    }
    
    private float GetStamina() {
        deltaP = Vector2.Distance(transform.position, lastPosition);

        if (stamina <= -5)
            increase = true;
        else if (stamina >= maxStam)
            increase = false;
        if (increase)
            stamina += Time.deltaTime;
        else
            stamina -= deltaP;
        
        return stamina;  
    }

    void Update() {
        stamina = GetStamina();
        lastPosition = transform.position;
    }
}


