﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    
    [SerializeField] GameObject player;
    [SerializeField] GameObject crosshair;
    [SerializeField] CanvasGroup spotlight, scoreboard;
    [SerializeField] string throwInput, moveHorizontalInput, moveVerticalInput, aimHorizontalInput, aimVerticalInput;
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] int teamID;

    Rigidbody2D rb;
    Vector3 aimDirection;
    List<GameObject> balls;
    bool throwing;
    Vector3[] ballPositions = new Vector3[] { new Vector3(0,0,0), new Vector3(-0.1f,-0.1f,0), new Vector3(0.1f,-0.1f,0) };
    Vector3 spawnPosition;
    public bool roundLive, countdownLive;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        aimDirection = new Vector3(0, 1, 0);
        balls = new List<GameObject>();
        spawnPosition = transform.position;
        roundLive = true;
        countdownLive = false;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        UpdateCrosshair();

        if (Input.GetAxis(throwInput) > 0) {
            if (!throwing && roundLive) {
                Throw();
            }
            throwing = true;
        } else {
            throwing = false;
        }
    }

    // Player movement
    void Move() {
        var currentPos = rb.position;

        var x = Input.GetAxis(moveHorizontalInput) * Time.deltaTime * Time.timeScale * 5.0f;
        var y = Input.GetAxis(moveVerticalInput) * Time.deltaTime * Time.timeScale * 5.0f;

        if (countdownLive) {
            x = 0;
        }

        // force player to stay within boundaries
        player.transform.position = new Vector2(
            Mathf.Clamp(x + currentPos.x, minX, maxX),
            Mathf.Clamp(y + currentPos.y, minY, maxY)
        );
    }

    // Update crosshair
    void UpdateCrosshair() {
        Vector3 newRotation = Vector3.right * Input.GetAxis(aimHorizontalInput) + Vector3.up * Input.GetAxis(aimVerticalInput);

        // only update if atleast one of the two axes is being used
        if (newRotation.sqrMagnitude > 0.0f) {
            newRotation.Normalize();
            aimDirection = newRotation;
            crosshair.transform.rotation = Quaternion.LookRotation(Vector3.forward, newRotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ballNew") {
            // Pickup ball
            if (other.gameObject.GetComponent<Ball>().onGround && balls.Count < 3) {
                Pickup(other.gameObject);
            }

            // Get hit
            if (other.gameObject.GetComponent<Ball>().flying) {
                other.gameObject.GetComponent<Ball>().OnBounce(transform.position);

                if (roundLive) {
                    spotlight.GetComponent<SpotlightManager>().SetTarget(gameObject);
                    PlayerHit();
                }
            }
        }
    }

    void PlayerHit() {
        scoreboard.GetComponent<ScoreboardManager>().RemoveLife(teamID);
    }

    // Pickup ball
    void Pickup(GameObject ball) {
        ball.transform.parent = player.transform;
        balls.Add(ball);
        ball.GetComponent<Ball>().Pickup();
        SortBalls();
    }

    // Sort balls
    void SortBalls() {
        for (int i = 0; i < balls.Count; i++) {
            balls[i].transform.localPosition = ballPositions[i];
        }
    }

    // Throw ball
    void Throw() {
        if (balls.Count > 0) {
            var ball = balls[0];
            balls.RemoveAt(0);
            ball.transform.parent = null;
            ball.GetComponent<Ball>().Fire(aimDirection);
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
