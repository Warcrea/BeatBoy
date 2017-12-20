using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Holds information about the currently playing tracks and their accompanying koreographs/eventIDs/components
*/
public class TrackManager : MonoBehaviour {

    private GameObject player;
    public List<Track> beatTracks;
    public List<Track> leadTracks;
    public int currentBeat, previousBeat;
    public int currentLead, previousLead;
    public Dictionary<string, AudioSource> beatSources = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> leadSources = new Dictionary<string, AudioSource>();

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        AudioSource[] beatSourceArray = GameObject.FindGameObjectWithTag("Beats").GetComponents<AudioSource>();
        foreach (AudioSource source in beatSourceArray) {
            beatSources.Add(source.clip.name, source);
        }

        AudioSource[] leadSourceArray = GameObject.FindGameObjectWithTag("Leads").GetComponents<AudioSource>();
        foreach (AudioSource source in leadSourceArray) {
            leadSources.Add(source.clip.name, source);
        }

        foreach (Track track in beatTracks){
            AudioSource src = null;
            Debug.Log(track.audioClipName);
            bool success = beatSources.TryGetValue(track.audioClipName, out src);
            if (success) {
                track.audioSource = src;
                track.audioSource.mute = true;
            }
            else
                Debug.Log("Couldn't find audio source for " + track.audioClipName + " | " + track.name);
        }

        foreach (Track track in leadTracks) {
            AudioSource src;
            bool success = leadSources.TryGetValue(track.audioClipName, out src);
            if (success)
                track.audioSource = src;
            else
                Debug.Log("Couldn't find audio source for " + track.audioClipName + " | " + track.name);
        }

        MuteTracks(beatTracks, currentBeat);
        MuteTracks(leadTracks, currentLead);
    }

    public string GetCurrentBeatEvent() {
        return beatTracks[currentBeat].eventId;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            IncrementLead();
            //MuteTracks(leadTracks, currentLead);
            //player.GetComponent<PlayerShooting>().RegisterForOneTrack(leadTracks[previousLead].eventId, leadTracks[currentLead].eventId);
            player.GetComponent<PlayerShooting>().SetCurrentWeapon(currentLead);
        }
        if (Input.GetMouseButtonDown(1)) {
            IncrementBeat();
            MuteTracks(beatTracks, currentBeat);
       
            player.GetComponent<PlayerShooting>().RegisterForOneTrack(beatTracks[previousBeat].eventId, beatTracks[currentBeat].eventId);
        }
    }

    void IncrementBeat() {
        previousBeat = currentBeat;
        if (currentBeat == beatTracks.Count - 1) {
            currentBeat = 0;
        } else {
            currentBeat++;
        }
    }

    void IncrementLead() {
        previousLead = currentLead;
        if (currentLead == leadTracks.Count - 1) {
            currentLead = 0;
        }
        else {
            currentLead++;
        }
    }


    void MuteTracks(List<Track> trackList, int index) {
        for (int i = 0; i < trackList.Count; i++) {
            if (i != index) {
                trackList[i].audioSource.mute = true;
            }
            else {
                trackList[i].audioSource.mute = false;
            }
        }
    }
}
