using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    private Animator animator;
    private CameraShake cameraShake;
    private bool wasGrounded = true;
    private Rigidbody2D rbody;

    // Group death SFX in the same object, detach it on deatch
    public GameObject deathSFX;
    public AudioSource glubSound, frySound;
    public LayerMask groundLayer;
    public Vector2 landDetectOffset, landShake;

    public Vector3 landSettings;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        rbody = GetComponent<Rigidbody2D>();
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

    private void Land() {
        float magnitude = rbody.velocity.magnitude;
        magnitude = Mathf.Pow(magnitude, landSettings.z);
        Debug.Log("LANDING " + magnitude);
        magnitude = Mathf.Clamp(magnitude, landSettings.x, landSettings.y);

        StartCoroutine(cameraShake.Shake(.15f, magnitude * landShake));
    }

    private void FixedUpdate() {
        //Debug.Log(Physics2D.OverlapCircle((Vector2)transform.position + landDetectOffset, .5f, groundLayer));
        bool isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + landDetectOffset, .2f, groundLayer);
        //Debug.Log(wasGrounded + " => " + isGrounded);
        if (!wasGrounded && isGrounded)
            Land();

        wasGrounded = isGrounded;
    }
}
