using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent onEnable, onDisable;

    // We don't know the order of OnEnter/OnExit during a swap
    // So we need to keep track of how many colliders we met
    private int colliderCount;
    private bool isEnabled;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag(tag))
            colliderCount++;

        if (isEnabled == false && colliderCount > 0) {
            isEnabled = true;
            onEnable.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.CompareTag(tag))
            colliderCount--;

        if (isEnabled == true && colliderCount == 0) {
            isEnabled = false;
            onDisable.Invoke();
        }
    }
}
