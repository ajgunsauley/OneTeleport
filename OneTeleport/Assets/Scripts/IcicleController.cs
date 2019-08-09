using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour, ISwapResponder {
    //public UIController UI;
    public EndStateController endStateController;
    public LayerMask rayFallingMask;
    public float gigglingTime = .2f;

    private Rigidbody2D rbody;
    private float fallingTimer;
    private bool wasSwapped;
    private bool isFalling;

    private AudioSource breakSound;

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        breakSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        // Once the player swapped with the icicle, it can't fall any further
        if (wasSwapped)
            return;

        if (IsFalling() == false) {
            if (fallingTimer == 0f) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, rayFallingMask);
                if (hit && hit.transform.name == "Hero")
                    fallingTimer = Time.time + gigglingTime;
            } else if (Time.time >= fallingTimer) {
                Fall();
            }
        }
    }

    private void FixedUpdate() {
        // Re-enable appropriate constraints (necessary after swap)
        rbody.constraints = (IsFalling() || wasSwapped)
            ? RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX
            : RigidbodyConstraints2D.FreezeAll;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (IsFalling()) {
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
        if (IsFalling()) {
            Collider2D col = collision.collider;
            if (col.name != "Hero" && col.name != "Crate" && col.name != "Drone")
                Break(); // We're hitting the ground
        }
    }

    public void Swapped(GameObject hero) {
        // Remove Position constraints to allow swapping
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // If the hero swap while falling, the icicle must fall too
        bool heroIsFalling = hero.GetComponent<Rigidbody2D>().velocity.y < 0;
        if (heroIsFalling)
            Fall();

        // After a swap, we won't fall anymore. So...
        wasSwapped = true; // Disable player detection
        fallingTimer = 0f; // Stop giggling
    }

    public bool IsFalling() {
        return isFalling;
    }

    public void Fall() {
        // Remove FreezeY to let the icicle fall
        isFalling = true;
        rbody.constraints ^= RigidbodyConstraints2D.FreezePositionY;
    }

    public void Break() {
        // Prevent break sound from playing twice
        if (breakSound.isPlaying == false)
            breakSound.Play();

        // Disable components that affect the game
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 1f);
    }
}
