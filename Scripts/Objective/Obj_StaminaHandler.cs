using System.Collections.Generic;
using UnityEngine;
/* player index starting at 0 */
public class Obj_StaminaHandler : MonoBehaviour{

    /* private */
    private Vector3 p0LastPosition, p1LastPosition;
    private static int maxStaminaGet;
    private bool _p0IncreaseStam, _p1IncreaseStam;
    private static List<float> stamina_list = new List<float>();

    /* public */
    public Transform p0Trans, p1Trans;
    private float _p0Stamina, _p1Stamina;
    public int maxStamina;

    private float DeltaStamina(Transform playerTrans, Vector3 lastPosition, float stamina, ref bool increase){
        var position = playerTrans.position;
        var deltaPosition = Vector2.Distance(position, lastPosition);

        if (increase){
            if (stamina >= maxStamina){
                stamina -= deltaPosition;    // w.o. this deltaStamina will skip a frame on bounds
                increase = false;
            }
            else stamina += Time.deltaTime;

            return stamina;
        }

        if (stamina <= 0){
            stamina += Time.deltaTime;    // w.o. this deltaStamina will skip a frame on bounds
            increase = true;
        }
        else stamina -= deltaPosition;
        
        return stamina;
    }

    private void UpdateStaminaList(){
        /* Stamina values are indexed by player number */
        stamina_list.Clear();
        stamina_list.Add(DeltaStamina(p0Trans, p0LastPosition, _p0Stamina, ref _p0IncreaseStam));
        stamina_list.Add(DeltaStamina(p1Trans, p1LastPosition, _p0Stamina, ref _p1IncreaseStam));
    }

    private void UpdateLastPosition(){
        p0LastPosition = p0Trans.position;
        p1LastPosition = p1Trans.position;
    }
    
    private void InitPlayerScopeVars(){
        _p0Stamina = _p1Stamina = maxStamina;
        _p0IncreaseStam = _p1IncreaseStam = false;
        p0LastPosition = p0Trans.position;
        p1LastPosition = p1Trans.position;
    }

    public static List<float> GetStaminaList(){ return stamina_list; }

    void Start(){ InitPlayerScopeVars(); }

    void Update(){
        UpdateStaminaList();
        
        // This should be always be the final method in Update(){...}
        UpdateLastPosition();
    }
}
