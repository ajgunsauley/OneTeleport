using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    private Collider2D doorCollider;
    private Animator animator;

    private void Start() {
        doorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void SetFieldActive(bool state) {
        doorCollider.enabled = state;
        animator.SetBool("IsEnabled", state);
    }
}
