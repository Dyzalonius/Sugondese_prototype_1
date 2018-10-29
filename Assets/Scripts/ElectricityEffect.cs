using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityEffect : MonoBehaviour {

    public float timeBeforeRemove, timeToRemove;
    float currentScaleFactor, currentTimeBeforeRemove, currentAlpha;
    Vector3 endScale;
    const float MAX_SCALE_FACTOR = 1f;
    const float MAX_ALPHA_FACTOR = 1f;
    [HideInInspector] public bool shocking;

    // Use this for initialization
    void Start () {
        currentScaleFactor = MAX_SCALE_FACTOR;
        currentAlpha = MAX_ALPHA_FACTOR;
        endScale = transform.localScale;
        UpdateScale();
        shocking = true;
    }
	
	// Update is called once per frame
	void Update () {
        FadeOut();
	}

    // Updating the scale of the particle
    void UpdateScale() {
        transform.localScale = endScale * currentScaleFactor;
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "water":
                other.GetComponent<WaterEffect>().Spark();
                break;
        }
    }

    // Fade the particle out
    void FadeOut() {
        if (currentTimeBeforeRemove < timeBeforeRemove) {
            currentTimeBeforeRemove += Time.fixedDeltaTime * Time.timeScale;
        } else {
            currentAlpha -= MAX_ALPHA_FACTOR * Time.fixedDeltaTime * Time.timeScale / timeToRemove;
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, MAX_ALPHA_FACTOR * Time.fixedDeltaTime * Time.timeScale / timeToRemove);

            if (currentAlpha <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
