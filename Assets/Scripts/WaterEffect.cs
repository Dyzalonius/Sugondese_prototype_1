using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour {

    public GameObject player1, player2;
    public float timeToMaxScale, timeBeforeRemove, timeToRemove, normalAlpha;
    float currentScaleFactor, currentTimeBeforeRemove, currentAlpha;
    Vector3 endScale;
    const float MAX_SCALE_FACTOR = 1f;
    const float MAX_ALPHA_FACTOR = 1f;
    public bool active;

	// Use this for initialization
	void Start () {
        currentScaleFactor = 0;
        currentAlpha = MAX_ALPHA_FACTOR;
        endScale = transform.localScale;
        active = true;
        UpdateScale();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentScaleFactor < MAX_SCALE_FACTOR) {
            IncreaseScale();
        } else {
            FadeOut();
        }

	}

    void IncreaseScale() {
        currentScaleFactor += MAX_SCALE_FACTOR * Time.fixedDeltaTime * Time.timeScale / timeToMaxScale;

        if (currentScaleFactor >= MAX_SCALE_FACTOR) {
            currentScaleFactor = MAX_SCALE_FACTOR;
            active = false;
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, MAX_ALPHA_FACTOR - normalAlpha);
        }

        UpdateScale();
    }

    void UpdateScale() {
        transform.localScale = endScale * currentScaleFactor;
    }

    void FadeOut() {

        if (currentTimeBeforeRemove < timeBeforeRemove) {
            currentTimeBeforeRemove += Time.fixedDeltaTime * Time.timeScale;
        } else {
            currentAlpha -= normalAlpha * Time.fixedDeltaTime * Time.timeScale / timeToRemove;
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, normalAlpha * Time.fixedDeltaTime * Time.timeScale / timeToRemove);

            if (currentAlpha <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
