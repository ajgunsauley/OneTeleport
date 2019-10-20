using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Song {
    public float introOffset;
    public AudioClip introClip, loopClip;
}

public class InfiniteMusic : MonoBehaviour {
    public Song[] songs;

    private int songIndex;
    private AudioSource source;

    // Start is called before the first frame update
    void Start() {
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        if (musics.Length > 1) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();

        foreach (Song song in songs) {
            if (song.introClip != null)
                song.introClip.LoadAudioData();

            if (song.loopClip != null)
                song.loopClip.LoadAudioData();
        }

        PlaySong(Random.Range(0, songs.Length));
    }

    public void PlayNext() {
        PlaySong(songIndex + 1);
    }

    public void PlaySong(int index) {
        songIndex = index % songs.Length;
        Song song = songs[songIndex];

        source.Stop();
        source.clip = song.loopClip;

        float offset = 0;
        if (song.introClip != null) {
            source.PlayOneShot(song.introClip);
            offset = song.introClip.length + song.introOffset;
        }
        source.PlayDelayed(offset);
    }
}
