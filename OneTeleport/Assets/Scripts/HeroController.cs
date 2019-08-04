using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Win() {
        animator.SetTrigger("DoWin");
        Destroy(gameObject, 2f);
    }
}