using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipExplosion : MonoBehaviour {

    public ParticleSystem circleParticles;
    public ParticleSystem shrapnelParticles;
    public float shrapnelStartTime = 0.5f;
    public float totalTime = 2f;
    bool played;
    float timer;

	void Awake () {
        circleParticles.Play();
	}

	void Update () {
        timer += Time.deltaTime;

        if(timer > shrapnelStartTime && !played) {
            shrapnelParticles.Play();
            played = true;
        }

        if (timer > totalTime) {
            Destroy(gameObject);
        }
	}
}
