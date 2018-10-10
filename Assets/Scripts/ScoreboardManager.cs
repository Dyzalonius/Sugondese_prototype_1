using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour {

    int score1, score2, lives1, lives2;
    [SerializeField] GameObject player1, player2;
    [SerializeField] int scoreToWin, livesMax, countdownTimerMax;
    [SerializeField] float resetDelay;
    [SerializeField] Text lives1Text, lives2Text, score1Text, score2Text, time1Text, time2Text, countdownText;
    List<GameObject> balls;
    float[][] ballSpawns = new float[][] { new float[] {0, -1.245f, 1.245f }, new float[] {-1.5f, 0, -3f, 1.5f, -4.5f} };
    public GameObject ball, ballBounce, ballCurve;
    GameObject[] ballTypes;
    List<int>[] ballsToSpawn = new List<int>[] { new List<int> { }, new List<int> { }, new List<int> { } };
    int nextBallSpawn, countdownTimer;
    float courtTimer1, courtTimer2;

    // Use this for initialization
    void Start () {
        score1 = 0;
        score2 = 0;
        lives1 = livesMax;
        lives2 = livesMax;
        courtTimer1 = 0;
        courtTimer2 = 0;
        UpdateText();
        balls = new List<GameObject>();
        ballTypes = new GameObject[] { ball, ballBounce, ballCurve };
        nextBallSpawn = 0;
        countdownTimer = countdownTimerMax+1;
        GenerateBallsToSpawn(0, 2);
        ResetRound();
    }
	
	// Update is called once per frame
	void Update () {
        HandleCourtTimers();
	}

    void HandleCourtTimers() {
        int ballsInCourt1 = 0;
        int ballsInCourt2 = 0;


        for (int i = 0; i < balls.Count; i++) {
            float x = balls.ElementAt(i).transform.position.x;

            if (x <= 0) {
                ballsInCourt1++;
            }

            if (x >= 0) {
                ballsInCourt2++;
            }
        }

        if (ballsInCourt1 == 0) {
            courtTimer1 += Time.deltaTime;
            UpdateText();
        }

        if (ballsInCourt2 == 0) {
            courtTimer2 += Time.deltaTime;
            UpdateText();
        }
    }

    public void RemoveLife(int teamID) {
        switch (teamID) {
            case 1:
                lives1--;
                UpdateText();

                if (lives1 == 0) {
                    GenerateBallsToSpawn(1, lives2);
                    score2++;
                    UpdateText();
                    EndRound();
                }
                break;
            case 2:
                lives2--;
                UpdateText();

                if (lives2 == 0) {
                    GenerateBallsToSpawn(2, lives1);
                    score1++;
                    UpdateText();
                    EndRound();
                }
                break;
        }
    }

    void UpdateText() {
        score1Text.text = "" + score1;
        score2Text.text = "" + score2;
        lives1Text.text = "Lives: " + lives1;
        lives2Text.text = "Lives: " + lives2;
        time1Text.text = "" + (int) courtTimer1 + "s";
        time2Text.text = "" + (int) courtTimer2 + "s";
    }

    void EndRound() {
        lives1 = livesMax;
        lives2 = livesMax;
        player1.GetComponent<PlayerManager>().roundLive = false;
        player2.GetComponent<PlayerManager>().roundLive = false;
        Invoke("ResetRound", resetDelay);
    }

    void ResetRound() {
        player1.GetComponent<PlayerManager>().ResetRound();
        player2.GetComponent<PlayerManager>().ResetRound();
        UpdateText();
        DeleteBalls();
        nextBallSpawn = 0;
        SpawnBalls();
        Countdown();
    }

    void DeleteBalls() {
        for (int i = balls.Count - 1; i >= 0; i--) {
            var ball = balls.ElementAt(i);
            balls.RemoveAt(i);
            Destroy(ball);
        }
    }

    void GenerateBallsToSpawn(int loserID, int winnerLivesLeft) {
        ballsToSpawn[0].Add(1);
        ballsToSpawn[0].Add(0);
        ballsToSpawn[0].Add(0);
        
        for (int i = 0; i < winnerLivesLeft; i++) {
            ballsToSpawn[loserID].Add(2);
        }
    }

    void SpawnBalls() {
        // middle balls
        nextBallSpawn = 0;
        for (int i = 0; i < ballsToSpawn[0].Count; i++) {
            CreateBall(ballsToSpawn[0].ElementAt(i), 0);
            nextBallSpawn++;
        }
        ballsToSpawn[0].Clear();

        // player 1 balls
        nextBallSpawn = ballSpawns[1].Length - 1;
        for (int i = 0; i < ballsToSpawn[1].Count; i++) {
            CreateBall(ballsToSpawn[1].ElementAt(i), 1);
            nextBallSpawn--;
        }
        ballsToSpawn[1].Clear();

        // player 2 balls
        nextBallSpawn = ballSpawns[1].Length - 1;
        for (int i = 0; i < ballsToSpawn[2].Count; i++) {
            CreateBall(ballsToSpawn[2].ElementAt(i), 2);
            nextBallSpawn--;
        }
        ballsToSpawn[2].Clear();
    }

    void CreateBall(int ballTypeIndex, int xPosition) {
        GameObject ballType = ballTypes[ballTypeIndex];
        balls.Add(Instantiate(ballType, new Vector3(ballSpawns[0][xPosition], ballSpawns[1][nextBallSpawn],0), Quaternion.Euler(0, 0, 0)));
    }

    void Countdown() {
        countdownTimer--;
        switch (countdownTimer) {
            case -1:
                countdownText.text = "";
                countdownTimer = countdownTimerMax + 1;
                break;
            case 0:
                countdownText.text = "GO!";
                player1.GetComponent<PlayerManager>().countdownLive = false;
                player2.GetComponent<PlayerManager>().countdownLive = false;
                Invoke("Countdown", 1f);
                break;
            default:
                countdownText.text = "" + countdownTimer;
                Invoke("Countdown", 1f);
                break;
        }
    }
}
