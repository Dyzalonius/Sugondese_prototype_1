using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour {

    public CanvasGroup titleScreen, allowInputText;
    public GameObject arena;
    public float inputDelay;

    GameManager gameManager;
    bool allowInput;

    // Use this for initialization
    void Start () {
        gameManager = arena.GetComponent<GameManager>();
        titleScreen.alpha = 1;
        allowInputText.alpha = 0;
        allowInput = false;
        Invoke("AllowInput", inputDelay);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameManager.titleScreenLive) {
            // tween move
            float movementFloat = Mathf.Sin(Time.fixedTime * 3);
            titleScreen.transform.position += new Vector3(0, movementFloat * 0.007f, 0);
        }

        if (allowInput) {
            if (gameManager.titleScreenLive && Input.anyKey) {
                End();
            }

            // tween blink
            float blinkFloat = Mathf.Sin(Time.fixedTime * 5);
            if (blinkFloat > 0) {
                allowInputText.alpha = 1;
            }
            else {
                allowInputText.alpha = 0;
            }
        } else {
            allowInputText.alpha = 0;
        }
    }

    void AllowInput() {
        allowInput = true;
    }

    void End() {
        allowInput = false;
        titleScreen.alpha = 0;
        allowInputText.alpha = 0;
        gameManager.EndTitleScreen();
    }
}
