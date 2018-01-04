using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour {

    public bool selfDestruct; //Explode when coming into contact

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            //Do damage
            if (selfDestruct) {
                col.gameObject.GetComponent<Destroyable>().TakeDamage(1);
                GetComponent<Destroyable>().Destroy();
            }
        }
    }
}
