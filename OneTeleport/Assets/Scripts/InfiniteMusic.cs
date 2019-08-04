using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMusic : MonoBehaviour {
    private AudioSource source;

    // Start is called before the first frame update
    void Start() {
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        if (musics.Length > 1)
            Destroy(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
        }
    }

    public void PlayMusic() {
        source.Play();
    }

    public void StopMusic() {
        source.Stop();
    }
}
