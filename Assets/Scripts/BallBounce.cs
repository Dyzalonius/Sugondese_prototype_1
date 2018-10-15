using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : Ball {

    protected int bounceCount;
    public int maxBounceCount;

    protected override void Start() {
        bounceCount = 0;
        base.Start();
    }

    protected override void OnBounce() {
        bounceCount++;
        if (bounceCount == maxBounceCount) {
            Kill();
        }
    }

    protected override void Kill() {
        bounceCount = 0;
        base.Kill();
    }
}
