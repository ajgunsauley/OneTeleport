using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroy : MonoBehaviour {

    public void Play(BreakCause cause) {
        GetComponent<Animator>().SetTrigger(cause.ToString());
        Destroy(gameObject, 5f);
    }
}
