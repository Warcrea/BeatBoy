using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUCKINGLOOK : MonoBehaviour {

    Transform target;

    // Use this for initialization
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("FUCK YOU IT's AT " + target);
    }
    void Update() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
    }


}
