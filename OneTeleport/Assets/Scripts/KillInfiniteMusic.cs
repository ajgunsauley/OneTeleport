using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillInfiniteMusic : MonoBehaviour {
    void Start() {
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        foreach (GameObject music in musics)
            Destroy(music);
    }
}
