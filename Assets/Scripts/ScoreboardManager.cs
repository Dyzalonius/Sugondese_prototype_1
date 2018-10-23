using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour {

    public int scoreToWin, hitsMax, countdownTimerMax;
    public float maxCourtTimer, timeToReadyUp, readyUpDeadzone;
    public Text hits1Text, hits2Text, score1Text, score2Text, time1Text, time2Text, countdownText1, countdownText2;
    public GameObject readyGroup1, readyGroup2, readyBlock1, readyBlock2, tutorial1, tutorial2, arena;
    public CanvasGroup spotlight;

    int score1, score2, hits1, hits2;
    int countdownTimer;
    float courtTimer1, courtTimer2, ready1, ready2;
    GameManager gameManager;

    // Use this for initialization
    void Start() {
        score1 = 0;
        score2 = 0;
        hits1 = 0;
        hits2 = 0;
        courtTimer1 = maxCourtTimer;
        courtTimer2 = maxCourtTimer;
        UpdateText();
        countdownTimer = countdownTimerMax + 1;

        gameManager = arena.GetComponent<GameManager>();

        StartWarmup();
        ReadyingUpCancel(1);
        ReadyingUpCancel(2);
    }

    void FixedUpdate() {
        if (gameManager.warmupLive) {
            float movementFloat1 = Mathf.Sin(Time.fixedTime * 3);
            float movementFloat2 = Mathf.Sin(Time.fixedTime * 3 - 1.5f);
            readyGroup1.transform.position += new Vector3(0, movementFloat1 * 0.007f, 0);
            readyGroup2.transform.position += new Vector3(0, movementFloat2 * 0.007f, 0);
        }
    }

    public void TurnOn() {
        hits1Text.color = Color.gray;
        hits2Text.color = Color.gray;
        score1Text.color = Color.white;
        score2Text.color = Color.white;
        time1Text.color = Color.gray;
        time2Text.color = Color.gray;
    }

    public void TurnOff() {
        hits1Text.color = Color.clear;
        hits2Text.color = Color.clear;
        score1Text.color = Color.clear;
        score2Text.color = Color.clear;
        time1Text.color = Color.clear;
        time2Text.color = Color.clear;
    }

    public void Hogg(int teamID) {
        switch (teamID) {
            case 1:
                // count up courttimer2 if there is no balls in court 2
                if (courtTimer1 > 0) {
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
                }

                // add score if either courtTimer is over max
                if (courtTimer1 == 0) {
                    // increment, so it's not exactly max anymore (so it doesn't count twice)
                    courtTimer1 -= 0.1f;
                    spotlight.GetComponent<SpotlightManager>().SetTarget(time1Text.transform.position);
                    AddScore(2);
                }
                break;

            case 2:
                // count up courttimer1 if there is no balls in court 1
                if (courtTimer2 > 0) {
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
                }
                if (courtTimer2 == 0) {
                    // increment, so it's not exactly max anymore (so it doesn't count twice)
                    courtTimer2 -= 0.1f;
                    spotlight.GetComponent<SpotlightManager>().SetTarget(time2Text.transform.position);
                    AddScore(1);
                }
                break;
        }
    }

    public void NotHogg(int teamID) {
        switch (teamID) {
            case 1:
                if (courtTimer1 > 0) {
                    time1Text.color = Color.gray;
                }
                break;
            case 2:
                if (courtTimer2 > 0) {
                    time2Text.color = Color.gray;
                }
                break;
        }
    }

    public void ReadyingUp(int teamID) {
        switch (teamID) {
            case 1:
                ready1 += Time.fixedDeltaTime / timeToReadyUp;
                if (ready1 > 1f) {
                    ready1 = 1f;
                }
                readyBlock1.GetComponent<RectTransform>().sizeDelta = new Vector2((ready1 - readyUpDeadzone) * 2.4f * (1f / (1f - readyUpDeadzone)), 0.4f);
                break;
            case 2:
                ready2 += Time.fixedDeltaTime / timeToReadyUp;
                if (ready2 > 1f) {
                    ready2 = 1f;
                }
                readyBlock2.GetComponent<RectTransform>().sizeDelta = new Vector2((ready2 - readyUpDeadzone) * 2.4f * (1f / (1f - readyUpDeadzone)), 0.4f);
                break;
        }

        //if (ready1 >= 1f && ready2 >= 1f) { //Multiplayer
        if (ready1 >= 1f) { //Singleplayer
            gameManager.EndWarmup();
        }
    }

    public void ReadyingUpCancel(int teamID) {
        switch (teamID) {
            case 1:
                ready1 = 0;
                readyBlock1.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0.4f);
                break;

            case 2:
                ready2 = 0;
                readyBlock2.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0.4f);
                break;
        }
    }

    public void StartGame() {
        readyGroup1.SetActive(false);
        readyGroup2.SetActive(false);
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        TurnOn();
    }

    public void StartWarmup() {
        readyGroup1.SetActive(true);
        readyGroup2.SetActive(true);
        tutorial1.SetActive(true);
        tutorial2.SetActive(true);
        TurnOff();
    }

    public void ResetRound() {
        hits1 = 0;
        hits2 = 0;
        courtTimer1 = maxCourtTimer;
        courtTimer2 = maxCourtTimer;
        UpdateText();
        Invoke("Countdown", gameManager.lightManager.fadeOutDelay);
    }

    public void AddHit(int teamID) {
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
                score1++;
                gameManager.GenerateBallsToSpawn(2, hitsMax - hits2);
                UpdateText();
                gameManager.EndRound();
                break;
            case 2:
                score2++;
                gameManager.GenerateBallsToSpawn(1, hitsMax - hits1);
                UpdateText();
                gameManager.EndRound();
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
                gameManager.StartRound();
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
