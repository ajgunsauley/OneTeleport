using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour {
    public GameObject destroyFX;

    public void Break(BreakCause cause) {
        Instantiate(destroyFX, transform.position, transform.rotation)
            .GetComponent<CrateDestroy>()
            .Play(cause);

        Destroy(gameObject);
    }
}
