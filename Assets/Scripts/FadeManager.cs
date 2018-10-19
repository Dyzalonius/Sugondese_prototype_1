using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public float fadeInSpeed, fadeOutDelay, fadeOutSpeed;

    CanvasGroup canvasGroup;
    int state;
    float currentFadeOutDelay;

    [HideInInspector] public bool active;
    [HideInInspector] public List<GameObject> objectsToFade;
    [HideInInspector] public List<CanvasGroup> canvasGroupsToFade;

    // Use this for initialization
    void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
        objectsToFade = new List<GameObject>();
        canvasGroupsToFade = new List<CanvasGroup>();
        active = false;
        state = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active) {
            HandleFade();
        }
	}

    void HandleFade() {
        switch (state) {
            case 0:
                if (canvasGroup.alpha < 1f) {
                    canvasGroup.alpha += Time.fixedUnscaledDeltaTime / fadeInSpeed;

                    for (int i = 0; i < objectsToFade.Count; i++) {
                        objectsToFade[i].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, Time.fixedUnscaledDeltaTime / fadeInSpeed);
                    }
                    for (int i = 0; i < canvasGroupsToFade.Count; i++) {
                        canvasGroupsToFade[i].alpha -= Time.fixedUnscaledDeltaTime / fadeInSpeed;
                    }
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
                    canvasGroup.alpha -= Time.fixedUnscaledDeltaTime / fadeOutSpeed;

                    for (int i = 0; i < objectsToFade.Count; i++) {
                        objectsToFade[i].GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, Time.fixedUnscaledDeltaTime / fadeInSpeed);
                    }
                    for (int i = 0; i < canvasGroupsToFade.Count; i++) {
                        canvasGroupsToFade[i].alpha += Time.fixedUnscaledDeltaTime / fadeInSpeed;
                    }
                }
                else {
                    FinishFadeOut();
                }
                break;
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

    void FinishFadeOut() {
        canvasGroup.alpha = 0f;
        active = false;
        state = 0;
    }
}
