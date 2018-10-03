using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBall : MonoBehaviour {

    Rigidbody2D rb;
    Vector3 direction;
    public bool onGround, flying, dying;
    [SerializeField] float minX, maxX, minY, maxY, maxSpeed, speedDropRate, speedDivisionOnKill, speedCutOff;
    float speed;
    Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        Reset();
        speed = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (flying || dying) {
            Move();
        }
        if (dying) {
            SpeedDrop();
        }
    }

    void Move() {
        var currentPos = rb.position;
        var newX = currentPos.x + direction.x * speed * Time.deltaTime * Time.timeScale;
        var newY = currentPos.y + direction.y * speed * Time.deltaTime * Time.timeScale;

        //invert speeds if outside boundaries
        if (newX < minX || newX > maxX) {
            direction.x = -direction.x;
            Kill();
        }
        if (newY < minY || newY > maxY) {
            direction.y = -direction.y;
            Kill();
        }

        // force ball to stay within boundaries
        rb.position = new Vector2(
            Mathf.Clamp(newX, minX, maxX),
            Mathf.Clamp(newY, minY, maxY)
        );
    }

    void SpeedDrop() {
        speed *= Mathf.Pow(speedDropRate, Time.timeScale); // introduce Time.deltaTime and Time.timeScale properly

        if (speed <= speedCutOff) {
            speed = 0;
            dying = false;
        }
    }

    public void Pickup() {
        onGround = false;
        dying = false;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Fire(Vector3 newDirection) {
        direction = newDirection;
        flying = true;
        speed = maxSpeed;
    }

    public void Kill() {
        speed = speed / speedDivisionOnKill;
        dying = true;
        onGround = true;
        flying = false;
    }

    public void Kill(Vector3 other) {
        Collide(other);
        Kill();
    }

    public void Collide(Vector3 other) {
        direction = transform.position - other;
    }

    public void Reset() {
        direction = Vector3.zero;
        onGround = true;
        flying = false;
        transform.position = originalPosition;
    }
}
