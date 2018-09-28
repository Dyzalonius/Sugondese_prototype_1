using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBall : MonoBehaviour {

    Rigidbody2D rb;
    Vector3 direction;
    public bool onGround, flying, dying;
    [SerializeField] float minX, maxX, minY, maxY, maxSpeed, speedDropRate;
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
	void Update () {
        if (flying) {
            Move();
        }
        if (dying) {
            //speed *= speedDropRate * Time.deltaTime; // FIX THIS, it now drops linearly which is ugly
            speed -= speedDropRate * Time.deltaTime;
            if (speed <= 0) {
                speed = 0;
                flying = false;
                onGround = true;
                dying = false;
            }
        }
    }

    void Move() {
        var currentPos = rb.position;
        var newX = currentPos.x + direction.x * speed * Time.deltaTime * Time.timeScale;
        var newY = currentPos.y + direction.y * speed * Time.deltaTime * Time.timeScale;

        //invert speeds if outside boundaries
        if (newX < minX || newX > maxX) {
            direction.x = -direction.x;
            dying = true;
        }
        if (newY < minY || newY > maxY) {
            direction.y = -direction.y;
            dying = true;
        }

        // force ball to stay within boundaries
        rb.position = new Vector2(
            Mathf.Clamp(newX, minX, maxX),
            Mathf.Clamp(newY, minY, maxY)
        );
    }

    public void Pickup() {
        onGround = false;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Fire(Vector3 newDirection) {
        direction = newDirection;
        flying = true;
        speed = maxSpeed;
    }

    public void Reset() {
        direction = Vector3.zero;
        onGround = true;
        flying = false;
        transform.position = originalPosition;
    }
}
