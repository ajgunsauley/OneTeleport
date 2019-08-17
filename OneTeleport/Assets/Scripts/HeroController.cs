using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    private Animator animator;

    // Group death SFX in the same object, detach it on deatch
    public GameObject deathSFX;
    public AudioSource glubSound, frySound;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Die(bool playFrying = false) {
        // Detach SFX so we can destroy the Hero imediatly, but keep the sound playing
        deathSFX.transform.parent = null;

        // Play the death sound
        glubSound.enabled = true;
        frySound.enabled = true;

        glubSound.Play();
        if (playFrying) frySound.Play();

        // Destroy both object at different point in time
        Destroy(gameObject);
        Destroy(deathSFX, 1f);
    }

    public void Win() {
        animator.SetTrigger("DoWin");
        GetComponentInChildren<LineRenderer>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 2f);
    }
}