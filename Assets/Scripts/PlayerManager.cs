﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    
    public GameObject player, crosshair;
    public GameObject arena;
    public string inputType;
    public float maxSpeed;
    public int teamID;

    [HideInInspector]
    public bool countdownLive, roundLive;
    
    bool throwing;
    List<Ball> balls;
    float speed, minX, maxX, minY, maxY;
    Vector3 spawnPosition, aimDirection;
    Vector3[] ballPositions = new Vector3[] { new Vector3(0,0,0), new Vector3(-0.1f,-0.1f,0), new Vector3(0.1f,-0.1f,0) };
    Rigidbody2D rb;
    GameManager gameManager;

	// Use this for initialization
	void Start () {
        countdownLive = false;
        roundLive = true;
        throwing = false;
        balls = new List<Ball>();
        speed = maxSpeed;
        spawnPosition = gameObject.transform.position;
        aimDirection = new Vector3(0, 1, 0);

        rb = GetComponent<Rigidbody2D>();
        gameManager = arena.GetComponent<GameManager>();
        
        if (inputType != "") {
            inputType = "_" + inputType;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
        UpdateCrosshair();
        HandleFire();
    }
    
    public void SetBoundaries(float[] newBoundaries) {
        minX = newBoundaries[0];
        maxX = newBoundaries[1];
        minY = newBoundaries[2];
        maxY = newBoundaries[3];
    }

    void HandleFire() {
        if (Input.GetAxis("Fire" + inputType) > 0) {
            if (!throwing && roundLive) {
                Throw();
            }
            throwing = true;

            // handle fire during warmup
            if (gameManager.warmupLive) {
                gameManager.scoreboardManager.ReadyingUp(teamID);
            }
        }
        else {
            throwing = false;

            if (gameManager.warmupLive) {
                gameManager.scoreboardManager.ReadyingUpCancel(teamID);
            }
        }
    }

    // Player movement
    void Move() {
        Vector3 currentPos = rb.position;
        Vector3 direction = new Vector3(Input.GetAxis("MoveHorizontal"+inputType), Input.GetAxis("MoveVertical"+inputType), 0);
        direction.Normalize();
        direction *= Time.fixedDeltaTime * Time.timeScale * speed;

        if (countdownLive) {
            direction.x = 0;
        }

        // force player to stay within boundaries
        player.transform.position = new Vector2(
            Mathf.Clamp(direction.x + currentPos.x, minX, maxX),
            Mathf.Clamp(direction.y + currentPos.y, minY, maxY)
        );
    }

    // Update crosshair
    void UpdateCrosshair() {
        Vector3 newRotation = Vector3.right * Input.GetAxis("AimHorizontal"+inputType) + Vector3.up * Input.GetAxis("AimVertical"+inputType);

        // only update if atleast one of the two axes is being used
        if (newRotation.sqrMagnitude > 0.0f) {
            newRotation.Normalize();
            aimDirection = newRotation;
            crosshair.transform.rotation = Quaternion.LookRotation(Vector3.forward, newRotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "ball":
                Ball ball = other.gameObject.GetComponent<Ball>();

                // Pickup ball
                if (ball.onGround && balls.Count < 3) {
                    Pickup(ball);
                }

                // Get hit
                if (ball.flying) {
                    ball.OnBounce(transform.position);

                    if (roundLive) {
                        gameManager.spotLightManager.SetTarget(gameObject);
                        PlayerHit();
                    }
                }
                break;

            case "water":
                speed = maxSpeed * other.gameObject.GetComponent<WaterEffect>().speedReductionFactor;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "water":
                speed = maxSpeed;
                break;
        }
    }

    void PlayerHit() {
        gameManager.scoreboardManager.AddHit(teamID);
    }

    // Pickup ball
    void Pickup(Ball ball) {
        ball.transform.parent = player.transform;
        balls.Add(ball);
        ball.GetComponent<Ball>().Pickup();
        SortBalls();
    }

    // Sort balls
    void SortBalls() {
        for (int i = 0; i < balls.Count; i++) {
            balls[i].gameObject.transform.localPosition = ballPositions[i];
        }
    }

    // Throw ball
    void Throw() {
        if (balls.Count > 0) {
            var ball = balls[0];
            balls.RemoveAt(0);
            ball.gameObject.transform.parent = null;
            ball.Fire(aimDirection);
            SortBalls();
        }
    }

    public void ResetRound() {
        roundLive = true;
        countdownLive = true;
        balls.Clear();
        player.transform.position = spawnPosition;
    }
}
