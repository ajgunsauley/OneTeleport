using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour, ISwapResponder {
    //public UIController UI;
    public EndStateController endStateController;
    public LayerMask rayFallingMask;
    public float gigglingTime = .2f;
    public float fallingGravity = 4f;

    private Rigidbody2D rbody;
    private bool isFalling;
    private float fallingTimer;
    private bool wasSwapped;

    private AudioSource breakSound;

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        isFalling = false;
        fallingTimer = 0f;

        breakSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        // Once the player swapped with the icicle, it can't fall any further
        if (wasSwapped)
            return;

        if (isFalling == false) {
            if (fallingTimer == 0f) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, rayFallingMask);
                if (hit && hit.transform.name == "Hero")
                    fallingTimer = Time.time + gigglingTime;
            } else if (Time.time >= fallingTimer) {
                isFalling = true;
                rbody.gravityScale = fallingGravity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (isFalling) {
            // Hitting poor chunky boy
            if (other.name == "Hero") {
                other.GetComponent<HeroController>().Die(false);
                endStateController.FailLevel();
            }
            // Hitting a crate/drone
            else if (other.name == "Crate" || other.name == "Drone")
                Destroy(other.gameObject);
        } else {
            // Being hit by a drone
            if (other.name == "Drone")
                Break();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isFalling) {
            Collider2D col = collision.collider;
            if (col.name != "Hero" && col.name != "Crate" && col.name != "Drone")
                Break(); // We're hitting the ground
        }
    }

    public void Swapped(GameObject hero) {
        bool heroIsFalling = hero.GetComponent<Rigidbody2D>().velocity.y < 0;
        if (heroIsFalling) {
            isFalling = true;
            rbody.gravityScale = fallingGravity;
        }
        fallingTimer = 0f;
        wasSwapped = true;
    }

    public void Break() {
        // Prevent break sound from playing twice
        if (breakSound.isPlaying == false)
            breakSound.Play();

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 1f);
    }
}
