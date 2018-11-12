using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [SerializeField] string inputType;
    [SerializeField] float maxSpeed;
    [SerializeField] GameObject character, crosshairPrefab;

    float speed;
    Rigidbody2D rigidBody;
    Vector3 aimDirection;
    bool throwing, allowThrow;
    List<Ball> balls;
    GameObject crosshair;

    Vector3[] ballPositions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(-0.1f, -0.1f, 0), new Vector3(0.1f, -0.1f, 0) }; //should make serializeable

    // Start
    void Start() {
        throwing = false;
        allowThrow = true;
        speed = maxSpeed;
        rigidBody = character.GetComponent<Rigidbody2D>();
        balls = new List<Ball>();
        aimDirection = new Vector3(0, 1, 0);

        if (inputType != "") {
            inputType = "_" + inputType;
        }
    }

    // StartLocalPlayer
    public override void OnStartLocalPlayer() {
        character.GetComponent<SpriteRenderer>().color = Color.blue;
        crosshair = Instantiate(crosshairPrefab);
        crosshair.transform.parent = transform;
    }

    // Update
    void Update() {
        if (!isLocalPlayer) return;

        Move();
        UpdateCrosshair();
        HandleFire();
    }

    // Movement
    void Move() {
        Vector2 currentPos = rigidBody.position;
        Vector2 direction = new Vector2(
            Input.GetAxis("MoveHorizontal" + inputType),
            Input.GetAxis("MoveVertical" + inputType)
        );

        direction.Normalize();
        direction *= Time.deltaTime * Time.timeScale * speed;

        transform.position = new Vector2(
            direction.x + currentPos.x,
            direction.y + currentPos.y
        );

        /*if (countdownLive) {
            direction.x = 0;
        }

        // force player to stay within boundaries
        player.transform.position = new Vector2(
            Mathf.Clamp(direction.x + currentPos.x, minX, maxX),
            Mathf.Clamp(direction.y + currentPos.y, minY, maxY)
        );*/
    }

    // Aim
    void UpdateCrosshair() {
        Vector3 newRotation = Vector3.right * Input.GetAxis("AimHorizontal" + inputType) + Vector3.up * Input.GetAxis("AimVertical" + inputType);

        // update if any input
        if (newRotation.sqrMagnitude > 0.0f) {
            newRotation.Normalize();
            aimDirection = newRotation;
            crosshair.transform.rotation = Quaternion.LookRotation(Vector3.forward, newRotation);
        }
    }

    // Fire
    void HandleFire() {
        if (Input.GetAxis("Fire" + inputType) > 0) {
            if (!throwing && allowThrow) {
                TryThrow();
            }
            throwing = true;
        }
        else {
            throwing = false;
        }
    }

    // Throw ball local
    void TryThrow() {
        if (balls.Count > 0) {
            var ball = balls[0];
            balls.RemoveAt(0);
            SortBalls();

            CmdThrow(ball.gameObject, transform.position, aimDirection);
        }
    }

    // Throw ball server
    [Command]
    void CmdThrow(GameObject ballObject, Vector3 position, Vector3 aimDirection) {
        //audioSourceThrow.Play();
        RpcThrow(ballObject, position, aimDirection);
    }

    // Throw ball clients
    [ClientRpc]
    void RpcThrow(GameObject ballObject, Vector3 position, Vector3 aimDirection) {
        ballObject.transform.parent = null;
        ballObject.transform.position = position;
        ballObject.GetComponent<Ball>().Fire(aimDirection);
    }

    // Pickup ball
    public void TryPickup(Ball ball) {
        if (balls.Count < 3) {
            balls.Add(ball);
            SortBalls();

            CmdPickup(ball.gameObject, gameObject);
        }
    }

    [Command]
    void CmdPickup(GameObject ballObject, GameObject parentObject) {
        RpcPickup(ballObject, parentObject);
    }

    [ClientRpc]
    void RpcPickup(GameObject ballObject, GameObject parentObject) {
        ballObject.GetComponent<Ball>().Pickup();
        ballObject.transform.parent = parentObject.transform;
        ballObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    // Sort balls
    void SortBalls() {
        for (int i = 0; i < balls.Count; i++) {
            balls[i].gameObject.transform.localPosition = ballPositions[i];
        }
    }
}
