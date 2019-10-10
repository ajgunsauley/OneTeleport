using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BreakParams {
    public string animationTrigger;
    public AudioClip sfx;
    public Vector2 pitchRange;
}

public enum BreakCause { Break, Melt };

public class IcicleDestroy : MonoBehaviour {
    private Animator animator;
    private AudioSource sound;

    public BreakParams breaking, melting;

    private void Awake() {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    public void Play(BreakCause cause) {
        BreakParams bp = (cause == BreakCause.Melt) ? melting : breaking;

        animator.SetTrigger(bp.animationTrigger);

        sound.pitch = Random.Range(bp.pitchRange.x, bp.pitchRange.y);
        sound.clip = bp.sfx;
        sound.Play();

        Destroy(gameObject, 2f);
    }
}
