using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VampireManager : MonoBehaviour {

    [SerializeField] float speedMax;
    GameObject player1, player2;
    Text panel1, panel2;
    Vector3 targetPos;
    bool travelling;
    float speed;

	// Use this for initialization
	void Start () {
        travelling = true;
        speed = speedMax / 20;
	}
	
	// Update is called once per frame
	void Update () {
        if (travelling) {
            Move();
        } else {
            FadeOut();
        }
	}

    void Move() {
        Vector3 direction = targetPos - transform.position;

        if (direction.magnitude < speed) {
            travelling = false;
            transform.position = targetPos;
        }
        else {
            direction.Normalize();
            speed += speedMax / 100;
            transform.position += direction * speed;
        }
    }

    void FadeOut() {
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.1f);
    }

    public void Initialize(PlayerManager playerManager) {
        /*player1 = playerManager.gameManager.player1;
        player2 = playerManager.gameManager.player2;
        panel1 = playerManager.gameManager.scoreboardManager.hits1Text;
        panel2 = playerManager.gameManager.scoreboardManager.hits2Text;

        switch (playerManager.teamID) {
            case 1:
                transform.position = player1.transform.position;
                targetPos = panel1.transform.position;
                break;
            case 2:
                transform.position = player2.transform.position;
                targetPos = panel2.transform.position;
                break;
        }*/
    }
}
