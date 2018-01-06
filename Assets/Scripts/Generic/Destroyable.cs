using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public GameObject explosionPrefab;
    public int startHealth;
    public int currentHealth;
    public int scoreOnKill;

    void Start() {
        currentHealth = startHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Destroy();
        }
        if (gameObject.CompareTag("Player")) {
            GameObject.Find("GameManager").GetComponent<GameManager>().SetPlayerHealth(currentHealth);
        }
    }
	public void Destroy() {
        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation, GameObject.FindGameObjectWithTag("Particles").transform);
        if (scoreOnKill > 0) {
            GameObject.Find("GameManager").GetComponent<GameManager>().AddToScore(scoreOnKill);
        }
        Destroy(gameObject);
    }

}
