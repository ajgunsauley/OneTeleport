﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGun : MonoBehaviour {
    public LayerMask rayIntersectLayers;

    public ParticleSystem vfx;
    public Gradient swapColor, cantSwapColor;

    private Rigidbody2D rbody;
    private LineRenderer rayLine;

    private Rigidbody2D swapObject;
    private bool doSwap;

    private ParticleSystem.MainModule mainPS;
    private ParticleSystem.MinMaxGradient swapColorPS, cantSwapColorPS;

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        rayLine = GetComponentInChildren<LineRenderer>();

        mainPS = vfx.main;
        swapColorPS = new ParticleSystem.MinMaxGradient(swapColor);
        cantSwapColorPS = new ParticleSystem.MinMaxGradient(cantSwapColor);
    }

    // Update is called once per frame
    void Update() {
        Vector3 gamepadDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (gamepadDirection.sqrMagnitude > 0.25f)
            ? gamepadDirection
            : mousePos - transform.position;
        aimDirection.z = 0;

        swapObject = null;

        // Hide the ray
        rayLine.colorGradient = cantSwapColor;
        rayLine.SetPosition(0, transform.position);
        rayLine.SetPosition(1, transform.position);

        mainPS.startColor = cantSwapColorPS;
        vfx.transform.position = new Vector3(0, 0, -1000);

        if (aimDirection.sqrMagnitude > 0.25f) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, 100f, rayIntersectLayers);

            if (hit) {
                swapObject = hit.transform.CompareTag("Swappable") ? hit.rigidbody : null;
                if (swapObject != null) {
                    rayLine.colorGradient = swapColor;
                    mainPS.startColor = swapColorPS;
                }

                rayLine.SetPosition(1, hit.point);

                vfx.transform.position = hit.point;
                vfx.transform.rotation = Quaternion.Euler(0f, 0f,
                    Vector2.SignedAngle(Vector2.up, hit.normal));
            } else {
                swapObject = null;
                rayLine.SetPosition(1, transform.position + 100f * aimDirection);
            }
        }

        // The `or` prevent doSwap to be cleared if FixedUpdate didn't consume the action
        doSwap |= Input.GetButtonDown("Fire1");
    }

    private void FixedUpdate() {
        if (doSwap) {
            if (swapObject != null) {
                swapObject.constraints = RigidbodyConstraints2D.None;

                Vector2 sourcePosition = rbody.position;
                Vector2 targetPosition = swapObject.position;

                if (swapObject.name.StartsWith("Drone", System.StringComparison.Ordinal)) {
                    targetPosition.x = RoundCoord(targetPosition.x);
                    targetPosition.y = RoundCoord(targetPosition.y);
                }

                rbody.position = targetPosition;
                swapObject.position = sourcePosition;

                ISwapResponder sr = swapObject.gameObject.GetComponent<ISwapResponder>();
                if (sr != null) sr.Swapped(gameObject);
            }

            doSwap = false;
        }
    }

    private float RoundCoord(float a) {
        return Mathf.Floor(2f * a + 0.5f) / 2f;
    }
}
