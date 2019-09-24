using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name.StartsWith("DroneRoof", System.StringComparison.Ordinal))
            transform.SetParent(coll.transform);
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.name.StartsWith("DroneRoof", System.StringComparison.Ordinal))
            transform.parent = null;
    }
}
