using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Transform player;
    public float speed;
    public float turnSpeed; 

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        transform.Translate(transform.up * (speed * Time.deltaTime), Space.World);
    }
}
