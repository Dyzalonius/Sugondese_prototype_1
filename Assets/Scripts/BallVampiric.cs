using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVampiric : Ball {

    protected override void OnBounce() {
        if (!dying) {
            LifeSteal();
        }

        base.OnBounce();
    }

    void LifeSteal() {
        // spawn thingy with a ghostly trail, moving to the hit player's hits, and removing 1
    }
}
