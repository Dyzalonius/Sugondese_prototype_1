using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : Ball {

    int bounceCount;
    public int maxBounceCount;

    public override void Start() {
        bounceCount = 0;
        base.Start();
    }

    public override void OnBounce() {
        bounceCount++;
        if (bounceCount == maxBounceCount) {
            Kill();
        }
    }

    public override void Kill() {
        bounceCount = 0;
        base.Kill();
    }
}
