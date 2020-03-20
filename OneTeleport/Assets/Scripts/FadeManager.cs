using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private System.Action callback;
    public void FadeIn(System.Action callback) {
        animator.SetTrigger("FadeIn");
        this.callback = callback;
    }

    public void FadeOver() {
        callback.Invoke();
    }
}
