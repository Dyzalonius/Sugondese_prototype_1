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
    [SerializeField] Text lives1Text, lives2Text, score1Text, score2Text, countdownText;
    List<GameObject> balls;
    Vector3[] ballSpawns = new Vector3[] { new Vector3(0, -1.5f, 0), new Vector3(0, 0, 0), new Vector3(0, -3f, 0) };
    public GameObject ball;
    int nextBallSpawn, countdownTimer;

    // Use this for initialization
    void Start () {
        score1 = 0;
        score2 = 0;
        lives1 = livesMax;
        lives2 = livesMax;
        UpdateText();
        balls = new List<GameObject>();
        nextBallSpawn = 0;
        countdownTimer = countdownTimerMax+1;
        ResetRound();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveLife(int teamID) {
        switch (teamID) {
            case 1:
                lives1--;
                UpdateText();

                if (lives1 == 0) {
                    score2++;
                    UpdateText();
                    EndRound();
                }
                break;
            case 2:
                lives2--;
                UpdateText();

                if (lives2 == 0) {
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

    void SpawnBalls() {
        nextBallSpawn = 0;
        CreateBall();
        CreateBall();
        CreateBall();
    }

    void CreateBall() {
        balls.Add(Instantiate(ball, ballSpawns[nextBallSpawn], Quaternion.Euler(0,0,0)));
        nextBallSpawn++;
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
