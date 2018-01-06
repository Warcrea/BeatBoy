using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbMissile : MonoBehaviour {

    public float launchSpeed;
    public float speed;
    public float maxSpeed;
    public float acceleration;

    // Use this for initialization
    void Start () {
        speed = launchSpeed;
    }
	
	void FixedUpdate () {
        if (speed < maxSpeed) {
            speed += acceleration * Time.fixedDeltaTime;
        }
        transform.Translate(transform.up * (speed * Time.deltaTime), Space.World);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<Destroyable>().TakeDamage(1);
            GetComponent<Destroyable>().Destroy();
        }
    }
}
