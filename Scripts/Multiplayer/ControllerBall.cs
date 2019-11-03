using System;
using UnityEngine;
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
    private float _p1DistanceToBall;
    private float _p2DistanceToBall;
    private float _possessionRadius;
    
    // Shot mechanics //
    public float shotSpeed = 5f;
    public float sCurveConst = 20;
    private float _xComp, _yComp;
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
        
        _randomNum = UnityEngine.Random.Range(-0.3f, 0.3f);
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
         * while maintaining O(n)
         * Implicit team assignment prevents extra pull data from player classes
         * -> Team 2: even playerNum;
         * -> Team 1: odd playerNum;
         * For team stat based challenge either iterate through even/odds or     [ ]
         * compare max dictionary values per team, then iterate through team     [x]
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

    int GeneralizedChallenge() {
        var player1Position = Player1Transform.position;
        var player2Position = Player2Transform.position;
        _p1DistanceToBall = Mathf.Abs((transform.position - player1Position).magnitude);
        _p2DistanceToBall = Mathf.Abs((transform.position - player2Position).magnitude);

        Dictionary<int, float> team1Stamina = new Dictionary<int, float>();
        Dictionary<int, float> team2Stamina = new Dictionary<int, float>();

        if (_p1DistanceToBall < _possessionRadius){
            float p1Stam = GetComponent<StaminaP1>().stamina;
            team1Stamina.Add(1, p1Stam);
        }

        if (_p2DistanceToBall < _possessionRadius){
            float p2Stam = GetComponent<StaminaP2>().stamina;
            team2Stamina.Add(2, p2Stam);
        }
        /*
         * .
         * .
         * .
         * player_n
         */

        var team1Max = team1Stamina.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        var team2Max = team1Stamina.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

        if (team1Max > team2Max){
            List<int> randPick = new List<int>();
            foreach (KeyValuePair<int, float> entry in team1Stamina){
                for (int i = 0; i < entry.Value; ++i){
                    randPick.Add(entry.Key);
                }
            }

            var rand = new System.Random();
            int index = rand.Next(randPick.Count);
            return index;
        }
        if (team2Max > team1Max){
            List<int> randPick = new List<int>();
            foreach (KeyValuePair<int, float> entry in team2Stamina){
                for (int i = 0; i < entry.Value; ++i){
                    randPick.Add(entry.Key);
                }
            }

            var rand = new System.Random();
            int index = rand.Next(randPick.Count);
            return index;
        }
        throw new InvalidOperationException("Generalized challenge exception");
    }

    
    void BallPosition() {
        // Ball makes a figure eight, useful to test player movement and possession bools
        // Debug fig-8
        
        /*
         * May consolidate w/ challenge method
         */
        
        while (!(p1Possession || p2Possession)) {
            _angle += rotateSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(_angle) * Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;
            transform.position = _center + offset;
        }

        int currentPossession = Challenge();
        //int currentPossession = GeneralizedChallenge();
        
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
