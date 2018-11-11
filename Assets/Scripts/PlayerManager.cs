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
