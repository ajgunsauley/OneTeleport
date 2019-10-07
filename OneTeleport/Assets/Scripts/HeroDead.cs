using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathSource {
    Icicle, Lava
}

public class HeroDead : MonoBehaviour {
    public AudioSource glubSound, frySound;
    public GameObject sprite;

    public void Play(DeathSource killer, Quaternion rotation) {
        sprite.transform.rotation = rotation;

        GetComponent<Animator>().SetTrigger(killer.ToString());

        glubSound.Play();
        if (killer == DeathSource.Lava)
            frySound.Play();
    }
}
