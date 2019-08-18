using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public IEnumerator Shake(float shake, Vector2 magnitude) {
        Vector3 originalPos = transform.localPosition;

        float shakeEnd = Time.time + shake;
        while (Time.time < shakeEnd) {
            float x = Random.Range(-1f, 1f) * magnitude.x;
            float y = Random.Range(-1f, 1f) * magnitude.y;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
