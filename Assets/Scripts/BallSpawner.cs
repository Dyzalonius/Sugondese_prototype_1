using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawner : NetworkBehaviour {

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Vector3 ballSpawnPosition;

	// Use this for initialization
	public override void OnStartServer() {
        var ball = Instantiate(ballPrefab, ballSpawnPosition, Quaternion.Euler(0, 0, 0));
        NetworkServer.Spawn(ball);
    }
}
