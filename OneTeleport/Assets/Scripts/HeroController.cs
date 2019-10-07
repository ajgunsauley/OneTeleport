using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    private Animator animator;
    private CameraShake cameraShake;
    private bool wasGrounded = true;
    private Rigidbody2D rbody;
    private EndStateController endState;

    // Group death SFX in the same object, detach it on deatch
    public GameObject deathFX;
    public AudioSource landSound;
    public LayerMask groundLayer;
    public Vector2 landDetectOffset, landShake;

    public Vector3 landSettings;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        rbody = GetComponent<Rigidbody2D>();
        endState = GameObject.FindGameObjectWithTag("EndZone").GetComponent<EndStateController>();
    }

    public void Die(bool playFrying = false) {
        DeathSource killer = (playFrying) ? DeathSource.Lava : DeathSource.Icicle;
        HeroDead dfx = Instantiate(deathFX, transform.position, Quaternion.identity)
            .GetComponent<HeroDead>();

        dfx.Play(killer, transform.rotation);

        endState.FailLevel();

        Destroy(gameObject);
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
        magnitude = Mathf.Clamp(magnitude, landSettings.x, landSettings.y);

        StartCoroutine(cameraShake.Shake(.15f, magnitude * landShake));
        landSound.Play();
    }

    private void FixedUpdate() {
        bool isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + landDetectOffset, .2f, groundLayer);
        if (!wasGrounded && isGrounded)
            Land();

        wasGrounded = isGrounded;
    }
}
