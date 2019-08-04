using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "DroneRoof")
            transform.SetParent(coll.transform);
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.name == "DroneRoof")
            transform.parent = null;
    }
}
