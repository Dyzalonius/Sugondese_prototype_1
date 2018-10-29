using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallWater : BallBounce {

    public float timeBetweenWaterParticles;
    float currentTimeBetweenWaterParticles;
    public GameObject waterParticle;
    public List<GameObject> waterParticles;
    bool isActive, isElectrocuted;

    protected override void Start() {
        isActive = false;
        isElectrocuted = false;
        base.Start();
    }

    protected override void Move() {
        if (isActive) {
            currentTimeBetweenWaterParticles += Time.fixedDeltaTime * Time.timeScale;
            if (currentTimeBetweenWaterParticles > timeBetweenWaterParticles) {
                currentTimeBetweenWaterParticles = 0;
                Explode();
            }
        }

        base.Move();
    }

    protected override void OnBounce() {
        if (!dying) {
            isActive = true;
            isElectrocuted = false;
        }

        base.OnBounce();
    }

    protected override void Kill() {
        isActive = false;
        base.Kill();
    }

    void Explode() {
        GameObject particle = Instantiate(waterParticle, transform.position, Quaternion.Euler(0, 0, 0));
        particle.GetComponent<WaterEffect>().Initialize(this, isElectrocuted);
        waterParticles.Add(particle);
    }

    public void Electrocute() {
        isElectrocuted = true;
        for (int i = waterParticles.Count - 1; i >= 0; i--) {
            if (waterParticles[i] != null) {
                waterParticles[i].GetComponent<WaterEffect>().Electrocute();
            }
        }
        Invoke("ElectrocuteStop", 0.5f);
    }

    void ElectrocuteStop() {
        isElectrocuted = false;
        for (int i = waterParticles.Count - 1; i >= 0; i--) {
            if (waterParticles[i] != null) {
                waterParticles[i].GetComponent<WaterEffect>().ElectrocuteStop();
            }
        }
    }

    public void OnDestroy() {
        for (int i = waterParticles.Count - 1; i >= 0; i--) {
            var particle = waterParticles.ElementAt(i);
            waterParticles.RemoveAt(i);
            Destroy(particle);
        }
        Destroy(gameObject);
    }
}
