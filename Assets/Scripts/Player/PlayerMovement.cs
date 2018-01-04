using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 1f;
    public float sensitivity = 0.1f;
    public Rigidbody2D rigidbody;
    private float turnSmoothing = 5f;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float leftStickx = Input.GetAxis("Horizontal");
        float leftSticky = Input.GetAxis("Vertical");

        transform.Translate(leftStickx * Time.deltaTime * speed, leftSticky * Time.deltaTime * speed, 0, Space.World);

        if(!(leftStickx == 0 && leftSticky == 0)) {
            Vector3 targetDirection = new Vector3(leftStickx, leftSticky, 0f);
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
       
            // Change the players rotation to this new rotation.
            transform.rotation = (newRotation);
        }

    }
}
