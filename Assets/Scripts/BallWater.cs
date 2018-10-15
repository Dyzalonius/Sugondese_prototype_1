using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWater : Ball {
    
    public GameObject waterArea;

    protected override void OnBounce() {
        if (!dying) {
            Explode();
        }
        base.OnBounce();
    }

    void Explode() {
        Instantiate(waterArea, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
