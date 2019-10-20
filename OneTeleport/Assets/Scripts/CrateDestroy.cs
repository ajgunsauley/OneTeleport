using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroy : MonoBehaviour {
    public GameObject sprite;

    public void Play(BreakCause cause, Quaternion spriteRot) {
        GetComponent<Animator>().SetTrigger(cause.ToString());
        sprite.transform.rotation = spriteRot;
        Destroy(gameObject, 5f);
    }
}
