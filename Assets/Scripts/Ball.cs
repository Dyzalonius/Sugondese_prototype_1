using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    Rigidbody2D rb;
    protected Vector3 direction;
    public bool onGround, flying, dying;
    [SerializeField] float minX, maxX, minY, maxY, maxSpeed, speedDropRate, speedDivisionOnBounce, speedCutOff;
    float speed;
    Vector3 originalPosition;

	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        speed = 0;
        Reset();
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

    protected virtual void Move() {
        var currentPos = rb.position;
        var newX = currentPos.x + direction.x * speed * Time.fixedDeltaTime * Time.timeScale;
        var newY = currentPos.y + direction.y * speed * Time.fixedDeltaTime * Time.timeScale;

        //invert speeds if outside boundaries
        if (newX < minX || newX > maxX) {
            direction.x = -direction.x;
            OnBounce();
        }
        if (newY < minY || newY > maxY) {
            direction.y = -direction.y;
            OnBounce();
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

    protected void ChangeDirection(Vector3 newDirection) {
        direction = newDirection;
    }

    public void Fire(Vector3 newDirection) {
        ChangeDirection(newDirection);
        flying = true;
        speed = maxSpeed;
    }

    protected virtual void OnBounce() {
        Kill();
    }

    public void OnBounce(Vector3 other) {
        Collide(other);
        OnBounce();
    }

    protected virtual void Kill() {
        speed = speed / speedDivisionOnBounce;
        dying = true;
        onGround = true;
        flying = false;
    }

    protected void Collide(Vector3 other) {
        direction = transform.position - other;
    }

    protected void Reset() {
        direction = Vector3.zero;
        onGround = true;
        flying = false;
        transform.position = originalPosition;
    }
}
