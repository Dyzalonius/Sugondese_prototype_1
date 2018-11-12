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
                Throw();
            }
            throwing = true;
        }
        else {
            throwing = false;
        }
    }

    // Throw ball
    void Throw() {
        if (balls.Count > 0) {
            var ball = balls[0];
            balls.RemoveAt(0);
            SortBalls();
            CmdThrow(ball.gameObject);
        }
    }

    // Throw ball
    [Command]
    void CmdThrow(GameObject ballObject) {
        //audioSourceThrow.Play();
        ballObject.transform.parent = null;
        Ball ball = ballObject.GetComponent<Ball>();
        ball.Fire(aimDirection);

        // rpc
        RpcSetParent(ballObject);
    }

    [ClientRpc]
    void RpcSetParent(GameObject ballObject) {
        ballObject.transform.parent = null;
    }

    // Pickup ball
    public void Pickup(Ball ball) {
        ball.transform.parent = transform;
        balls.Add(ball);
        ball.GetComponent<Ball>().Pickup();
        SortBalls();
    }

    // Sort balls
    void SortBalls() {
        for (int i = 0; i < balls.Count; i++) {
            balls[i].gameObject.transform.localPosition = ballPositions[i];
        }
    }

    // Get ballcount
    public int GetBallCount() {
        return balls.Count;
    }
}
