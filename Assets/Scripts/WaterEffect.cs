using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour {

    public float timeToMaxScale, timeBeforeRemove, timeToRemove, speedReductionFactor, startScaleFactor;
    public Sprite electrocutedWaterSprite;
    float currentScaleFactor, currentTimeBeforeRemove, currentAlpha;
    Vector3 endScale;
    BallWater parent;
    const float MAX_SCALE_FACTOR = 1f;
    const float MAX_ALPHA_FACTOR = 1f;

    [HideInInspector] public bool isElectrocuted;

	// Use this for initialization
	void Start () {
        currentScaleFactor = startScaleFactor;
        currentAlpha = MAX_ALPHA_FACTOR;
        endScale = transform.localScale;
        isElectrocuted = false;
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

    public void Initialize(BallWater parent, bool isElectrocuted) {
        this.parent = parent;
        if (isElectrocuted) {
            Electrocute();
        }
    }

    // Making the water particle bigger per frame
    void IncreaseScale() {
        currentScaleFactor += MAX_SCALE_FACTOR * Time.fixedDeltaTime * Time.timeScale / timeToMaxScale;

        if (currentScaleFactor >= MAX_SCALE_FACTOR) {
            currentScaleFactor = MAX_SCALE_FACTOR;
        }

        UpdateScale();
    }

    // Updating the scale of the particle
    void UpdateScale() {
        transform.localScale = endScale * currentScaleFactor;
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

    public void Spark() {
        parent.Electrocute();
    }

    public void Electrocute() {
        isElectrocuted = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = electrocutedWaterSprite;
    }
}
