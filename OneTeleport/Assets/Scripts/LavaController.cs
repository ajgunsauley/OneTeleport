using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;
        if (other.name.StartsWith("Hero", System.StringComparison.Ordinal)) {
            other.GetComponent<HeroController>().Die(true);
        }
    }
}
