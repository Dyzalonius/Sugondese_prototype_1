using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour {

    public float fadeInSpeed, fadeOutDelay, fadeOutSpeed;

    CanvasGroup canvasGroup;
    int state;
    float currentFadeOutDelay;

    [HideInInspector] public bool roundEndFade, titleScreenFade;
    [HideInInspector] public List<GameObject> objectsToFade;
    [HideInInspector] public List<CanvasGroup> canvasGroupsToFade;

    // Use this for initialization
    void Awake () {
        canvasGroup = GetComponent<CanvasGroup>();
        objectsToFade = new List<GameObject>();
        canvasGroupsToFade = new List<CanvasGroup>();
        roundEndFade = false;
        titleScreenFade = false;
        state = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (roundEndFade) {
            HandleFadeRoundEnd();
        }
        if (titleScreenFade) {
            HandleFadeTitleScreen();
        }
    }

    void HandleFadeRoundEnd() {
        switch (state) {
            case 0:
                if (canvasGroup.alpha < 1f) {
                    FadeIn();
                }
                else {
                    FinishFadeIn();
                }
                break;

            case 1:
                if (currentFadeOutDelay < fadeOutDelay) {
                    currentFadeOutDelay += Time.fixedUnscaledDeltaTime;
                } else {
                    FinishFadeOutDelay();
                }
                break;

            case 2:
                if (canvasGroup.alpha > 0f) {
                    FadeOut();
                }
                else {
                    FinishFadeOut();
                }
                break;
        }
    }

    void HandleFadeTitleScreen() {
        switch (state) {
            case 0:
                FadeInFull();
                FinishFadeIn();
                break;

            case 1:
                break;

            case 2:
                if (canvasGroup.alpha > 0f) {
                    FadeOut();
                }
                else {
                    FinishFadeOut();
                }
                break;
        }
    }

    void FadeIn() {
        canvasGroup.alpha += Time.fixedUnscaledDeltaTime / fadeInSpeed;

        for (int i = 0; i < objectsToFade.Count; i++) {
            objectsToFade[i].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, Time.fixedUnscaledDeltaTime / fadeInSpeed);
        }
        for (int i = 0; i < canvasGroupsToFade.Count; i++) {
            canvasGroupsToFade[i].alpha -= Time.fixedUnscaledDeltaTime / fadeInSpeed;
        }
    }

    void FadeInFull() {
        canvasGroup.alpha = 1;

        for (int i = 0; i < objectsToFade.Count; i++) {
            objectsToFade[i].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
        }
        for (int i = 0; i < canvasGroupsToFade.Count; i++) {
            canvasGroupsToFade[i].alpha = 0;
        }
    }

    void FadeOut() {
        canvasGroup.alpha -= Time.fixedUnscaledDeltaTime / fadeOutSpeed;

        for (int i = 0; i < objectsToFade.Count; i++) {
            objectsToFade[i].GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, Time.fixedUnscaledDeltaTime / fadeInSpeed);
        }
        for (int i = 0; i < canvasGroupsToFade.Count; i++) {
            canvasGroupsToFade[i].alpha += Time.fixedUnscaledDeltaTime / fadeInSpeed;
        }
    }

    void FinishFadeIn() {
        canvasGroup.alpha = 1f;
        state = 1;
    }

    void FinishFadeOutDelay() {
        state = 2;
        currentFadeOutDelay = 0f;
    }

    public void EndFadeTitleScreen() {
        state = 2;
    }
    
    void FinishFadeOut() {
        canvasGroup.alpha = 0f;
        titleScreenFade = false;
        roundEndFade = false;
        state = 0;
    }
}
