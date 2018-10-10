using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCurve : Ball {

    public float curveAmount;
    float angle;

    protected override void Start() {
        base.Start();
    }

    protected override void Move() {
        // convert direction to angle
        angle = Vector3.SignedAngle(direction, Vector3.right, Vector3.back);

        // calc new angle
        angle += curveAmount / (Time.deltaTime * Time.timeScale);

        // convert new angle to new direction
        Vector3 newDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        newDirection.Normalize();
        
        ChangeDirection(newDirection);

        base.Move();
    }
}
