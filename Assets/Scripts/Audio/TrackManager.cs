using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Holds information about the currently playing tracks and their accompanying koreographs/eventIDs/components
*/
public class TrackManager : MonoBehaviour {

    private GameObject player;
    private RecordManager vinylManager;
    public List<AudioSource> tracks;
    public int currentTrack, previousTrack;


    public Dictionary<string, AudioSource> beatSources = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> leadSources = new Dictionary<string, AudioSource>();


    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        vinylManager = GameObject.Find("VinylManager").GetComponent<RecordManager>();
        MuteTracks(currentTrack);
        vinylManager.setSelected(currentTrack);
    }

    /*
    public string GetCurrentBeatEvent() {
        return beatTracks[currentBeat].eventId;
    }
    */

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            IncrementTrack();
            MuteTracks(currentTrack);
            player.GetComponent<PlayerShooting>().SetCurrentWeapon(currentTrack);
            vinylManager.setSelected(currentTrack);
        }
        if (Input.GetMouseButtonDown(1)) {
            DecrementTrack();
            MuteTracks(currentTrack);
            player.GetComponent<PlayerShooting>().SetCurrentWeapon(currentTrack);
            vinylManager.setSelected(currentTrack);
        }
    }

    void IncrementTrack() {
        previousTrack = currentTrack;
        if (currentTrack == tracks.Count - 1) {
            currentTrack = 0;
        }
        else {
            currentTrack++;
        }
    }

    void DecrementTrack() {
        previousTrack = currentTrack;
        if (currentTrack == 0) {
            currentTrack = tracks.Count-1;
        }
        else {
            currentTrack--;
        }
    }


    void MuteTracks(int index) {
        for (int i = 0; i < tracks.Count; i++) {
            if (i != index) {
                tracks[i].mute = true;
            }
            else {
                tracks[i].mute = false;
            }
        }
    }
}
