using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public GameObject explosionPrefab;

	public void Destroy() {
        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation, GameObject.FindGameObjectWithTag("Particles").transform);
        Destroy(gameObject);
    }

}
