using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    private Animator animator;

    public AudioSource glubSound, frySound;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Die(bool playFrying = false) {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponentInChildren<LineRenderer>().enabled = false;
        glubSound.Play();
        if (playFrying) frySound.Play();
    }

    public void Win() {
        animator.SetTrigger("DoWin");
        Destroy(gameObject, 2f);
    }
}