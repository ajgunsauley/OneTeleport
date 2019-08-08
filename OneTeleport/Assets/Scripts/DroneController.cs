using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {
    Rigidbody2D rbody;
    public float speed = 1f;

    private Vector2 direction = Vector2.right;
    private bool switched = true;

    private AudioSource pushSound;

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = speed * direction;

        pushSound = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (!switched && rbody.velocity.sqrMagnitude < speed * speed * .99f) {
            switched = true;
            RotateDrone();
        } else {
            switched = false;
        }

        rbody.velocity = speed * direction;
    }

    void OnTriggerEnter2D(Collider2D other) {
        //if icicle is not moving, drone kills it and then flips direction
        if (other.name == "Icicle") {
            var icicleSpeed = other.GetComponent<Rigidbody2D>().velocity;
            if (icicleSpeed.y == 0) {
                Destroy(other.gameObject);
                RotateDrone();
            }
        }

    }

    void RotateDrone() {
        pushSound.Play();

        if (direction == Vector2.right)
            direction = Vector2.left;
        else
            direction = Vector2.right;
    }
}
