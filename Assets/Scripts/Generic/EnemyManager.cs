using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private Transform player;
    private Transform enemyParent;
    public List<GameObject> waves;
    public int waveFrequency;
    public int totalWaveCount;
    private int wavesSinceLast;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyParent = GameObject.Find("Enemies").transform;
        Koreographer.Instance.RegisterForEvents("SpawnWave", SpawnEvent);
        wavesSinceLast = waveFrequency;
    }

    //Every few beats on the main track, trigger a random wave
    void SpawnEvent(KoreographyEvent koreoEvent) {
        wavesSinceLast++;
        if (wavesSinceLast > waveFrequency) {
            wavesSinceLast = 0;
            int r = Random.Range(0, waves.Count);
            SpawnWave(waves[r]);
        }
    }

    //Spawns a wave relative to the player
    void SpawnWave(GameObject wave) {
        totalWaveCount++;
        GameObject spanwedWave = Instantiate(wave, player.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360f)), enemyParent);
    }
}
