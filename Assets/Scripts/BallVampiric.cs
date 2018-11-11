using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVampiric : Ball {

    [SerializeField] GameObject vampirePrefab;
    PlayerController playerController;

    protected override void Start() {
        base.Start();
    }

    public override void OnBounce(PlayerController playerController) {
        this.playerController = playerController;
        SpawnVampire();

        base.OnBounce(playerController);
    }

    void SpawnVampire() {
        GameObject vampire = Instantiate(vampirePrefab, playerController.transform.position, Quaternion.Euler(0, 0, 0));
        vampire.GetComponent<VampireManager>().Initialize(playerController);
        Invoke("LifeSteal", 0.4f);
    }

    void LifeSteal() {
        /*ScoreboardManager scoreboardManager = playerManager.gameManager.scoreboardManager;

        if (playerManager.teamID == 1) {
            if (scoreboardManager.hits2 > 0) {
                scoreboardManager.RemoveHit(2);
            }
        }
        else {
            if (scoreboardManager.hits1 > 0) {
                scoreboardManager.RemoveHit(1);
            }
        }*/
    }
}
