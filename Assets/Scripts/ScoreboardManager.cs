using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour {

    int score1, score2, hits1, hits2;
    public GameObject player1, player2;
    public int scoreToWin, hitsMax, countdownTimerMax, minBallCountMid, minBallCountTotal;
    public float resetDelay, maxCourtTimer;
    public Text hits1Text, hits2Text, score1Text, score2Text, time1Text, time2Text, countdownText1, countdownText2;
    public CanvasGroup spotlight;
    List<GameObject> balls;
    float[][] ballSpawns = new float[][] { new float[] {0, -1.78f, 1.78f }, new float[] {-1.5f, 0, -3f, 1.5f, -4.5f} };
    public GameObject ball, ballBounce, ballCurve, ballWater;
    GameObject[] ballTypes;
    List<int>[] ballsToSpawn = new List<int>[] { new List<int> { }, new List<int> { }, new List<int> { } };
    int nextBallSpawn, countdownTimer;
    float courtTimer1, courtTimer2;

    // Use this for initialization
    void Start () {
        score1 = 0;
        score2 = 0;
        hits1 = 0;
        hits2 = 0;
        courtTimer1 = maxCourtTimer;
        courtTimer2 = maxCourtTimer;
        UpdateText();
        balls = new List<GameObject>();
        ballTypes = new GameObject[] { ball, ballBounce, ballCurve, ballWater };
        nextBallSpawn = 0;
        countdownTimer = countdownTimerMax+1;
        GenerateBallsToSpawn(0, 0);
        Invoke("ResetRound", 0.1f); // needs a delay, to make sure that players are initialized
    }
	
	// Update is called once per frame
	void Update () {
        HandleCourtTimers();
	}

    void HandleCourtTimers() {
        int ballsInCourt1 = 0;
        int ballsInCourt2 = 0;

        // check for every ball on which side of the court they are
        for (int i = 0; i < balls.Count; i++) {
            float x = balls.ElementAt(i).transform.position.x;

            if (x <= 0) {
                ballsInCourt1++;
            }
            if (x >= 0) {
                ballsInCourt2++;
            }
        }

        // count up courttimer1 if there is no balls in court 1
        if (ballsInCourt1 == 0 && courtTimer2 > 0) {
            courtTimer2 -= Time.deltaTime;

            // if timer is over max, set to max
            if (courtTimer2 < 0) {
                courtTimer2 = 0;
            }

            // if timer is under half a second, blink
            if (courtTimer2 % 1 > 0.5) {
                time2Text.color = Color.clear;
            }
            else {
                time2Text.color = Color.white;
            }
            UpdateText();
        } else {
            if (courtTimer2 > 0) {
                time2Text.color = Color.gray;
            }
        }

        // count up courttimer2 if there is no balls in court 2
        if (ballsInCourt2 == 0 && courtTimer1 > 0) {
            courtTimer1 -= Time.deltaTime;

            // if timer is over max, set to max
            if (courtTimer1 < 0) {
                courtTimer1 = 0;
            }

            // if timer is over half a second, blink
            if (courtTimer1 % 1 > 0.5) {
                time1Text.color = Color.clear;
            }
            else {
                time1Text.color = Color.white;
            }
            UpdateText();
        } else {
            if (courtTimer1 > 0) {
                time1Text.color = Color.gray;
            }
        }

        // add score if either courtTimer is over max
        if (courtTimer1 == 0) {
            // increment, so it's not exactly max anymore (so it doesn't count twice)
            courtTimer1 -= 0.1f;
            spotlight.GetComponent<SpotlightManager>().SetTarget(time1Text.transform.position);
            AddScore(2);
        }
        if (courtTimer2 == 0) {
            // increment, so it's not exactly max anymore (so it doesn't count twice)
            courtTimer2 -= 0.1f;
            spotlight.GetComponent<SpotlightManager>().SetTarget(time2Text.transform.position);
            AddScore(1);
        }
    }

    public void RemoveLife(int teamID) {
        switch (teamID) {
            case 1:
                hits2++;
                UpdateText();

                if (hits2 == hitsMax) {
                    AddScore(2);
                }
                break;
            case 2:
                hits1++;
                UpdateText();

                if (hits1 == hitsMax) {
                    AddScore(1);
                }
                break;
        }
    }

    void AddScore(int teamID) {
        switch (teamID) {
            case 1:
                GenerateBallsToSpawn(2, hitsMax - hits2);
                score1++;
                UpdateText();
                EndRound();
                break;
            case 2:
                GenerateBallsToSpawn(1, hitsMax - hits1);
                score2++;
                UpdateText();
                EndRound();
                break;
        }
    }

    void UpdateText() {

        string hits1string = "";
        for (int i = 0; i < hits1; i++) {
            hits1string += "X";
        }
        for (int i = 0; i < hitsMax-hits1; i++) {
            hits1string += "-";
        }

        string hits2string = "";
        for (int i = 0; i < hits2; i++) {
            hits2string += "X";
        }
        for (int i = 0; i < hitsMax - hits2; i++) {
            hits2string += "-";
        }

        hits1Text.text = hits1string;
        hits2Text.text = hits2string;
        score1Text.text = "" + score1;
        score2Text.text = "" + score2;

        if (courtTimer1 < 10) {
            time1Text.text = "0:0" + (int)courtTimer1;
        } else {
            time1Text.text = "0:" + (int)courtTimer1;
        }

        if (courtTimer2 < 10) {
            time2Text.text = "0:0" + (int)courtTimer2;
        }
        else {
            time2Text.text = "0:" + (int)courtTimer2;
        }
    }

    void EndRound() {
        player1.GetComponent<PlayerManager>().roundLive = false;
        player2.GetComponent<PlayerManager>().roundLive = false;
        Invoke("ResetRound", resetDelay);
    }

    void ResetRound() {
        player1.GetComponent<PlayerManager>().ResetRound();
        player2.GetComponent<PlayerManager>().ResetRound();

        hits1 = 0;
        hits2 = 0;
        courtTimer1 = maxCourtTimer;
        courtTimer2 = maxCourtTimer;
        nextBallSpawn = 0;
        UpdateText();

        DeleteBalls();
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

    void GenerateBallsToSpawn(int loserID, int hitsLeft) {
        // spawn one ball on the losing side, for every life the winner had left
        for (int i = 0; i < hitsLeft; i++) {
            ballsToSpawn[loserID].Add(ballTypes.Length);
        }

        // calculate how many balls to spawn in the middle
        int midBallCount = minBallCountTotal - hitsLeft;
        if (midBallCount < minBallCountMid) {
            midBallCount = minBallCountMid;
        }

        // spawn balls in the middle
        for (int i = 0; i < midBallCount; i++) {
            ballsToSpawn[0].Add(0);
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
        if (ballTypeIndex == ballTypes.Length) {
            ballTypeIndex = (int)Random.Range(1, ballTypes.Length - 0.01f);
        }
        GameObject ballType = ballTypes[ballTypeIndex];
        balls.Add(Instantiate(ballType, new Vector3(ballSpawns[0][xPosition], ballSpawns[1][nextBallSpawn],0), Quaternion.Euler(0, 0, 0)));
    }

    void Countdown() {
        countdownTimer--;
        switch (countdownTimer) {
            case -1:
                countdownText1.text = "";
                countdownText2.text = "";
                countdownTimer = countdownTimerMax + 1;
                break;
            case 0:
                countdownText1.text = "GO!";
                countdownText2.text = "GO!";
                player1.GetComponent<PlayerManager>().countdownLive = false;
                player2.GetComponent<PlayerManager>().countdownLive = false;
                Invoke("Countdown", 1f);
                break;
            default:
                countdownText1.text = "" + countdownTimer;
                countdownText2.text = "" + countdownTimer;
                Invoke("Countdown", 1f);
                break;
        }
    }
}
