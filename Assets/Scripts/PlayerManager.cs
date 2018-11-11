using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    /*
    [SerializeField] AudioSource audioSourceThrow;
    [SerializeField] int teamID;

    [HideInInspector] public bool countdownLive;
    
    bool stunned;
    float minX, maxX, minY, maxY;
    Vector3 spawnPosition;

	// Use this for initialization
	void Start () {
        countdownLive = false;
        stunned = false;
        spawnPosition = gameObject.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (stunned) {
            CheckStun();
        }
    }

    public void SetBoundaries(float[] newBoundaries) {
        minX = newBoundaries[0];
        maxX = newBoundaries[1];
        minY = newBoundaries[2];
        maxY = newBoundaries[3];
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
                    ball.OnBounce(this);

                    if (roundLive) {
                        //gameManager.spotLightManager.SetTarget(gameObject);
                        PlayerHit();
                    }
                }
                break;

            case "water":
                WaterEffect waterEffect = other.gameObject.GetComponent<WaterEffect>();
                if (waterEffect.isElectrocuted) {
                    if (!stunned) {
                        Stun();
                    }
                }
                else {
                    speed = maxSpeed * waterEffect.speedReductionFactor;
                }
                break;

            case "electricity":
                if (!stunned) {
                    Stun();
                }
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "water":
                if (!stunned) {
                    speed = maxSpeed;
                }
                break;
        }
    }

    void Stun() {
        speed = 0f;
        statusManager.Stun();
        stunned = true;
    }

    void CheckStun() {
        /*if (statusManager.stunned == false) {
            // stop stun
            stunned = false;
            speed = maxSpeed;
        }
    }

    void PlayerHit() {
        //gameManager.scoreboardManager.AddHit(teamID);
    }

    public void ResetRound() {
        roundLive = true;
        countdownLive = true;
        balls.Clear();
        player.transform.position = spawnPosition;
    }
    */
}
