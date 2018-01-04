using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject missilePrefab;
    private GameObject projectilesHolder;

    private Transform player;
    public float speed;
    public float turnSpeed;
    public float fireFrequency;
    private float timer;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        projectilesHolder = GameObject.Find("Projectiles");
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        if (timer >= fireFrequency) {
            FireMissile();
            timer = 0;
        }
    }

    void FireMissile() {
        Instantiate(missilePrefab, transform.position, transform.rotation, projectilesHolder.transform);
    }
}
