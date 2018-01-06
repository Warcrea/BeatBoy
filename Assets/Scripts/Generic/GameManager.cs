using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool dead;
    public int score;
    public int health;
    public Text healthCounter;
    public Text scoreCounter;

    public Text gameOverText;
    public Text scoreText;
    public Text restartText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
            if (Input.GetKeyDown(KeyCode.R)){
                SceneManager.LoadScene(0);
            }
        }
	}

    public void SetPlayerHealth(int health) {
        this.health = Mathf.Max(0, health);
        healthCounter.text = "Health : " + this.health.ToString();

        if (health < 1) {
            dead = true;
            DisplayGameOverText();
        }
    }

    public void DisplayGameOverText() {
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "You scored " + score + " points";
        restartText.gameObject.SetActive(true);
    }

    public void AddToScore(int score) {
        this.score += score;
        scoreCounter.text =  this.score.ToString();
    }
}
