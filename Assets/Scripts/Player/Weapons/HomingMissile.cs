using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { seek, accelerate };

/*
    1. Be launched from ship in direction of mouse
    2. Decellerate to mostly standstill
    3. Rotate towards nearest enemy in 30 degree arc infront of missile
    4. Accelerate, turn on particles
    5. While in acceleration mode, can only adjust rotation slowly
*/
public class HomingMissile : MonoBehaviour {
    public State state;
    public Transform target;

    private float maxDistance = 400f;

    public float launchSpeed = 0.5f;
    public float speed;
    public float maxSpeed = 50f;
    public float acceleration = 50f;
    public float seekTurnSpeed = 0.001f;
    public float accelerateTurnSpeed = 0.1f;

    private float seekLength = 0.5f;
    private float timer;

    void Start() {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        //Vector3.RotateTowards()
        timer = 0;
        state = State.seek;
        speed = launchSpeed;
        target = null;
    }

    void FixedUpdate() {
        timer += Time.fixedDeltaTime;

        if (state == State.seek) {
            if (timer >= seekLength) {
                state = State.accelerate;
                timer = 0;
            }
            else {
                if (target == null) {
                    GameObject enemy = FindClosestEnemy();
                    if (enemy != null) {
                        target = enemy.transform;
                    }
                }
                else {
                    Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, seekTurnSpeed * Time.deltaTime);
                }
                //if (speed > 0.1f) {
                //    speed = Mathf.Max(0.1f, acceleration * Time.fixedDeltaTime);
                //}
            }
        }
        else if (state == State.accelerate) {
            if (speed < maxSpeed) {
                speed += acceleration * Time.fixedDeltaTime;
            }
            if (target != null) {
                Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, seekTurnSpeed * Time.deltaTime);
            }
        }

        transform.Translate(transform.up * (speed * Time.deltaTime), Space.World);

    }

    void OnDrawGizmos() {
        Color color;
        color = Color.green;
        DrawHelperAtCenter(this.transform.up, color, 2f);
    }

    private void DrawHelperAtCenter(
       Vector3 direction, Color color, float scale) {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }


    //Todo - 
    GameObject FindClosestEnemy() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance <= maxDistance) {
                Debug.Log(curDistance);
                closest = go;
                distance = curDistance;
            }
        }
        if (closest == null) Debug.Log("Couldn't find an enemy");
        return closest;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Destroyable>().Destroy();
            //add an explosion or something
            Destroy(gameObject);
        }
    }
}
