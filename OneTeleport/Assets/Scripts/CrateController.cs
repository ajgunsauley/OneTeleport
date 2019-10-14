using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour {
    public GameObject destroyFX;

    public void Break(BreakCause cause) {
        Instantiate(destroyFX, transform.position, Quaternion.identity)
            .GetComponent<CrateDestroy>()
            .Play(cause);

        Destroy(gameObject);
    }
}
