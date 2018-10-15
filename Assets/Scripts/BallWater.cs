using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWater : BallBounce {

    public float timeBetweenWaterAreas;
    float currentTimeBetweenWaterAreas;
    public GameObject waterArea;
    bool isActive;

    protected override void Start() {
        isActive = false;
        base.Start();
    }

    protected override void Move() {
        if (isActive) {
            currentTimeBetweenWaterAreas += Time.fixedDeltaTime * Time.timeScale;
            if (currentTimeBetweenWaterAreas > timeBetweenWaterAreas) {
                currentTimeBetweenWaterAreas = 0;
                Explode();
            }
        }

        base.Move();
    }

    protected override void OnBounce() {
        if (!dying) {
            isActive = true;
            Explode();
        }

        base.OnBounce();
    }

    protected override void Kill() {
        isActive = false;
        base.Kill();
    }

    void Explode() {
        Instantiate(waterArea, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
