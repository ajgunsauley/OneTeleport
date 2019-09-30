using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour, ISwapResponder {
    Rigidbody2D rbody;
    public float speed = 1f;

    [Range(0f, 1f)]
    public float bounceSensitivity = .99f;

    private Vector2 direction = Vector2.right;
    private float switchTimer;

    private AudioSource pushSound;

    private readonly RigidbodyConstraints2D roamingConstraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    private readonly RigidbodyConstraints2D swappingConstraints = RigidbodyConstraints2D.FreezeRotation;

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = speed * direction;

        pushSound = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        // Restore roaming constraints
        rbody.constraints = roamingConstraints;

        if (Time.time > switchTimer && rbody.velocity.sqrMagnitude < speed * speed * bounceSensitivity) {
            switchTimer = Time.time + .1f;
            RotateDrone();
        }

        rbody.velocity = speed * direction;
    }

    void RotateDrone() {
        pushSound.Play();

        if (direction == Vector2.right)
            direction = Vector2.left;
        else
            direction = Vector2.right;
    }

    public void Swapped(GameObject hero) {
        // Temporarily remove physic constraints
        rbody.constraints = swappingConstraints;
    }
}
