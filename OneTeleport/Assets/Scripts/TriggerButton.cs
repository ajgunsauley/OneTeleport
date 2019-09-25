using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerButton : MonoBehaviour
{
    public UnityEvent onEnable, onDisable;
    private Animator animator;

    // We don't know the order of OnEnter/OnExit during a swap
    // So we need to keep track of how many colliders we met
    private int colliderCount;
    private bool isEnabled;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag(tag))
            colliderCount++;

        if (isEnabled == false && colliderCount > 0) {
            isEnabled = true;
            animator.SetBool("IsEnabled", true);
            onEnable.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.CompareTag(tag))
            colliderCount--;

        if (isEnabled == true && colliderCount == 0) {
            isEnabled = false;
            animator.SetBool("IsEnabled", false);
            onDisable.Invoke();
        }
    }
}
