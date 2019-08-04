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

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        isFalling = false;
        fallingTimer = 0f;
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
            else
                Destroy(gameObject);
        } else {
            // Being hit by a drone
            if (other.name == "Drone")
                Destroy(gameObject);
        }
    }

    public void Swapped(GameObject hero) {
        Debug.Log(hero.GetComponent<Rigidbody2D>().velocity);
        bool heroIsFalling = hero.GetComponent<Rigidbody2D>().velocity.y < 0;
        if (heroIsFalling) {
            isFalling = true;
            rbody.gravityScale = fallingGravity;
        }
        fallingTimer = 0f;
        wasSwapped = true;
    }
}
