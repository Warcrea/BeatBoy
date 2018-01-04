using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public GameObject explosionPrefab;
    public int startHealth;
    public int currentHealth;

    void Start() {
        currentHealth = startHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Destroy();
        }
    }
	public void Destroy() {
        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation, GameObject.FindGameObjectWithTag("Particles").transform);
        Destroy(gameObject);
    }

}
