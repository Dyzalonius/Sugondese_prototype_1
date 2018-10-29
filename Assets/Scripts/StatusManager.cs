using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

    [SerializeField] CanvasGroup statusBox;
    [SerializeField] float stunDuration;
    [SerializeField] GameObject stunBar;
    [HideInInspector] public bool stunned;

    float currentStunTime;

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 statusPos = Camera.main.WorldToScreenPoint(transform.position);
        statusBox.transform.position = statusPos;
	}

    private void FixedUpdate() {
        if (stunned) {
            HandleStun();
        }
    }

    public void Stun() {
        stunned = true;
        statusBox.alpha = 1f;
    }

    void HandleStun() {
        currentStunTime -= Time.fixedDeltaTime / stunDuration;
        stunBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentStunTime * 90f, 5f);

        // check if stun over
        if (currentStunTime <= 0f) {
            Reset();
        }
    }

    void Reset() {
        stunned = false;
        currentStunTime = stunDuration;
        statusBox.alpha = 0f;
        stunBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentStunTime * 90f, 5f);
    }
}
