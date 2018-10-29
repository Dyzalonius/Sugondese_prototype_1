using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallElectricity : Ball {

    public GameObject electricityParticlePrefab;
    
    GameObject electricityParticle;

    protected override void Start() {
        base.Start();
    }

    protected override void Move() {
        base.Move();
    }

    protected override void OnBounce() {
        if (!dying) {
            Explode();
        }

        base.OnBounce();
    }

    protected override void Kill() {
        base.Kill();
    }

    void Explode() {
        electricityParticle = (Instantiate(electricityParticlePrefab, transform.position, Quaternion.Euler(0, 0, 0)));
    }

    public void OnDestroy() {
        Destroy(electricityParticle);
        Destroy(gameObject);
    }
}
