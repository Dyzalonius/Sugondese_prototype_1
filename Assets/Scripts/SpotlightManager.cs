using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour {

    GameObject target;
    CanvasGroup canvasGroup;
    [SerializeField] float shrinkThreshold, fadeInSpeed, fadeOutSpeed, slowmoValue;
    public float shrinkTime;

    // Use this for initialization
    void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target != null) {
            FollowTarget();
        }
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
    }

    void RemoveTarget() {
        target = null;
        canvasGroup.alpha = 0f;
        transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 1;
    }

    void FollowTarget() {
        transform.position = target.transform.position;

        if (transform.localScale.x > shrinkThreshold) {
            // Shrink
            transform.localScale *= Mathf.Pow(shrinkThreshold, 1/(shrinkTime/Time.fixedUnscaledDeltaTime));

            // Fade in
            if (canvasGroup.alpha != 1) {
                canvasGroup.alpha += fadeInSpeed * Time.fixedUnscaledDeltaTime;
            }

            // Slow-mo
            Time.timeScale = slowmoValue;
        } else {
            // Fade out
            canvasGroup.alpha -= fadeOutSpeed * Time.fixedUnscaledDeltaTime;

            // Remove
            if (canvasGroup.alpha == 0) {
                RemoveTarget();
                // now you can reset the game
            }
        }
    }
}
