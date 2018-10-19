﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int minBallCountMid, minBallCountTotal;
    public float resetDelay;
    public float[] team0Boundaries, team1Boundaries, team2Boundaries;
    public CanvasGroup scoreBoard, spotLight, fader;
    public GameObject player1, player2;
    public GameObject ball, ballBounce, ballCurve, ballWater;

    List<GameObject> balls;
    GameObject[] ballTypes;
    List<int>[] ballsToSpawn;
    float[][] ballSpawns;
    int nextBallSpawn;
    int ballsInCourt1, ballsInCourt2;
    
    [HideInInspector] public bool warmupLive, gameLive;
    [HideInInspector] public ScoreboardManager scoreboardManager;
    [HideInInspector] public SpotlightManager spotLightManager;
    [HideInInspector] public FadeManager fadeManager;
    [HideInInspector] public PlayerManager playerManager1, playerManager2;

    // Use this for initialization
    void Start () {
        balls = new List<GameObject>();
        ballTypes = new GameObject[] { ball, ballBounce, ballCurve, ballWater };
        ballsToSpawn = new List<int>[] { new List<int> { }, new List<int> { }, new List<int> { } };
        ballSpawns = new float[][] { new float[] { 0, -1.78f, 1.78f }, new float[] { -1.5f, 0, -3f, 1.5f, -4.5f } };
        nextBallSpawn = 0;
        warmupLive = false;
        gameLive = false;

        fadeManager = fader.GetComponent<FadeManager>();
        scoreboardManager = scoreBoard.GetComponent<ScoreboardManager>();
        spotLightManager = spotLight.GetComponent<SpotlightManager>();
        playerManager1 = player1.GetComponent<PlayerManager>();
        playerManager2 = player2.GetComponent<PlayerManager>();

        GenerateBallsToSpawn(0, 0);
        FillFadeManagerObjects();
        playerManager1.SetBoundaries(team1Boundaries);
        playerManager2.SetBoundaries(team2Boundaries);
        Invoke("StartWarmup", 0.1f); // needs a delay, to make sure that players are initialized
        //Invoke("StartGame", 10f); // needs a delay, to make sure that players are initialized
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameLive) {
            CheckHogg();
        }
    }

    // Generate ballsToSpawn for the next round
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

    // End countdown, start round
    public void StartRound() {
        playerManager1.countdownLive = false;
        playerManager2.countdownLive = false;
    }

    // End round, start reset after delay
    public void EndRound() {
        playerManager1.roundLive = false;
        playerManager2.roundLive = false;
        Invoke("DelayReset", resetDelay);
    }

    void DelayReset() {
        fadeManager.active = true;
        Invoke("StartGame", fadeManager.fadeInSpeed + fadeManager.fadeOutDelay);
    }

    public void EndWarmup() {
        warmupLive = false;
        fadeManager.active = true;
        Invoke("StartGame", fadeManager.fadeInSpeed + fadeManager.fadeOutDelay);
    }

    void FillFadeManagerObjects() {
        fadeManager.objectsToFade.Add(player1);
        fadeManager.objectsToFade.Add(player2);
        fadeManager.objectsToFade.Add(playerManager1.crosshair);
        fadeManager.objectsToFade.Add(playerManager2.crosshair);
        fadeManager.objectsToFade.Add(scoreboardManager.tutorial1.transform.GetChild(0).gameObject);
        fadeManager.objectsToFade.Add(scoreboardManager.tutorial2.transform.GetChild(0).gameObject);
        fadeManager.canvasGroupsToFade.Add(scoreboardManager.tutorial1.GetComponent<CanvasGroup>());
        fadeManager.canvasGroupsToFade.Add(scoreboardManager.tutorial2.GetComponent<CanvasGroup>());
        fadeManager.canvasGroupsToFade.Add(scoreboardManager.readyGroup1.GetComponent<CanvasGroup>());
        fadeManager.canvasGroupsToFade.Add(scoreboardManager.readyGroup2.GetComponent<CanvasGroup>());
    }

    void StartGame() {
        gameLive = true;
        scoreboardManager.StartGame();
        ResetRound();
    }

    void StartWarmup() {
        warmupLive = true;
    }

    // Reset round
    void ResetRound() {
        nextBallSpawn = 0;
        DeleteBalls();
        SpawnBalls();
        playerManager1.ResetRound();
        playerManager2.ResetRound();
        scoreboardManager.ResetRound();
    }

    // Spawn all balls from ballsToSpawn
    void SpawnBalls() {
        // middle balls
        nextBallSpawn = 0;
        for (int i = 0; i < ballsToSpawn[0].Count; i++) {
            CreateBall(ballsToSpawn[0][i], 0);
            nextBallSpawn++;
        }
        ballsToSpawn[0].Clear();

        // player 1 balls
        nextBallSpawn = ballSpawns[1].Length - 1;
        for (int i = 0; i < ballsToSpawn[1].Count; i++) {
            CreateBall(ballsToSpawn[1][i], 1);
            nextBallSpawn--;
        }
        ballsToSpawn[1].Clear();

        // player 2 balls
        nextBallSpawn = ballSpawns[1].Length - 1;
        for (int i = 0; i < ballsToSpawn[2].Count; i++) {
            CreateBall(ballsToSpawn[2][i], 2);
            nextBallSpawn--;
        }
        ballsToSpawn[2].Clear();
    }

    // Create an individual ball
    void CreateBall(int ballTypeIndex, int xPosition) {
        if (ballTypeIndex == ballTypes.Length) {
            ballTypeIndex = (int)Random.Range(1, ballTypes.Length - 0.01f);
        }
        GameObject ballType = ballTypes[ballTypeIndex];
        GameObject ball = Instantiate(ballType, new Vector3(ballSpawns[0][xPosition], ballSpawns[1][nextBallSpawn], 0), Quaternion.Euler(0, 0, 0));
        balls.Add(ball);
        fadeManager.objectsToFade.Add(ball);
    }

    // Delete all balls stored in balls
    void DeleteBalls() {
        for (int i = balls.Count - 1; i >= 0; i--) {
            var ball = balls[i];
            balls.Remove(ball);
            fadeManager.objectsToFade.Remove(ball);

            Destroy(ball);
        }
    }

    // Check if either team is hogging all the balls
    void CheckHogg() {
        int ballsInCourt1 = 0;
        int ballsInCourt2 = 0;

        // check for every ball on which side of the court they are
        for (int i = 0; i < balls.Count; i++) {
            float x = balls[i].transform.position.x;

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

}
