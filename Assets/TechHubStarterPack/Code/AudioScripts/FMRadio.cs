using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script for selecting songs from a list of songs, or playing random songs at the start
public class FMRadio : MonoBehaviour {

    [Header("The Speaker Plays the music")]
    public AudioSource speaker;
    public List<AudioClip> songs;
    public int currentSongID = 0;

    [Header(" --- Settings --- ")]
    [Header("Select a random song from the playlist")]
    public bool playOnStart = true;
    public bool selectRandomSong = true;
    public bool continiousPlay = true;

    [Header("Debug")]
    public bool DEBUG_MODE = true;

    // Starting Settings
    public void Start(){
        if (speaker == null) {
            speaker = GetComponent<AudioSource>();
        }

        // [ ] Debug check for existance
        if(playOnStart){
            if (selectRandomSong) {
                PlayRandomSong();
            } else {
                PlaySong(0);
            }
        }


    }

    public void PlayRandomSong(){
        int randomSongID = Random.Range(0, songs.Count);
        if(DEBUG_MODE) { Debug.Log("Music Playing: Track : " + randomSongID); }

        PlaySong(randomSongID);
    }

    // Play a single song
    public void PlaySong(int selectedTrack){

        if ( speaker != null && songs!= null){

            if ( songs.Count > selectedTrack && selectedTrack >= 0 ){


                AudioClip toPlay = songs[selectedTrack];

                if ( toPlay != null){

                    currentSongID = selectedTrack;

                    speaker.clip = toPlay;
                    speaker.Play();

                }else{ Debug.LogWarning("The Song Selected was null for id : " + selectedTrack); } 
            }else{ Debug.LogWarning("The Song ID is invalid and out of range : " + selectedTrack); }
        }else{ Debug.LogWarning(" Either the speaker or the song list is null"); }
    }

    public void SkipTrack(){
        if (selectRandomSong) {
            PlayRandomSong();
        } else {
            int nextSong = currentSongID + 1;
            // if the next song would be out the end of the list, loop back around to the beginning
            if( nextSong >= songs.Count){
                nextSong = 0;
            }

            PlaySong(nextSong);
        }

        if( DEBUG_MODE){ Debug.Log("Skip Track Pressed"); }
    }

	// Update is called once per frame
	void Update () {
		if(continiousPlay && speaker.isActiveAndEnabled){
            // no song is playing, select the next song
            if( !speaker.isPlaying){
                if (selectRandomSong) {
                    PlayRandomSong();
                } else {
                    PlaySong(0);
                }
            }
        }
    }
}
