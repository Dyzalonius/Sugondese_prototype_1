using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour {

    int score1, score2, lives1, lives2;
    [SerializeField] int scoreToWin, livesMax;
    [SerializeField] Text scoreText1, scoreText2;

	// Use this for initialization
	void Start () {
        score1 = 0;
        score2 = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveLife1(int teamID) {
        switch (teamID) {
            case 1:
                lives1--;
                if (lives1 == 0) {
                    score1++;
                    Reset();
                }
                break;
            case 2:
                lives2--;
                if (lives2 == 0) {
                    score2++;
                    Reset();
                }
                break;
        }
    }

    public void Reset() {
        lives1 = livesMax;
        lives2 = livesMax;
    }
}
