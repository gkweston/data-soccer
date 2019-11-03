using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;

public class ControllerBall : MonoBehaviour {

    public Transform Player1Transform, Player2Transform;

    // figure 8 variables for debug //
    public float rotateSpeed = 0.5f;
    private float _angle;
    public float radius = 2;
    
    // position vectors //
    private Vector2 _p1Goal, _p2Goal, _center;    
    
    // gen global booleans //
    [HideInInspector]
    public bool p1Possession, p2Possession;
    
    // distance to ball and goal //
    private float _p1DistanceToBall;        /*debug*/
    private float _p2DistanceToBall;
    private float _possessionRadius;
    
    // Shot mechanics //
    public float shotSpeed = 5f;
    public float sCurveConst = 20;    // defines 'A' in S-Curve for shooting mechanics
    private float _xComp, _yComp;   // for S-curve
    private float _randomNum, _shotRadius;
    private bool _p1TakeShot, _p2TakeShot;


    void Start() {
        
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
    
    void ShotMechanics() {
        // implements 1/2 of s-curve whose deviation from line of shot depends on _randomNum range
        for (var i = 10 - 1; i >= 0; i--)
        {
            
            _xComp = _randomNum * i;
            _yComp = (sCurveConst / (1 + Mathf.Exp(-i))) - (sCurveConst / 2);
    
            var shotPosition = transform.position;
            transform.position = Vector2.MoveTowards(shotPosition, new Vector2(_xComp, _yComp), shotSpeed);
    
            _xComp = shotPosition.x;
            _yComp = shotPosition.y;
        }
    }

    int Challenge() {
        /*
         * This method continuously adds and removes the winning player from a dictionary
         * however it generalized to n players better that purely comparison based operations
         * while retaining O(n) complexity
         */
        var player1Position = Player1Transform.position;
        var player2Position = Player2Transform.position;
        _p1DistanceToBall = Mathf.Abs((transform.position - player1Position).magnitude);
        _p2DistanceToBall = Mathf.Abs((transform.position - player2Position).magnitude);
        Dictionary<int, float> PlayerStaminas = new Dictionary<int, float>();
        
        if (_p1DistanceToBall < _possessionRadius){
            float player1Stamina = GetComponent<StaminaP1>().stamina;
            PlayerStaminas.Add(1, player1Stamina);
        }

        if (_p2DistanceToBall < _possessionRadius){
            float player2Stamina = GetComponent<StaminaP2>().stamina;
            PlayerStaminas.Add(2, player2Stamina);
        }
        
        /*
         * player_3
         * .
         * .
         * .
         * player_n
         */
        
        var winner = PlayerStaminas.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        PlayerStaminas.Clear();

        return winner;
    }


    
    void BallPosition() {
        // Ball makes a figure eight, useful to test player movement and possession bools
        // Debug fig-8
        while (!(p1Possession || p2Possession)) {
            _angle += rotateSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
            transform.position = _center + offset;
        }

        int currentPossession = Challenge();
        
        if (currentPossession == 1){
            transform.position = Player1Transform.position;
        }
        else if (currentPossession == 2){
            transform.position = Player2Transform.position;
        }
        /*
         * player_3
         * .
         * .
         * .
         * player_n
         */
        else if (currentPossession != 1 || currentPossession != 2){
            throw new InvalidOperationException("Invalid return from challenge");
        }

    }

    

    void Update() {
        BallPosition();
        
        // <Debug> //

    }
}
