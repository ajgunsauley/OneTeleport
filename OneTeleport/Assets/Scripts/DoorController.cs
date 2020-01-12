using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    public bool state_ = true;

    private Collider2D doorCollider;
    private Animator animator;

    private void Start() {
        doorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        SetFieldActive(state_);
    }

    public void SetFieldActive(bool state) {
        state_ = state;
        doorCollider.enabled = state;
        animator.SetBool("IsEnabled", state);
    }
}
