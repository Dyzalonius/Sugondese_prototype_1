using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int minBallCountMid, minBallCountTotal;
    public float resetDelay;
    public CanvasGroup scoreBoard;
    public GameObject player1, player2;
    public GameObject ball, ballBounce, ballCurve, ballWater;

    List<GameObject> balls;
    GameObject[] ballTypes;
    List<int>[] ballsToSpawn;
    float[][] ballSpawns;
    int nextBallSpawn;
    int ballsInCourt1, ballsInCourt2;

    ScoreboardManager scoreboardManager;
    PlayerManager playerManager1, playerManager2;

    // Use this for initialization
    void Start () {
        balls = new List<GameObject>();
        ballTypes = new GameObject[] { ball, ballBounce, ballCurve, ballWater };
        ballsToSpawn = new List<int>[] { new List<int> { }, new List<int> { }, new List<int> { } };
        ballSpawns = new float[][] { new float[] { 0, -1.78f, 1.78f }, new float[] { -1.5f, 0, -3f, 1.5f, -4.5f } };
        nextBallSpawn = 0;

        scoreboardManager = scoreBoard.GetComponent<ScoreboardManager>();
        playerManager1 = player1.GetComponent<PlayerManager>();
        playerManager2 = player2.GetComponent<PlayerManager>();

        GenerateBallsToSpawn(0, 0);
        Invoke("StartRound", 0.1f); // needs a delay, to make sure that players are initialized
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckHogg();
    }

    public void GenerateBallsToSpawn(int loserID, int hitsLeft) {
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

    public void EndRound() {
        playerManager1.roundLive = false;
        playerManager2.roundLive = false;
        Invoke("StartRound", resetDelay);
    }

    public void EndCountDown() {
        playerManager1.roundLive = true;
        playerManager2.roundLive = true;
        playerManager1.countdownLive = false;
        playerManager2.countdownLive = false;
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
        balls.Add(Instantiate(ballType, new Vector3(ballSpawns[0][xPosition], ballSpawns[1][nextBallSpawn], 0), Quaternion.Euler(0, 0, 0)));
    }

    void DeleteBalls() {
        for (int i = balls.Count - 1; i >= 0; i--) {
            var ball = balls.ElementAt(i);
            balls.RemoveAt(i);
            Destroy(ball);
        }
    }

    void CheckHogg() {
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

        if (ballsInCourt1 == 0) {
            scoreboardManager.Hogg(2);
        }
        else {
            scoreboardManager.NotHogg(2);
        }

        if (ballsInCourt2 == 0) {
            scoreboardManager.Hogg(1);
        }
        else {
            scoreboardManager.NotHogg(1);
        }
    }

    void StartRound () {
        nextBallSpawn = 0;
        DeleteBalls();
        SpawnBalls();
        playerManager1.StartRound();
        playerManager2.StartRound();
        scoreboardManager.StartRound();
    }

    public void EndCountdown() {
        playerManager1.countdownLive = false;
        playerManager2.countdownLive = false;
    }
}
