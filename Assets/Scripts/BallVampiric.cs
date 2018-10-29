using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVampiric : Ball {

    protected override void Start() {
        base.Start();
    }

    public override void OnBounce(PlayerManager playerManager) {
        LifeSteal(playerManager);

        base.OnBounce(playerManager);
    }

    void LifeSteal(PlayerManager playerManager) {
        ScoreboardManager scoreboardManager = playerManager.gameManager.scoreboardManager;

        if (playerManager.teamID == 1) {
            if (scoreboardManager.hits2 > 0) {
                scoreboardManager.RemoveHit(2);
            }
        }
        else {
            if (scoreboardManager.hits1 > 0) {
                scoreboardManager.RemoveHit(1);
            }
        }
    }
}
